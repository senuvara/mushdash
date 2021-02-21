using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetText : Action
	{
		[SerializeField]
		[Variable(new Type[]
		{
			typeof(UnityEngine.UI.Text),
			typeof(YlyRichText),
			typeof(InputField),
			typeof(TextMeshProUGUI),
			typeof(TextMeshPro)
		})]
		private IVariable m_Object;

		[SerializeField]
		[Variable(typeof(AnimationCurve), null, false)]
		[HideIf("duration", 0f, true)]
		private IVariable m_Curve;

		[SerializeField]
		[Variable(typeof(float), null, false)]
		private IVariable m_Duration;

		public string value = "%s";

		[SerializeField]
		[Variable(typeof(string), null, false)]
		private List<IVariable> m_Datas = new List<IVariable>
		{
			new Constance()
		};

		private const string tmpStr = "%s";

		private static readonly string[] tmpStrArray = new string[1]
		{
			"%s"
		};

		private string[] m_SplitStrs;

		private UnityEngine.UI.Text m_Text;

		private YlyRichText m_YlyRichText;

		private TextMeshProUGUI m_TextMeshProUGUI;

		private TextMeshPro m_TextMeshPro;

		private InputField m_InputField;

		private AnimationCurve m_AnimationCurve;

		private UnityGameManager.Looper m_Looper;

		private string m_OriginText;

		private static List<string> m_Strs = new List<string>();

		private static StringBuilder m_OutVal = new StringBuilder();

		public UnityEngine.UI.Text Object => m_Object.GetResult<UnityEngine.UI.Text>();

		public override float duration
		{
			get
			{
				if (m_Duration == null || m_Duration.result == null)
				{
					return 0f;
				}
				return m_Duration.GetResult<float>();
			}
		}

		public override void Enter()
		{
			string text = value.Replace("%s", "%%s");
			m_SplitStrs = text.Split(tmpStrArray, StringSplitOptions.None);
			Execute();
		}

		public override void Execute()
		{
			m_Datas.SelectNoAlloc(m_Strs, delegate(IVariable data)
			{
				object result = data.result;
				return (result == null) ? string.Empty : result.ToString();
			});
			m_OutVal.Length = 0;
			if (m_SplitStrs == null)
			{
				string text = value.Replace("%s", "%%s");
				m_SplitStrs = text.Split(tmpStrArray, StringSplitOptions.None);
			}
			for (int i = 0; i < m_SplitStrs.Length; i++)
			{
				string text2 = m_SplitStrs[i];
				string newValue = string.Empty;
				if (i < m_Strs.Count)
				{
					newValue = m_Strs[i];
					if (string.IsNullOrEmpty(newValue))
					{
						m_OutVal.Length = 0;
						break;
					}
				}
				m_OutVal.Append(text2.Replace("%", newValue));
			}
			m_Strs.Clear();
			DoText(m_OutVal.ToString());
			m_OutVal.Length = 0;
		}

		public override void Exit()
		{
			if (m_Looper != null)
			{
				SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop(m_Looper.uid);
			}
		}

		public override void Pause()
		{
			m_Looper.isPause = true;
		}

		public override void Resume()
		{
			m_Looper.isPause = false;
		}

		private void DoText(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				if (string.IsNullOrEmpty(m_OriginText))
				{
					return;
				}
				str = m_OriginText;
			}
			UnityEngine.Object result = m_Object.GetResult<UnityEngine.Object>();
			if (m_AnimationCurve == null && m_Curve != null && m_Curve.result != null)
			{
				m_AnimationCurve = m_Curve.GetResult<AnimationCurve>();
			}
			if (!result)
			{
				return;
			}
			if (!m_Text)
			{
				m_Text = result.GetObject<UnityEngine.UI.Text>();
				if ((bool)m_Text)
				{
					bool activeSelf = m_Text.gameObject.activeSelf;
					bool cull = m_Text.canvasRenderer.cull;
					if (!activeSelf)
					{
						m_Text.gameObject.SetActive(true);
					}
					m_Text.SetMaterialDirty();
					m_Text.canvasRenderer.cull = false;
					m_Text.Rebuild(CanvasUpdate.PreRender);
					m_Text.canvasRenderer.cull = cull;
					if (!activeSelf)
					{
						m_Text.gameObject.SetActive(false);
					}
				}
			}
			float numValue;
			if ((bool)m_Text)
			{
				if (string.IsNullOrEmpty(m_OriginText))
				{
					m_OriginText = m_Text.text;
				}
				if (float.TryParse(str, out numValue) && duration > 0f)
				{
					float curNumValue2;
					float.TryParse(m_Text.text, out curNumValue2);
					m_Looper = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("TextChange" + result.GetInstanceID(), delegate(float t)
					{
						float t6 = m_AnimationCurve.Evaluate(t / duration);
						m_Text.text = Mathf.RoundToInt(Mathf.Lerp(curNumValue2, numValue, t6)).ToString();
					}, UnityGameManager.LoopType.Update, duration);
				}
				else if (!string.IsNullOrEmpty(str))
				{
					m_Text.text = str;
				}
				return;
			}
			if (!m_TextMeshPro)
			{
				m_TextMeshPro = result.GetObject<TextMeshPro>();
				if ((bool)m_TextMeshPro)
				{
					bool activeSelf2 = m_TextMeshPro.gameObject.activeSelf;
					bool cull2 = m_TextMeshPro.canvasRenderer.cull;
					if (!activeSelf2)
					{
						m_TextMeshPro.gameObject.SetActive(true);
					}
					m_TextMeshPro.SetMaterialDirty();
					m_TextMeshPro.canvasRenderer.cull = false;
					m_TextMeshPro.Rebuild(CanvasUpdate.PreRender);
					m_TextMeshPro.canvasRenderer.cull = cull2;
					if (!activeSelf2)
					{
						m_TextMeshPro.gameObject.SetActive(false);
					}
				}
			}
			if ((bool)m_TextMeshPro)
			{
				if (string.IsNullOrEmpty(m_OriginText))
				{
					m_OriginText = m_TextMeshPro.text;
				}
				if (float.TryParse(str, out numValue) && duration > 0f)
				{
					float curNumValue3;
					float.TryParse(m_TextMeshPro.text, out curNumValue3);
					m_Looper = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("TextChange" + result.GetInstanceID(), delegate(float t)
					{
						float t5 = m_AnimationCurve.Evaluate(t / duration);
						m_TextMeshPro.text = Mathf.RoundToInt(Mathf.Lerp(curNumValue3, numValue, t5)).ToString();
					}, UnityGameManager.LoopType.Update, duration);
				}
				else if (!string.IsNullOrEmpty(str))
				{
					m_TextMeshPro.text = str;
				}
				return;
			}
			if (!m_TextMeshProUGUI)
			{
				m_TextMeshProUGUI = result.GetObject<TextMeshProUGUI>();
				if ((bool)m_TextMeshProUGUI)
				{
					bool activeSelf3 = m_TextMeshProUGUI.gameObject.activeSelf;
					bool cull3 = m_TextMeshProUGUI.canvasRenderer.cull;
					if (!activeSelf3)
					{
						m_TextMeshProUGUI.gameObject.SetActive(true);
					}
					m_TextMeshProUGUI.SetMaterialDirty();
					m_TextMeshProUGUI.canvasRenderer.cull = false;
					m_TextMeshProUGUI.Rebuild(CanvasUpdate.PreRender);
					m_TextMeshProUGUI.canvasRenderer.cull = cull3;
					if (!activeSelf3)
					{
						m_TextMeshProUGUI.gameObject.SetActive(false);
					}
				}
			}
			if ((bool)m_TextMeshProUGUI)
			{
				if (string.IsNullOrEmpty(m_OriginText))
				{
					m_OriginText = m_TextMeshProUGUI.text;
				}
				if (float.TryParse(str, out numValue) && duration > 0f)
				{
					float curNumValue4 = 0f;
					float.TryParse(m_TextMeshProUGUI.text, out curNumValue4);
					m_Looper = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("TextChange" + result.GetInstanceID(), delegate(float t)
					{
						float t4 = m_AnimationCurve.Evaluate(t / duration);
						m_TextMeshProUGUI.text = Mathf.RoundToInt(Mathf.Lerp(curNumValue4, numValue, t4)).ToString();
					}, UnityGameManager.LoopType.Update, duration);
				}
				else if (!string.IsNullOrEmpty(str))
				{
					m_TextMeshProUGUI.text = str;
				}
				return;
			}
			if (!m_InputField)
			{
				m_InputField = result.GetObject<InputField>();
				if ((bool)m_InputField)
				{
					bool activeSelf4 = m_InputField.gameObject.activeSelf;
					if (!activeSelf4)
					{
						m_InputField.gameObject.SetActive(true);
					}
					m_InputField.Rebuild(CanvasUpdate.LatePreRender);
					if (!activeSelf4)
					{
						m_InputField.gameObject.SetActive(false);
					}
				}
			}
			if ((bool)m_InputField)
			{
				if (string.IsNullOrEmpty(m_OriginText))
				{
					m_OriginText = m_InputField.text;
				}
				if (float.TryParse(str, out numValue) && duration > 0f)
				{
					float curNumValue5 = 0f;
					float.TryParse(m_InputField.text, out curNumValue5);
					m_Looper = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("TextChange" + result.GetInstanceID(), delegate(float t)
					{
						float t3 = m_AnimationCurve.Evaluate(t / duration);
						m_InputField.text = Mathf.RoundToInt(Mathf.Lerp(curNumValue5, numValue, t3)).ToString();
					}, UnityGameManager.LoopType.Update, duration);
				}
				else if (!string.IsNullOrEmpty(str))
				{
					m_InputField.text = str;
				}
				return;
			}
			if (!m_YlyRichText)
			{
				m_YlyRichText = result.GetObject<YlyRichText>();
				if ((bool)m_YlyRichText)
				{
					bool activeSelf5 = m_YlyRichText.gameObject.activeSelf;
					bool cull4 = m_YlyRichText.canvasRenderer.cull;
					if (!activeSelf5)
					{
						m_YlyRichText.gameObject.SetActive(true);
					}
					m_YlyRichText.SetMaterialDirty();
					m_YlyRichText.canvasRenderer.cull = false;
					m_YlyRichText.Rebuild(CanvasUpdate.PreRender);
					m_YlyRichText.canvasRenderer.cull = cull4;
					if (!activeSelf5)
					{
						m_YlyRichText.gameObject.SetActive(false);
					}
				}
			}
			if (!m_YlyRichText)
			{
				return;
			}
			if (string.IsNullOrEmpty(m_OriginText))
			{
				m_OriginText = m_YlyRichText.text;
			}
			if (float.TryParse(str, out numValue) && duration > 0f)
			{
				float curNumValue = 0f;
				float.TryParse(m_YlyRichText.text, out curNumValue);
				m_Looper = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("TextChange" + result.GetInstanceID(), delegate(float t)
				{
					float t2 = m_AnimationCurve.Evaluate(t / duration);
					m_YlyRichText.text = Mathf.RoundToInt(Mathf.Lerp(curNumValue, numValue, t2)).ToString();
				}, UnityGameManager.LoopType.Update, duration);
			}
			else if (!string.IsNullOrEmpty(str))
			{
				m_YlyRichText.text = str;
			}
		}
	}
}
