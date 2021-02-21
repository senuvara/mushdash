using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Panels;
using DG.Tweening;
using FormulaBase;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common
{
	public class UITest : MonoBehaviour
	{
		public class BmgGroup
		{
			public iBMSCManager.BMS easyBms;

			public iBMSCManager.BMS normalBms;

			public iBMSCManager.BMS hardBms;
		}

		public GameObject scrollSpace;

		public Dropdown dropdownCharacter;

		public Dropdown dropdownElfin;

		private ScrollRect m_ScrollRect;

		private List<BmgGroup> m_BmsGroups;

		private List<iBMSCManager.BMS> m_Bmss;

		private bool m_IsStart;

		private List<Toggle> m_Tgls;

		public int index;

		public int difficulty;

		private float m_KeyCoolTime = 0.15f;

		private float m_CoolTime;

		public List<Toggle> tgls
		{
			get
			{
				if (m_Tgls == null)
				{
					m_Tgls = new List<Toggle>();
					if (scrollSpace.transform.childCount > 0)
					{
						Transform child = scrollSpace.transform.GetChild(0);
						for (int i = 0; i < child.childCount; i++)
						{
							Toggle component = child.GetChild(i).GetComponent<Toggle>();
							m_Tgls.Add(component);
						}
					}
				}
				return m_Tgls;
			}
		}

		public static UITest instance
		{
			get;
			private set;
		}

		public iBMSCManager.BMS bms
		{
			get
			{
				BmgGroup bmgGroup = bgs[index];
				if (difficulty == 1)
				{
					return bmgGroup.easyBms;
				}
				if (difficulty == 2)
				{
					return bmgGroup.normalBms;
				}
				if (difficulty == 3)
				{
					return bmgGroup.hardBms;
				}
				return null;
			}
		}

		public List<BmgGroup> bgs
		{
			get
			{
				if (m_BmsGroups == null)
				{
					LoadBms();
				}
				return m_BmsGroups;
			}
		}

		public int count
		{
			get
			{
				if (bgs == null)
				{
					return 0;
				}
				return bgs.Count;
			}
		}

		public void GameStart()
		{
			if (!m_IsStart)
			{
				Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(Singleton<ConfigManager>.instance.GetConfigIndex("character", "order", dropdownCharacter.value + 1));
				Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].SetResult(Singleton<ConfigManager>.instance.GetConfigIndex("elfin", "order", dropdownElfin.value + 1));
				Singleton<DataManager>.instance.Save();
				m_IsStart = true;
				string name = (string)bms.info["TITLE"];
				Singleton<StageBattleComponent>.instance.InitByName(name);
				Singleton<AudioManager>.instance.PlayOneShot("sfx_common_button", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
				Singleton<ConfigManager>.instance.SaveObject("EditorModeIndex", index);
				Singleton<ConfigManager>.instance.SaveObject("EditorModeDifficulty", difficulty);
				SingletonMonoBehaviour<GameSceneMainController>.instance.GameInit();
				PnlBattle.instance.GameStart();
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					base.gameObject.SetActive(false);
				}, 0.1f);
			}
		}

		public bool ExistDifficulty(int idx, int diff)
		{
			BmgGroup bmgGroup = bgs[idx];
			if (diff == 1 && bmgGroup.easyBms != null)
			{
				return true;
			}
			if (diff == 2 && bmgGroup.normalBms != null)
			{
				return true;
			}
			if (diff == 3 && bmgGroup.hardBms != null)
			{
				return true;
			}
			return false;
		}

		public string GetValue(int idx, string key)
		{
			BmgGroup bmgGroup = bgs[idx];
			if (bmgGroup.easyBms != null)
			{
				return (string)bmgGroup.easyBms.info[key];
			}
			if (bmgGroup.normalBms != null)
			{
				return (string)bmgGroup.normalBms.info[key];
			}
			if (bmgGroup.hardBms != null)
			{
				return (string)bmgGroup.hardBms.info[key];
			}
			return string.Empty;
		}

		public string GetValue(int idx, int diff, string key)
		{
			BmgGroup bmgGroup = bgs[idx];
			switch (diff)
			{
			case 1:
				if (bmgGroup.easyBms != null)
				{
					return (string)bmgGroup.easyBms.info[key];
				}
				return string.Empty;
			case 2:
				if (bmgGroup.normalBms != null)
				{
					return (string)bmgGroup.normalBms.info[key];
				}
				return string.Empty;
			case 3:
				if (bmgGroup.hardBms != null)
				{
					return (string)bmgGroup.hardBms.info[key];
				}
				return string.Empty;
			default:
				return string.Empty;
			}
		}

		public void LoadAll()
		{
			LoadBms();
			Transform transform = base.transform.Find("PnlStage");
			transform.gameObject.SetActive(false);
			transform.gameObject.SetActive(true);
			m_Tgls = null;
		}

		private void LoadBms()
		{
			if (!GameSceneMainController.isEditorMode)
			{
				return;
			}
			m_BmsGroups = new List<BmgGroup>();
			m_Bmss = Singleton<iBMSCManager>.instance.LoadAll();
			foreach (iBMSCManager.BMS item in m_Bmss)
			{
				string musicName = (string)item.info["TITLE"];
				BmgGroup bmgGroup = m_BmsGroups.Find(delegate(BmgGroup b)
				{
					if (b.easyBms != null)
					{
						return (string)b.easyBms.info["TITLE"] == musicName;
					}
					if (b.normalBms != null)
					{
						return (string)b.normalBms.info["TITLE"] == musicName;
					}
					return b.hardBms != null && (string)b.hardBms.info["TITLE"] == musicName;
				});
				if (bmgGroup == null)
				{
					bmgGroup = new BmgGroup();
					m_BmsGroups.Add(bmgGroup);
				}
				int num = int.Parse((string)item.info["RANK"]);
				if (num == 1)
				{
					bmgGroup.easyBms = item;
				}
				switch (num)
				{
				case 2:
					bmgGroup.normalBms = item;
					break;
				case 3:
					bmgGroup.hardBms = item;
					break;
				}
			}
		}

		private void Awake()
		{
			if (GameSceneMainController.isEditorMode)
			{
				instance = this;
				LoadAll();
				InitDropdown();
				Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
			}
		}

		private void OnDestroy()
		{
			instance = null;
		}

		private void Start()
		{
			if (!GameSceneMainController.isEditorMode)
			{
				return;
			}
			index = Singleton<ConfigManager>.instance.GetObject<int>("EditorModeIndex");
			difficulty = Singleton<ConfigManager>.instance.GetObject<int>("EditorModeDifficulty");
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if (base.gameObject.activeSelf && m_Bmss != null)
				{
					iBMSCManager.BMS bMS = m_Bmss.Find((iBMSCManager.BMS b) => bool.Parse((string)b.info["NEW"]));
					if (bMS != null)
					{
						string a = (string)bMS.info["NAME"];
						for (int i = 0; i < bgs.Count; i++)
						{
							BmgGroup bmgGroup = bgs[i];
							if (bmgGroup.easyBms != null && a == (string)bmgGroup.easyBms.info["NAME"])
							{
								index = i;
								difficulty = 1;
								break;
							}
							if (bmgGroup.normalBms != null && a == (string)bmgGroup.normalBms.info["NAME"])
							{
								index = i;
								difficulty = 2;
								break;
							}
							if (bmgGroup.hardBms != null && a == (string)bmgGroup.hardBms.info["NAME"])
							{
								index = i;
								difficulty = 3;
								break;
							}
						}
						if (index < tgls.Count)
						{
							tgls[index].isOn = true;
							List<Toggle> list = from t in tgls[index].transform.GetChild(0).GetChild(0).GetComponentsInChildren<Toggle>()
								where t.IsInteractable()
								select t;
							if (difficulty >= 1 && difficulty < list.Count)
							{
								list[difficulty - 1].isOn = true;
							}
						}
					}
				}
			}, 1);
		}

		private void Update()
		{
			if (!m_ScrollRect)
			{
				m_ScrollRect = scrollSpace.GetComponent<ScrollRect>();
				if ((bool)m_ScrollRect)
				{
					m_ScrollRect.verticalNormalizedPosition = 1f - 1f * (float)index / (float)(count - 1);
				}
			}
			m_CoolTime -= Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.F5))
			{
				LoadAll();
			}
			if (Input.GetKeyDown(KeyCode.F1))
			{
				if (Singleton<StageBattleComponent>.instance.IsAutoPlay())
				{
					Singleton<StageBattleComponent>.instance.SetAutoPlay(false);
				}
				else
				{
					Singleton<StageBattleComponent>.instance.SetAutoPlay(true);
				}
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				if (Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>())
				{
					Singleton<DataManager>.instance["Account"]["IsAutoFever"].SetResult(false);
				}
				else
				{
					Singleton<DataManager>.instance["Account"]["IsAutoFever"].SetResult(true);
				}
				Singleton<DataManager>.instance.Save();
				Singleton<EventManager>.instance.Invoke("Battle/OnSetAutoFever");
			}
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && m_CoolTime <= 0f)
			{
				List<Toggle> tgls = this.tgls;
				if (tgls != null)
				{
					m_CoolTime = m_KeyCoolTime;
					if (index < tgls.Count - 1)
					{
						index++;
					}
					else
					{
						index = 0;
					}
					tgls[index].isOn = true;
					if (index == 0)
					{
						m_ScrollRect.verticalNormalizedPosition = 1f;
					}
					else if (index == tgls.Count - 1)
					{
						m_ScrollRect.verticalNormalizedPosition = 0f;
					}
					else if (index > 7 && m_ScrollRect.verticalNormalizedPosition > 0.05f)
					{
						Transform transform = m_ScrollRect.content.transform;
						Vector3 localPosition = m_ScrollRect.content.transform.localPosition;
						transform.DOLocalMoveY(localPosition.y + 100f, 0.1f);
					}
				}
			}
			if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && m_CoolTime <= 0f)
			{
				List<Toggle> tgls2 = this.tgls;
				if (tgls2 != null)
				{
					m_CoolTime = m_KeyCoolTime;
					if (index > 0)
					{
						index--;
					}
					else
					{
						index = tgls2.Count - 1;
					}
					tgls2[index].isOn = true;
					if (index == 0)
					{
						m_ScrollRect.verticalNormalizedPosition = 1f;
					}
					else if (index == tgls2.Count - 1)
					{
						m_ScrollRect.verticalNormalizedPosition = 0f;
					}
					else if (index < tgls2.Count - 8 && m_ScrollRect.verticalNormalizedPosition < 0.95f)
					{
						Transform transform2 = m_ScrollRect.content.transform;
						Vector3 localPosition2 = m_ScrollRect.content.transform.localPosition;
						transform2.DOLocalMoveY(localPosition2.y - 100f, 0.1f);
					}
				}
			}
			if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && m_CoolTime <= 0f)
			{
				List<Toggle> tgls3 = this.tgls;
				if (tgls3 != null)
				{
					m_CoolTime = m_KeyCoolTime;
					List<Toggle> list = (from t in tgls3[index].transform.GetChild(0).GetChild(0).GetComponentsInChildren<Toggle>()
						where t.IsInteractable()
						select t).ToList();
					if (difficulty > 1)
					{
						difficulty--;
					}
					else
					{
						difficulty = list.Count;
					}
					if (difficulty - 1 < list.Count)
					{
						Toggle toggle = list[difficulty - 1];
						if ((bool)toggle)
						{
							toggle.isOn = true;
						}
					}
				}
			}
			if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && m_CoolTime <= 0f)
			{
				List<Toggle> tgls4 = this.tgls;
				if (tgls4 != null)
				{
					m_CoolTime = m_KeyCoolTime;
					List<Toggle> list2 = (from t in tgls4[index].transform.GetChild(0).GetChild(0).GetComponentsInChildren<Toggle>()
						where t.IsInteractable()
						select t).ToList();
					if (difficulty < list2.Count)
					{
						difficulty++;
					}
					else
					{
						difficulty = 1;
					}
					if (difficulty - 1 < list2.Count)
					{
						Toggle toggle2 = list2[difficulty - 1];
						if ((bool)toggle2)
						{
							toggle2.isOn = true;
						}
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				GameStart();
			}
		}

		private void InitDropdown()
		{
			JArray jArray = Singleton<ConfigManager>.instance["character"];
			int count = jArray.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("character", i, "hide"))
				{
					string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("character", i, "cosName");
					int num = Singleton<ConfigManager>.instance.GetConfigIntValue("character", i, "order") - 1;
					array[num] = configStringValue;
				}
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					dropdownCharacter.options.Add(new Dropdown.OptionData(text));
				}
			}
			dropdownCharacter.value = Singleton<ConfigManager>.instance.GetConfigIntValue("character", Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>(), "order") - 1;
			dropdownCharacter.RefreshShownValue();
			JArray jArray2 = Singleton<ConfigManager>.instance["elfin"];
			int count2 = jArray2.Count;
			string[] array2 = new string[count2];
			for (int k = 0; k < count2; k++)
			{
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("elfin", k, "hide"))
				{
					string configStringValue2 = Singleton<ConfigManager>.instance.GetConfigStringValue("elfin", k, "name");
					int num2 = Singleton<ConfigManager>.instance.GetConfigIntValue("elfin", k, "order") - 1;
					array2[num2] = configStringValue2;
				}
			}
			foreach (string text2 in array2)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					dropdownElfin.options.Add(new Dropdown.OptionData(text2));
				}
			}
			int result = Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].GetResult<int>();
			dropdownElfin.value = Singleton<ConfigManager>.instance.GetConfigIntValue("elfin", (result >= 0) ? result : 0, "order") - 1;
			dropdownElfin.RefreshShownValue();
		}
	}
}
