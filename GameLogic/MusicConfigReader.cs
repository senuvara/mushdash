using Assets.Scripts.Common;
using Assets.Scripts.GameCore;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Newtonsoft.Json.Linq;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameLogic
{
	public class MusicConfigReader : BaseConfigReader
	{
		public static MusicConfigReader Instance = new MusicConfigReader();

		public List<SceneEvent> sceneEvents;

		public iBMSCManager.BMS bms;

		public StageInfo stageInfo;

		public float bpm;

		public decimal delay;

		public float floatDelay;

		private const int tickLimit = 0;

		private MusicConfigReader()
		{
		}

		public static void ReleaseReferences()
		{
			Instance.sceneEvents = null;
			Instance.bms = null;
			Instance.stageInfo = null;
		}

		public override void Init(string filename)
		{
			if (GetDataCount() > 0)
			{
				return;
			}
			List<NoteConfigData> data = NodeConfigReader.Instance.GetData();
			JArray jArray;
			if (bms != null)
			{
				jArray = bms.GetNoteDatas();
				sceneEvents = bms.GetSceneEvents().ToObject<List<SceneEvent>>();
				bpm = bms.GetBpm();
			}
			else
			{
				jArray = Singleton<ConfigManager>.instance[filename];
			}
			Add(default(MusicData));
			short num = 1;
			string text = (string)bms.info["GENRE"];
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("boss", "scene_name", "boss_name", text);
			GameObject gameObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(configStringValue);
			SpineActionController component = gameObject.GetComponent<SpineActionController>();
			SkeletonAnimation component2 = component.gameObject.GetComponent<SkeletonAnimation>();
			ExposedList<Spine.Animation> animations = component2.skeletonDataAsset.GetSkeletonData(true).Animations;
			MusicData musicData;
			for (int i = 0; i < jArray.Count; i++)
			{
				MusicConfigData configData = jArray[i].ToObject<MusicConfigData>();
				if (configData.time < 0m)
				{
					continue;
				}
				musicData = default(MusicData);
				musicData.objId = num++;
				musicData.tick = decimal.Round(configData.time, 3);
				musicData.configData = configData;
				musicData.isLongPressEnd = false;
				musicData.isLongPressing = false;
				MusicData musicData2 = musicData;
				string note_uid = musicData2.configData.note_uid;
				int pathway = musicData2.configData.pathway;
				foreach (NoteConfigData item in data)
				{
					if (item.uid == note_uid && item.pathway == pathway)
					{
						musicData2.noteData = item;
						break;
					}
				}
				Add(musicData2);
				if (!musicData2.isLongPressStart)
				{
					continue;
				}
				int longPressCount = musicData2.longPressCount;
				int endIndex = (int)(decimal.Round(musicData2.tick + musicData2.configData.length - musicData2.noteData.left_great_range - musicData2.noteData.left_perfect_range, 3) / 0.001m);
				for (int j = 1; j <= longPressCount; j++)
				{
					musicData = default(MusicData);
					musicData.objId = num++;
					musicData.tick = musicData2.tick + 0.1m * (decimal)j;
					musicData.configData = musicData2.configData;
					MusicData musicData3 = musicData;
					musicData3.configData.length = 0m;
					musicData3.isLongPressing = true;
					musicData3.isLongPressEnd = false;
					musicData3.noteData = musicData2.noteData;
					musicData3.longPressPTick = musicData2.configData.time;
					musicData3.endIndex = endIndex;
					if (j == longPressCount)
					{
						musicData3.isLongPressing = false;
						musicData3.isLongPressEnd = true;
						musicData3.tick = musicData2.tick + musicData2.configData.length;
					}
					Add(musicData3);
				}
			}
			Sort();
			string text2 = "0";
			ArrayList arrayList = new ArrayList(base.data);
			int num2 = -1;
			MusicConfigData musicConfigData;
			for (int num3 = arrayList.Count - 1; num3 >= 0; num3--)
			{
				MusicData musicData4 = (MusicData)arrayList[num3];
				string a2 = string.Empty;
				if (num3 + 1 < arrayList.Count)
				{
					MusicData musicData5 = (MusicData)arrayList[num3 + 1];
					a2 = musicData5.noteData.boss_action;
				}
				string bossAction = musicData4.noteData.boss_action;
				if (bossAction != "0" && !string.IsNullOrEmpty(bossAction))
				{
					string text3 = string.Empty;
					if ((bossAction == "boss_far_atk_1_L" || bossAction == "boss_far_atk_1_R") && a2 != "boss_far_atk_2")
					{
						text3 = "boss_far_atk_1_end";
					}
					if (bossAction == "boss_far_atk_2" && a2 != "boss_far_atk_1_L" && a2 != "boss_far_atk_1_R")
					{
						text3 = "boss_far_atk_2_end";
					}
					if (string.IsNullOrEmpty(text3))
					{
						break;
					}
					NoteConfigData noteData = default(NoteConfigData);
					foreach (NoteConfigData item2 in data)
					{
						if (item2.scene == text && item2.boss_action == text3)
						{
							noteData = item2;
							break;
						}
					}
					decimal d3 = (decimal)Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicData4.noteData.prefab_name).GetComponent<SpineActionController>().startDelay;
					d3 = decimal.Round(d3, 3);
					SkeletActionData skeletActionData = component.actionData.ToList().Find((SkeletActionData dd) => dd.name == bossAction);
					string animName4 = skeletActionData.actionIdx[0];
					decimal d4 = (decimal)animations.Find((Spine.Animation a) => a.Name == animName4).Duration;
					musicConfigData = default(MusicConfigData);
					musicConfigData.id = 0;
					musicConfigData.length = 0m;
					musicConfigData.note_uid = noteData.uid;
					musicConfigData.time = decimal.Round(musicData4.tick - d3 + d4, 3);
					MusicConfigData configData2 = musicConfigData;
					musicData = default(MusicData);
					musicData.objId = 0;
					musicData.tick = configData2.time;
					musicData.configData = configData2;
					musicData.noteData = noteData;
					MusicData musicData6 = musicData;
					Add(musicData6);
					break;
				}
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				MusicData musicData7 = (MusicData)arrayList[k];
				string boss_action = musicData7.noteData.boss_action;
				if (!(boss_action != "0") || string.IsNullOrEmpty(boss_action))
				{
					continue;
				}
				num2 = ((!(boss_action == "out") && !(musicData7.noteData.ibms_id == "17")) ? (-1) : musicData7.objId);
				MusicData musicData8 = default(MusicData);
				for (int num4 = k - 1; num4 >= 0; num4--)
				{
					MusicData musicData9 = (MusicData)arrayList[num4];
					if (musicData9.noteData.boss_action != "0")
					{
						musicData8 = musicData9;
						break;
					}
				}
				string preBossAction = text2;
				text2 = boss_action;
				NoteConfigData noteData2 = musicData7.noteData;
				bool flag = false;
				decimal d5 = (decimal)Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicData7.noteData.prefab_name).GetComponent<SpineActionController>().startDelay;
				d5 = decimal.Round(d5, 3);
				string action = string.Empty;
				if ((preBossAction == "boss_far_atk_1_L" || preBossAction == "boss_far_atk_1_R") && text2 != "boss_far_atk_1_end" && text2 != "boss_far_atk_1_L" && text2 != "boss_far_atk_1_R" && text2 != "boss_far_atk_2")
				{
					action = "boss_far_atk_1_end";
				}
				if (preBossAction == "boss_far_atk_2" && text2 != "boss_far_atk_2_end" && text2 != "boss_far_atk_2" && text2 != "boss_far_atk_1_L" && text2 != "boss_far_atk_1_R")
				{
					action = "boss_far_atk_2_end";
				}
				foreach (NoteConfigData item3 in data)
				{
					if (item3.scene == text && item3.boss_action == action)
					{
						noteData2 = item3;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					d5 = (decimal)Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicData8.noteData.prefab_name).GetComponent<SpineActionController>().startDelay;
					d5 = decimal.Round(d5, 3);
					SkeletActionData skeletActionData2 = component.actionData.ToList().Find((SkeletActionData dd) => dd.name == preBossAction);
					string[] findRes = skeletActionData2.actionIdx;
					if (findRes == null || findRes.Length <= 0)
					{
						Debug.LogErrorFormat("Can not find 'SkeletonActionData' of boss [{0}]", preBossAction);
					}
					decimal d6 = (decimal)animations.Find((Spine.Animation a) => a.Name == findRes.First()).Duration;
					musicConfigData = default(MusicConfigData);
					musicConfigData.id = 0;
					musicConfigData.length = 0m;
					musicConfigData.note_uid = noteData2.uid;
					musicConfigData.time = decimal.Round(musicData8.tick - d5 + d6, 3);
					MusicConfigData configData3 = musicConfigData;
					musicData = default(MusicData);
					musicData.objId = 0;
					musicData.tick = configData3.time;
					musicData.configData = configData3;
					musicData.noteData = noteData2;
					MusicData musicData10 = musicData;
					Add(musicData10);
				}
				action = string.Empty;
				flag = false;
				if ((preBossAction == "0" || preBossAction == "out" || musicData8.noteData.ibms_id == "17") && text2 != "in")
				{
					action = "in";
					foreach (NoteConfigData item4 in data)
					{
						if (item4.scene == text && item4.boss_action == action)
						{
							noteData2 = item4;
							break;
						}
					}
					SkeletActionData skeletActionData3 = component.actionData.ToList().Find((SkeletActionData dd) => dd.name == action);
					string animName3 = skeletActionData3.actionIdx[0];
					decimal d7 = (decimal)animations.Find((Spine.Animation a) => a.Name == animName3).Duration;
					musicConfigData = default(MusicConfigData);
					musicConfigData.id = 0;
					musicConfigData.length = 0m;
					musicConfigData.note_uid = noteData2.uid;
					musicConfigData.time = decimal.Round(musicData7.tick - d5 - d7, 3);
					MusicConfigData configData4 = musicConfigData;
					musicData = default(MusicData);
					musicData.objId = 0;
					musicData.tick = configData4.time;
					musicData.configData = configData4;
					musicData.noteData = noteData2;
					MusicData musicData11 = musicData;
					Add(musicData11);
				}
				if ((text2 == "boss_far_atk_1_L" || text2 == "boss_far_atk_1_R") && preBossAction != "boss_far_atk_1_start" && preBossAction != "boss_far_atk_1_L" && preBossAction != "boss_far_atk_1_R")
				{
					action = ((!(preBossAction == "boss_far_atk_2")) ? "boss_far_atk_1_start" : "atk_2_to_1");
				}
				else if (text2 == "boss_far_atk_2" && preBossAction != "boss_far_atk_2_start" && preBossAction != "boss_far_atk_2")
				{
					action = ((!(preBossAction == "boss_far_atk_1_L") && !(preBossAction == "boss_far_atk_1_R")) ? "boss_far_atk_2_start" : "atk_1_to_2");
				}
				foreach (NoteConfigData item5 in data)
				{
					if (item5.scene == text && item5.boss_action == action)
					{
						flag = true;
						noteData2 = item5;
						break;
					}
				}
				if (flag)
				{
					SkeletActionData skeletActionData4 = component.actionData.ToList().Find((SkeletActionData dd) => dd.name == action);
					string[] actionIdx = skeletActionData4.actionIdx;
					if (actionIdx == null || actionIdx.Length <= 0)
					{
						Debug.LogErrorFormat("Can not find 'SkeletonActionData' of boss [{0}]", action);
					}
					string animName2 = actionIdx.First();
					decimal d8 = (decimal)animations.Find((Spine.Animation a) => a.Name == animName2).Duration;
					musicConfigData = default(MusicConfigData);
					musicConfigData.id = 0;
					musicConfigData.length = 0m;
					musicConfigData.note_uid = noteData2.uid;
					musicConfigData.time = decimal.Round(musicData7.tick - d5 - d8, 3);
					MusicConfigData configData5 = musicConfigData;
					musicData = default(MusicData);
					musicData.objId = 0;
					musicData.tick = configData5.time;
					musicData.configData = configData5;
					musicData.noteData = noteData2;
					MusicData musicData12 = musicData;
					Add(musicData12);
				}
			}
			if (num2 == -1 && base.data.Cast<MusicData>().Exists((MusicData d) => d.noteData.boss_action != "0" && !string.IsNullOrEmpty(d.noteData.boss_action)))
			{
				List<MusicData> list = base.data.Cast<MusicData>().ToList();
				MusicData myMd;
				musicData = (myMd = default(MusicData));
				decimal num5 = list.Max((MusicData d) => d.tick);
				foreach (MusicData item6 in list)
				{
					if (item6.tick != num5)
					{
						continue;
					}
					myMd = item6;
					break;
				}
				if (myMd.noteData.boss_action != "0")
				{
					decimal d9 = (decimal)Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(myMd.noteData.prefab_name).GetComponent<SpineActionController>().startDelay;
					d9 = decimal.Round(d9, 3);
					string animName = myMd.noteData.boss_action;
					SkeletActionData skeletActionData5 = component.actionData.ToList().Find((SkeletActionData dd) => dd.name == myMd.noteData.boss_action);
					if (skeletActionData5.actionIdx != null && skeletActionData5.actionIdx.Length > 0)
					{
						animName = skeletActionData5.actionIdx[0];
					}
					decimal d10 = (decimal)animations.Find((Spine.Animation a) => a.Name == animName).Duration;
					num5 += decimal.Round(-d9 + d10, 3);
				}
				num5 = decimal.Round(num5 + 0.1m, 3);
				NoteConfigData noteData3 = default(NoteConfigData);
				foreach (NoteConfigData item7 in data)
				{
					if (item7.scene == text && item7.boss_action == "out")
					{
						noteData3 = item7;
						break;
					}
				}
				musicConfigData = default(MusicConfigData);
				musicConfigData.id = 0;
				musicConfigData.length = 0m;
				musicConfigData.note_uid = noteData3.uid;
				musicConfigData.time = num5;
				MusicConfigData configData6 = musicConfigData;
				MusicData musicData13 = default(MusicData);
				musicData13.objId = 0;
				musicData13.tick = configData6.time;
				musicData13.configData = configData6;
				musicData13.noteData = noteData3;
				MusicData musicData14 = musicData13;
				Add(musicData14);
			}
			delay = 0m;
			for (int l = 0; l < base.data.Count; l++)
			{
				MusicData musicData15 = (MusicData)base.data[l];
				if (!string.IsNullOrEmpty(musicData15.noteData.prefab_name))
				{
					GameObject gameObject2 = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicData15.noteData.prefab_name);
					if ((bool)gameObject2)
					{
						SpineActionController component3 = gameObject2.GetComponent<SpineActionController>();
						musicData15.dt = (decimal)component3.startDelay;
						base.data[l] = musicData15;
					}
				}
				decimal num6 = musicData15.tick - musicData15.dt;
				delay = ((!(num6 < delay)) ? delay : num6);
			}
			delay = decimal.Round(delay, 3);
			floatDelay = (float)delay;
			Sort();
			List<MusicData> list2 = base.data.Cast<MusicData>().ToList();
			for (int n = 1; n < base.data.Count; n++)
			{
				MusicData d2 = (MusicData)base.data[n];
				d2.doubleIdx = -1;
				if (d2.noteData.type != 1 && d2.noteData.type != 4)
				{
					base.data[n] = d2;
					continue;
				}
				int num7 = list2.FindIndex((MusicData m) => m.tick == d2.tick && m.isAir != d2.isAir && (m.noteData.type == 1 || m.noteData.type == 4));
				if (num7 != -1)
				{
					MusicData musicData16 = list2[num7];
					if (musicData16.noteData.ibms_id != "0E")
					{
						num7 = 9999;
					}
				}
				d2.doubleIdx = num7;
				d2.objId = (short)n;
				base.data[n] = d2;
			}
			for (int num8 = 0; num8 < base.data.Count; num8++)
			{
				MusicData musicData17 = (MusicData)base.data[num8];
				musicData17.tick -= delay;
				musicData17.showTick = decimal.Round(musicData17.tick - musicData17.dt, 2);
				if (musicData17.isLongPressType)
				{
					musicData17.endIndex -= (int)(delay / 0.001m);
				}
				base.data[num8] = musicData17;
			}
		}

		public void Sort()
		{
			List<object> list = data.ToArray().ToList();
			list.Sort(delegate(object l, object r)
			{
				MusicData musicData2 = (MusicData)l;
				MusicData musicData3 = (MusicData)r;
				if (musicData2.tick == 0m)
				{
					return -1;
				}
				return (musicData3.tick == 0m) ? 1 : ((!(musicData3.tick - musicData3.dt - (musicData2.tick - musicData2.dt) > 0m)) ? 1 : (-1));
			});
			data.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				MusicData musicData = (MusicData)list[i];
				musicData.tick = decimal.Round(musicData.tick, 3);
				musicData.objId = (short)i;
				data.Add(musicData);
			}
		}

		public List<MusicData> GetMusicDataFromStageInfo(string key, bool isOld = false)
		{
			ClearData();
			stageInfo = Singleton<AssetBundleManager>.instance.LoadFromName<StageInfo>(key);
			if (isOld)
			{
				string arg = $"{Application.dataPath}/EditorResources/CheckOldStageInfos";
				string url = $"file://{arg}/{key}.asset";
				WWW wWW = new WWW(url);
				while (!wWW.isDone)
				{
				}
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				using (MemoryStream serializationStream = new MemoryStream(wWW.bytes))
				{
					stageInfo = (StageInfo)binaryFormatter.Deserialize(serializationStream);
				}
			}
			sceneEvents = new List<SceneEvent>(stageInfo.sceneEvents);
			bpm = stageInfo.bpm;
			List<MusicData> list = new List<MusicData>(stageInfo.musicDatas);
			delay = stageInfo.delay;
			List<NoteConfigData> data = NodeConfigReader.Instance.GetData();
			bool entityValue = CustomDefines.GetEntityValue<bool>("Game/IsFool");
			for (int i = 0; i < list.Count; i++)
			{
				MusicData value = list[i];
				foreach (NoteConfigData item in data)
				{
					if (item.uid == value.configData.note_uid && item.pathway == value.configData.pathway)
					{
						value.noteData = item;
						list[i] = value;
						if (!entityValue || !(item.boss_action != "0") || item.damage <= 0 || item.type != 1)
						{
							break;
						}
						if (item.prefab_name.EndsWith("_fool"))
						{
							value.noteData = item;
							list[i] = value;
							break;
						}
					}
				}
			}
			floatDelay = (float)delay;
			return list;
		}

		public void LoadJKSkill(List<MusicData> musicDatas)
		{
			if (!Singleton<BattleProperty>.instance.isCatchAvailable)
			{
				return;
			}
			List<MusicData> list = new List<MusicData>(musicDatas);
			list.Sort(delegate(MusicData l, MusicData r)
			{
				if (l.tick == 0m)
				{
					return -1;
				}
				return (r.tick == 0m) ? 1 : ((!(r.tick - l.tick > 0m)) ? 1 : (-1));
			});
			List<int> catchHeadNotes = GameGlobal.gGameMusic.catchHeadNotes;
			List<int> catchNotes = GameGlobal.gGameMusic.catchNotes;
			MusicData musicData = default(MusicData);
			MusicData musicData2 = default(MusicData);
			int item = -1;
			int item2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				MusicData musicData3 = list[i];
				if ((musicData3.noteData.type != 3 || musicData3.isLongPressing) && musicData3.noteData.type != 1)
				{
					continue;
				}
				MusicData musicData4 = (!musicData3.isAir) ? musicData : musicData2;
				if (musicData4.tick > 0m)
				{
					float num = (float)(musicData3.tick - musicData4.tick);
					if (musicData4.isLongPressStart && musicData3.isLongPressEnd)
					{
						if (musicData3.isAir)
						{
							item = musicData4.objId;
						}
						else
						{
							item2 = musicData4.objId;
						}
					}
					if (num <= Singleton<BattleProperty>.instance.catchGapThreshold)
					{
						if (!catchNotes.Contains(musicData4.objId))
						{
							if (!catchHeadNotes.Contains(musicData4.objId))
							{
								catchHeadNotes.Add(musicData4.objId);
							}
							catchNotes.Add(musicData4.objId);
							if (musicData4.isLongPressEnd)
							{
								if (musicData4.isAir)
								{
									if (!catchNotes.Contains(item))
									{
										if (!catchHeadNotes.Contains(item))
										{
											catchHeadNotes.Add(item);
										}
										catchNotes.Add(item);
									}
								}
								else if (!catchNotes.Contains(item2))
								{
									if (!catchHeadNotes.Contains(item2))
									{
										catchHeadNotes.Add(item2);
									}
									catchNotes.Add(item2);
								}
							}
						}
						if (!catchNotes.Contains(musicData3.objId))
						{
							catchNotes.Add(musicData3.objId);
						}
					}
				}
				if (musicData3.isAir)
				{
					musicData2 = musicData3;
				}
				else
				{
					musicData = musicData3;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				MusicData md = list[j];
				if (!md.isLongPressEnd && !md.isLongPressing)
				{
					continue;
				}
				MusicData musicData5 = list.Find((MusicData m) => m.configData.time == md.longPressPTick && m.isLongPressStart && m.isAir == md.isAir);
				if (musicData5.isLongPressStart)
				{
					short objId = musicData5.objId;
					if (catchNotes.Contains(objId))
					{
						catchNotes.Add(md.objId);
					}
				}
			}
		}

		public override ArrayList GetData(string key)
		{
			ClearData();
			Init(key);
			return GetData();
		}

		public override object GetData(int idx)
		{
			if (idx < 0)
			{
				return null;
			}
			ArrayList data = GetData();
			if (data == null || idx > data.Count)
			{
				return null;
			}
			return data[idx];
		}
	}
}
