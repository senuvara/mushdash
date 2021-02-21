using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlOffsetOption : MonoBehaviour
	{
		public Transform start;

		public Transform end;

		public RectTransform touchRect;

		public Animator animator;

		public GameObject nonius;

		public GameObject fadeObject;

		public AudioClip audioClip;

		public Button btnNext;

		public Button btnPre;

		public Text text;

		public Transform line;

		public GameObject tip;

		public Button btnOffset;

		public GameObject title;

		public GameObject titleTvMode;

		private Timer m_Timer;

		private int m_Offset;

		public bool isTvMode;

		private float m_Acceleration;

		private float m_AccelerationTimer;

		private float m_LineDragX;

		[HideInInspector]
		public bool isLineDraging;

		private int m_FrameCount;

		private Tween m_Tween;

		public void SetTvMode(bool enable)
		{
			isTvMode = enable;
		}

		private void Start()
		{
			m_Tween = fadeObject.GetComponent<DOTweenAnimation>().tween;
			LongPressButton orAddComponent = btnNext.GetOrAddComponent<LongPressButton>();
			orAddComponent.delta = 0.5f;
			orAddComponent.triggerDelta = 0.13f;
			LongPressButton longPressButton = orAddComponent;
			longPressButton.onPress = (Action<bool>)Delegate.Combine(longPressButton.onPress, (Action<bool>)delegate(bool b)
			{
				if (b)
				{
					OnOffsetChanged(true);
				}
			});
			LongPressButton longPressButton2 = orAddComponent;
			longPressButton2.onPressUp = (Action)Delegate.Combine(longPressButton2.onPressUp, (Action)delegate
			{
				m_Acceleration = 0f;
				m_AccelerationTimer = 0f;
			});
			orAddComponent = btnPre.GetOrAddComponent<LongPressButton>();
			orAddComponent.delta = 0.5f;
			orAddComponent.triggerDelta = 0.13f;
			LongPressButton longPressButton3 = orAddComponent;
			longPressButton3.onPress = (Action<bool>)Delegate.Combine(longPressButton3.onPress, (Action<bool>)delegate(bool b)
			{
				if (b)
				{
					OnOffsetChanged(false);
				}
			});
			LongPressButton longPressButton4 = orAddComponent;
			longPressButton4.onPressUp = (Action)Delegate.Combine(longPressButton4.onPressUp, (Action)delegate
			{
				m_Acceleration = 0f;
				m_AccelerationTimer = 0f;
			});
		}

		private void OnApplicationPause(bool isPause)
		{
			if (!isPause)
			{
				base.gameObject.SetActive(false);
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					base.gameObject.SetActive(true);
				}, 0.1f);
			}
		}

		private void Update()
		{
			if (GraphicSettings.isOverOneHundred && (Input.anyKeyDown || Singleton<InputManager>.instance.RewiredGetAnyButton()) && !Input.GetButtonDown("Cancel") && !Input.GetButtonDown("Horizontal") && !Input.GetKeyDown(KeyCode.Escape) && !Singleton<InputManager>.instance.RewiredGetButtonDown("Cancel") && !Singleton<InputManager>.instance.RewiredGetButtonDown("Horizontal"))
			{
				Tap();
			}
			if (Input.GetKey(KeyCode.Mouse0) && isLineDraging)
			{
				Camera main = Camera.main;
				Vector3 mousePosition = Input.mousePosition;
				float x = mousePosition.x;
				Vector3 localPosition = line.localPosition;
				float y = localPosition.y;
				Vector3 localPosition2 = line.localPosition;
				Vector3 vector = main.ScreenToWorldPoint(new Vector3(x, y, localPosition2.z));
				m_LineDragX = vector.x;
				line.DOMoveX(m_LineDragX, 0f);
				m_Offset = (int)(m_LineDragX * 100f);
				if (m_Offset > 500)
				{
					m_Offset = 500;
				}
				else if (m_Offset < -500)
				{
					m_Offset = -500;
				}
				text.text = ((float)m_Offset * 0.001f).ToString("f3");
			}
			else
			{
				Transform transform = line;
				float x2 = m_Offset;
				Vector3 localPosition3 = line.localPosition;
				float y2 = localPosition3.y;
				Vector3 localPosition4 = line.localPosition;
				transform.localPosition = new Vector3(x2, y2, localPosition4.z);
			}
		}

		private void OnOffsetChanged(bool forward)
		{
			m_AccelerationTimer += 1f;
			if (m_AccelerationTimer >= 2f)
			{
				m_Acceleration += ((!forward) ? (-10) : 10);
				m_AccelerationTimer = 0f;
			}
			if (forward && (float)m_Offset >= 500f)
			{
				m_Offset = 500;
				text.text = ((float)m_Offset * 0.001f).ToString("f3");
				return;
			}
			if (!forward && m_Offset <= -500)
			{
				m_Offset = -500;
				text.text = ((float)m_Offset * 0.001f).ToString("f3");
				return;
			}
			m_Offset += (forward ? 1 : (-1));
			m_Offset += (int)m_Acceleration;
			text.text = ((float)m_Offset * 0.001f).ToString("f3");
			if ((bool)tip)
			{
				tip.SetActive(m_Offset > 50 || m_Offset < -50);
			}
			Singleton<AudioManager>.instance.PlayOneShot("sfx_switch", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
		}

		private void OnEnable()
		{
			if (m_Timer != null)
			{
				m_Timer.Kill();
			}
			Singleton<AudioManager>.instance.bgm.volume = Singleton<DataManager>.instance["GameConfig"]["BGMVolume"].GetResult<float>();
			fadeObject.SetActive(true);
			animator.Rebind();
			Singleton<AudioManager>.instance.PlayBGM(audioClip);
			m_Timer = new Timer("OffsetFixed");
			nonius.transform.position = start.position;
			int totalTick = Mathf.CeilToInt(Singleton<AudioManager>.instance.bgm.clip.length / 0.01f);
			int offset = 0;
			m_Timer.AddTickEvent(delegate(int t)
			{
				int num = t + offset;
				float t2 = (float)((num + 180) % 240) / 240f;
				nonius.transform.position = Vector3.Lerp(start.position, end.position, t2);
				if (num >= totalTick || num == 0)
				{
					m_Timer.stopWatch.Reset();
					m_Timer.stopWatch.Start();
					offset = Mathf.RoundToInt(1f * (float)Singleton<AudioManager>.instance.bgm.timeSamples / (float)Singleton<AudioManager>.instance.bgm.clip.frequency / 0.01f);
				}
			});
			Singleton<AudioManager>.instance.bgm.timeSamples = 0;
			m_Timer.Play();
			if ((bool)btnOffset)
			{
				btnOffset.interactable = false;
			}
			if (Singleton<SceneManager>.instance.curScene.name == "Welcome")
			{
			}
			m_Offset = -Singleton<DataManager>.instance["GameConfig"][(!isTvMode) ? "Offset" : "Offset_TvMode"].GetResult<int>();
			title.SetActive(true);
			text.text = ((float)m_Offset * 0.001f).ToString("f3");
		}

		private void OnDisable()
		{
			Singleton<AudioManager>.instance.bgm.volume = Singleton<DataManager>.instance["GameConfig"]["BGMVolume"].GetResult<float>();
			m_Timer.Kill();
			Singleton<DataManager>.instance["GameConfig"][(!isTvMode) ? "Offset" : "Offset_TvMode"].SetResult(-m_Offset);
			Singleton<DataManager>.instance.Save();
			if ((bool)btnOffset)
			{
				btnOffset.interactable = true;
			}
			title.SetActive(false);
			fadeObject.transform.Find("ImgLineLight").GetComponent<Image>().DOFade(0f, 0f);
		}

		private void FixedUpdate()
		{
			bool flag = m_FrameCount != Time.frameCount;
			m_FrameCount = Time.frameCount;
			if (!GraphicSettings.isOverOneHundred && (Input.anyKeyDown || Singleton<InputManager>.instance.RewiredGetAnyButton()) && !Input.GetButtonDown("Cancel") && !Input.GetButtonDown("Horizontal") && !Input.GetKeyDown(KeyCode.Escape) && !Singleton<InputManager>.instance.RewiredGetButtonDown("Cancel") && !Singleton<InputManager>.instance.RewiredGetButtonDown("Horizontal"))
			{
				Tap();
			}
		}

		private void Tap()
		{
			fadeObject.transform.position = nonius.transform.position;
			m_Tween.Restart();
			animator.Play("OffsetLineLight", 0, 0f);
		}
	}
}
