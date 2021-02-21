using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using UnityEngine;

public class GCClip : MonoBehaviour
{
	public int idx;

	private float m_ClipValue;

	private const float interval = 6.7f;

	private Material m_Material;

	private float m_SizePower = 20f;

	private float m_MinSize = 0.268f;

	public float updateInterval = 0.016f;

	private int m_PassFrame;

	private float m_PassTime;

	private void Reset()
	{
		idx = base.gameObject.transform.GetSiblingIndex();
	}

	private void Start()
	{
		m_Material = GetComponent<SpriteRenderer>().material;
	}

	private void FixedUpdate()
	{
		if (!Singleton<StageBattleComponent>.instance.isPause)
		{
			if (!GameAudioVisualization.isPlay)
			{
				m_ClipValue = m_MinSize;
			}
			else
			{
				UpAudioVisualization();
				m_ClipValue = ((!(m_ClipValue < m_MinSize)) ? m_ClipValue : m_MinSize);
			}
			m_Material.SetFloat("_ClipValue", m_ClipValue);
		}
	}

	private void UpAudioVisualization()
	{
		if (GraphicSettings.isOverOneHundred && GraphicSettings.isFrameOverOneHundred)
		{
			m_PassFrame++;
			m_PassTime += Time.fixedDeltaTime;
			if (m_PassTime >= updateInterval)
			{
				OnAudioVisualization();
				m_PassFrame = 0;
				m_PassTime = 0f;
			}
		}
		else
		{
			OnAudioVisualization();
		}
	}

	private void OnAudioVisualization()
	{
		float num = GameAudioVisualization._bandBuffer[idx] * m_SizePower;
		for (int i = 0; i < GameAudioVisualization._bandBuffer.Length; i++)
		{
			float num2 = GameAudioVisualization._bandBuffer[i] * m_SizePower;
			if (num > num2)
			{
				m_ClipValue = num2 / num;
			}
			else if (num < num2)
			{
				m_ClipValue = num / num2;
			}
			else
			{
				m_ClipValue = num;
			}
		}
		m_ClipValue = Mathf.Floor(m_ClipValue * 100f / 6.7f) * 6.7f / 100f;
		m_ClipValue = Mathf.Lerp(m_Material.GetFloat("_ClipValue"), m_ClipValue, 0.0167f * m_SizePower);
		m_ClipValue = Mathf.Floor(m_ClipValue * 100f / 6.7f) * 6.7f / 100f;
	}
}
