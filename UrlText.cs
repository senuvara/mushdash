using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UrlText : Text, IPointerClickHandler, IEventSystemHandler
{
	public delegate void VoidOnHrefClick(string hefName);

	private class HrefInfo
	{
		public int startIndex;

		public int endIndex;

		public string name;

		public readonly List<Rect> boxes = new List<Rect>();
	}

	public float lineW = 0.3f;

	public float num;

	private string m_OutputText;

	private readonly UIVertex[] m_TempVerts = new UIVertex[4];

	public static readonly Regex s_HrefRegex = new Regex("<link=([^>\\n\\s]+)>(.*?)(</link>)", RegexOptions.Singleline);

	private readonly List<HrefInfo> m_HrefInfos = new List<HrefInfo>();

	protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

	public VoidOnHrefClick onHrefClick;

	public override void SetVerticesDirty()
	{
		base.SetVerticesDirty();
		m_OutputText = GetOutputText(text);
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (base.font == null)
		{
			return;
		}
		m_DisableFontTextureRebuiltCallback = true;
		Vector2 size = base.rectTransform.rect.size;
		TextGenerationSettings generationSettings = GetGenerationSettings(size);
		string text = m_Text;
		m_Text = m_OutputText;
		base.cachedTextGenerator.Populate(m_Text, generationSettings);
		m_Text = text;
		Rect rect = base.rectTransform.rect;
		Vector2 textAnchorPivot = Text.GetTextAnchorPivot(base.alignment);
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Lerp(rect.xMin, rect.xMax, textAnchorPivot.x);
		zero.y = Mathf.Lerp(rect.yMin, rect.yMax, textAnchorPivot.y);
		Vector2 lhs = PixelAdjustPoint(zero) - zero;
		IList<UIVertex> verts = base.cachedTextGenerator.verts;
		float num = 1f / base.pixelsPerUnit;
		int num2 = verts.Count - 4;
		toFill.Clear();
		if (lhs != Vector2.zero)
		{
			for (int i = 0; i < num2; i++)
			{
				int num3 = i & 3;
				m_TempVerts[num3] = verts[i];
				m_TempVerts[num3].position *= num;
				m_TempVerts[num3].position.x += lhs.x;
				m_TempVerts[num3].position.y += lhs.y;
				if (num3 == 3)
				{
					toFill.AddUIVertexQuad(m_TempVerts);
				}
			}
		}
		else
		{
			float x = 0f;
			float num4 = 0f;
			float num5 = 0f;
			if (num2 > 0)
			{
				UIVertex uIVertex = verts[3];
				num5 = uIVertex.position.y;
			}
			for (int j = 0; j < num2; j++)
			{
				int num6 = j & 3;
				if (num6 == 0)
				{
					UIVertex uIVertex2 = verts[j];
					if (uIVertex2.position.y < num5)
					{
						UIVertex uIVertex3 = verts[j + 3];
						num5 = uIVertex3.position.y;
						x = num4;
						num4 = 0f;
					}
				}
				m_TempVerts[num6] = verts[j];
				m_TempVerts[num6].position -= new Vector3(x, 0f, 0f);
				m_TempVerts[num6].position *= num;
				if (num6 == 3)
				{
					toFill.AddUIVertexQuad(m_TempVerts);
				}
			}
		}
		m_DisableFontTextureRebuiltCallback = false;
		UIVertex vertex = default(UIVertex);
		foreach (HrefInfo hrefInfo in m_HrefInfos)
		{
			hrefInfo.boxes.Clear();
			if (hrefInfo.startIndex >= toFill.currentVertCount)
			{
				continue;
			}
			toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
			Vector3 position = vertex.position;
			Bounds bounds = new Bounds(position, Vector3.zero);
			int k = hrefInfo.startIndex;
			for (int endIndex = hrefInfo.endIndex; k < endIndex && k < toFill.currentVertCount; k++)
			{
				toFill.PopulateUIVertex(ref vertex, k);
				position = vertex.position;
				float x2 = position.x;
				Vector3 min = bounds.min;
				if (x2 < min.x)
				{
					hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
					bounds = new Bounds(position, Vector3.zero);
				}
				else
				{
					bounds.Encapsulate(position);
				}
			}
			hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
		}
	}

	protected virtual string GetOutputText(string outputText)
	{
		s_TextBuilder.Length = 0;
		m_HrefInfos.Clear();
		int num = 0;
		IEnumerator enumerator = s_HrefRegex.Matches(outputText).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Match match = (Match)enumerator.Current;
				s_TextBuilder.Append(outputText.Substring(num, match.Index - num));
				Group group = match.Groups[1];
				HrefInfo hrefInfo = new HrefInfo();
				hrefInfo.startIndex = s_TextBuilder.Length * 4;
				hrefInfo.endIndex = (s_TextBuilder.Length + match.Groups[2].Length - 1) * 4 + 3;
				hrefInfo.name = group.Value;
				HrefInfo item = hrefInfo;
				m_HrefInfos.Add(item);
				s_TextBuilder.Append(match.Groups[2].Value);
				num = match.Index + match.Length;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		s_TextBuilder.Append(outputText.Substring(num, outputText.Length - num));
		return s_TextBuilder.ToString();
	}

	private void HrefInfosIndexAdjust(int imgIndex)
	{
		foreach (HrefInfo hrefInfo in m_HrefInfos)
		{
			if (imgIndex < hrefInfo.startIndex)
			{
				hrefInfo.startIndex -= 8;
				hrefInfo.endIndex -= 8;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Vector2 localPoint;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
		foreach (HrefInfo hrefInfo in m_HrefInfos)
		{
			List<Rect> boxes = hrefInfo.boxes;
			for (int i = 0; i < boxes.Count; i++)
			{
				if (boxes[i].Contains(localPoint))
				{
					string text = hrefInfo.name;
					if (onHrefClick != null)
					{
						onHrefClick(hrefInfo.name);
					}
					if (text[0] == '"')
					{
						text = text.Remove(0, 1);
						text = text.Remove(text.Length - 1, 1);
					}
					Debug.Log("点击了:" + text);
					Application.OpenURL(text);
					return;
				}
			}
		}
	}
}
