using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using GameLogic;
using Newtonsoft.Json.Linq;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class iBMSCManager : Singleton<iBMSCManager>
	{
		public class BMS
		{
			public JObject info;

			public JArray notes;

			public JArray notesPercent;

			public string md5;

			public float GetBpm()
			{
				JToken jToken = info["BPM"];
				if (jToken.IsNullOrEmpty())
				{
					return (float)info["BPM01"];
				}
				return (float)jToken;
			}

			public JArray GetSceneEvents()
			{
				JArray jArray = new JArray();
				for (int i = 0; i < notes.Count; i++)
				{
					JToken jToken = notes[i];
					JToken value = jToken["value"];
					string text = (string)value;
					if (!string.IsNullOrEmpty(text))
					{
						string a = (string)jToken["tone"];
						if (a == "01")
						{
							JObject jObject = new JObject();
							jObject["time"] = (float)jToken["time"];
							jObject["uid"] = $"SceneEvent/{text}";
							jArray.Add(jObject);
						}
						else if (a == "03")
						{
							JObject jObject2 = new JObject();
							jObject2["time"] = (float)jToken["time"];
							jObject2["uid"] = "SceneEvent/OnBPMChanged";
							jObject2["value"] = text;
							jArray.Add(jObject2);
						}
					}
				}
				return jArray;
			}

			public JArray GetBloodNotes()
			{
				JArray jArray = new JArray();
				for (int i = 0; i < notes.Count; i++)
				{
					JToken jToken = notes[i];
					string value = (string)jToken["value"];
					if (!string.IsNullOrEmpty(value))
					{
						string a = (string)jToken["tone"];
						if (a == "34")
						{
							JObject jObject = new JObject();
							jObject["time"] = (float)jToken["time"];
							jObject["uid"] = value;
							jObject["pathway"] = 0;
							jArray.Add(jObject);
						}
						else if (a == "33")
						{
							JObject jObject2 = new JObject();
							jObject2["time"] = (float)jToken["time"];
							jObject2["uid"] = value;
							jObject2["pathway"] = 1;
							jArray.Add(jObject2);
						}
					}
				}
				return jArray;
			}

			public JArray GetNoteSpeedChangeNotes()
			{
				JArray jArray = new JArray();
				for (int i = 0; i < jArray.Count; i++)
				{
					JToken jToken = jArray[i];
					JToken value = jToken["value"];
					string value2 = (string)value;
					if (!string.IsNullOrEmpty(value2))
					{
						string a = (string)jToken["tone"];
						if (a == "15")
						{
							JObject jObject = new JObject();
							jObject["time"] = (float)jToken["time"];
							jObject["value"] = jToken["value"];
							jArray.Add(jObject);
						}
					}
				}
				return jArray;
			}

			public JArray GetNoteDatas()
			{
				List<NoteConfigData> data = NodeConfigReader.Instance.GetData();
				JArray jArray = new JArray();
				string scene = (string)info["GENRE"];
				int num = 1;
				int num2 = 1;
				JToken value2;
				if (info.TryGetValue("PLAYER", out value2))
				{
					num2 = (num = int.Parse((string)value2));
				}
				int num3 = 1;
				string preBossAction = "0";
				float num4 = 0f;
				for (int i = 0; i < notes.Count; i++)
				{
					JToken jToken = notes[i];
					string value = (string)jToken["value"];
					if (string.IsNullOrEmpty(value))
					{
						continue;
					}
					string text = (string)jToken["tone"];
					int pathway = -1;
					switch (text)
					{
					case "13":
					case "53":
					case "33":
						pathway = 1;
						break;
					}
					switch (text)
					{
					case "14":
					case "54":
					case "34":
						pathway = 0;
						break;
					}
					if (text == "15" || text == "15")
					{
						pathway = 0;
					}
					if (pathway == -1)
					{
						continue;
					}
					if (value == "0O")
					{
						num2 = (num = 1);
					}
					else if (value == "0P")
					{
						num2 = (num = 2);
					}
					else if (value == "0Q")
					{
						num2 = (num = 3);
					}
					else if (value == "0R")
					{
						num2 = 1;
					}
					else if (value == "0S")
					{
						num2 = 2;
					}
					else if (value == "0T")
					{
						num2 = 3;
					}
					else if (value == "0U")
					{
						num = 1;
					}
					else if (value == "0V")
					{
						num = 2;
					}
					else if (value == "0W")
					{
						num = 3;
					}
					float num5 = (float)jToken["time"];
					decimal value3 = 0m;
					int speed = (pathway != 1) ? num2 : num;
					if (value == "OE" && num != num2)
					{
						speed = Mathf.Min(num, num2);
					}
					NoteConfigData noteConfigData = data.Find(delegate(NoteConfigData n)
					{
						string scene6 = n.scene;
						int num10 = (n.type != 5 && n.type != 0 && !(value == "16") && !(value == "17")) ? n.speed : speed;
						scene6 = ((!(scene6 == "0")) ? scene6 : scene);
						return n.ibms_id == value && scene6 == scene && n.pathway == pathway && num10 == speed;
					});
					if (string.IsNullOrEmpty(noteConfigData.uid))
					{
						continue;
					}
					string boss_action = noteConfigData.boss_action;
					if (value == "18")
					{
						string text2 = string.Empty;
						float num6 = 0f;
						for (int j = i + 1; j < notes.Count; j++)
						{
							JToken jToken2 = notes[j];
							string v = (string)jToken2["value"];
							if (v != "18")
							{
								NoteConfigData noteConfigData2 = data.Find(delegate(NoteConfigData d)
								{
									string scene5 = d.scene;
									scene5 = ((!(scene5 == "0")) ? scene5 : scene);
									return d.ibms_id == v && scene5 == scene;
								});
								string boss_action2 = noteConfigData2.boss_action;
								if (!(boss_action2 == "0"))
								{
									text2 = boss_action2;
									num6 = (float)jToken2["time"];
									break;
								}
							}
						}
						if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(preBossAction))
						{
							switch (text2)
							{
							case "boss_far_atk_1_L":
							case "boss_far_atk_1_R":
							case "boss_far_atk_2":
							{
								float num7 = Mathf.Abs(num4 - num5);
								float num8 = Mathf.Abs(num6 - num5);
								if (num7 > num8)
								{
									preBossAction = text2;
								}
								break;
							}
							}
						}
						if (preBossAction == "boss_far_atk_1_start" || preBossAction == "boss_far_atk_1_L" || preBossAction == "boss_far_atk_1_R")
						{
							preBossAction = ((pathway != 1) ? "boss_far_atk_1_R" : "boss_far_atk_1_L");
							noteConfigData = data.Find(delegate(NoteConfigData n)
							{
								string scene4 = n.scene;
								scene4 = ((!(scene4 == "0")) ? scene4 : scene);
								return n.ibms_id == value && scene4 == scene && n.pathway == pathway && n.boss_action == preBossAction && n.speed == speed;
							});
						}
						else if (preBossAction == "boss_far_atk_2_start" || preBossAction == "boss_far_atk_2")
						{
							preBossAction = "boss_far_atk_2";
							noteConfigData = data.Find(delegate(NoteConfigData n)
							{
								string scene3 = n.scene;
								scene3 = ((!(scene3 == "0")) ? scene3 : scene);
								return n.ibms_id == value && scene3 == scene && n.pathway == pathway && n.boss_action == preBossAction && n.speed == speed;
							});
						}
						else if (!string.IsNullOrEmpty(text2))
						{
							if (text2 == "boss_far_atk_1_L" || text2 == "boss_far_atk_1_R")
							{
								text2 = ((pathway != 1) ? "boss_far_atk_1_R" : "boss_far_atk_1_L");
							}
							preBossAction = text2;
							noteConfigData = data.Find(delegate(NoteConfigData n)
							{
								string scene2 = n.scene;
								scene2 = ((!(scene2 == "0")) ? scene2 : scene);
								return n.ibms_id == value && scene2 == scene && n.pathway == pathway && n.boss_action == preBossAction && n.speed == speed;
							});
						}
						boss_action = noteConfigData.boss_action;
					}
					if (noteConfigData.type == 3 || noteConfigData.type == 8)
					{
						for (int k = i + 1; k < notes.Count; k++)
						{
							JToken jToken3 = notes[k];
							float num9 = (float)jToken3["time"];
							string a = (string)jToken3["value"];
							string a2 = (string)jToken3["tone"];
							if (a == value && a2 == text)
							{
								value3 = (decimal)(num9 - num5);
								notes[k]["value"] = string.Empty;
								break;
							}
						}
					}
					if (boss_action != "0" && !string.IsNullOrEmpty(boss_action))
					{
						preBossAction = boss_action;
						num4 = num5;
					}
					JObject jObject = new JObject();
					jObject["id"] = num3++;
					jObject["time"] = num5;
					jObject["note_uid"] = noteConfigData.uid;
					jObject["length"] = value3;
					jObject["pathway"] = pathway;
					if (text == "34" || text == "33")
					{
						jObject["blood"] = true;
					}
					else
					{
						jObject["blood"] = false;
					}
					jArray.Add(jObject);
				}
				return jArray;
			}
		}

		public const string sceneEventTone = "01";

		public const string beatPercentTone = "02";

		public const string bpmTone = "03";

		public const string bpmFloatTone = "08";

		public Dictionary<string, float> bpmTones = new Dictionary<string, float>();

		public const string airNormalNoteTone = "13";

		public const string groundNormalNoteTone = "14";

		public const string bossActionNoteTone = "15";

		public const string noteSpeedChange = "15";

		public const string sceneChangeTone = "15";

		public const string bossMulHit1Value = "16";

		public const string bossMulHit2Value = "17";

		public const string bossBlockValue = "18";

		public const string airBloodNoteTone = "33";

		public const string groundBloodNoteTone = "34";

		public const string airLengthNoteTone = "53";

		public const string groundLengthNoteTone = "54";

		public string bmsFile
		{
			get;
			private set;
		}

		public string bmsFileEditorMode
		{
			get;
			private set;
		}

		public string oldBmsFile
		{
			get;
			private set;
		}

		private void Init()
		{
			bmsFile = $"{Application.dataPath}/Static Resources/Map";
			oldBmsFile = $"{Application.dataPath}/EditorResources/CheckOldStageInfos";
			bmsFileEditorMode = $"{Application.streamingAssetsPath}/map";
		}

		public List<BMS> LoadAll()
		{
			char splitChar = '/';
			splitChar = '\\';
			string[] files = Directory.GetFiles(bmsFileEditorMode, "*", SearchOption.AllDirectories);
			return (from file in files
				where file.EndsWith(".bms") && !file.Contains("__TempBMS.bms")
				select Load(file.LastAfter(splitChar).Replace(".bms", string.Empty), false)).ToList();
		}

		public BMS Load(string bmsName, bool loadNotes = true, bool isOld = false)
		{
			bool flag = false;
			flag = bool.Parse(SingletonScriptableObject<ConstanceManager>.instance["IsEditorMode"]);
			string url = $"file://{(flag ? bmsFileEditorMode : bmsFile)}/{bmsName}.bms";
			if (isOld)
			{
				url = $"file://{(flag ? bmsFileEditorMode : oldBmsFile)}/{bmsName}.bms";
			}
			bpmTones.Clear();
			WWW wWW = new WWW(url);
			while (!wWW.isDone)
			{
			}
			if (!string.IsNullOrEmpty(wWW.error))
			{
				Debug.Log($"{bmsName} Not Found!");
				wWW.Dispose();
				return null;
			}
			JObject jObject = new JObject();
			JArray jArray = new JArray();
			byte[] bytes = wWW.bytes;
			JArray jArray2 = new JArray();
			List<JObject> list = new List<JObject>();
			StreamReader streamReader = new StreamReader(new MemoryStream(bytes), AssetsUtils.GetBytesEncodeType(bytes));
			byte[] array = MD5.Create().ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			string md = stringBuilder.ToString();
			wWW.Dispose();
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (string.IsNullOrEmpty(text) || !text.Contains("#"))
				{
					continue;
				}
				text = text.Substring(1, text.Length - 1);
				if (text.Contains(" "))
				{
					string[] array2 = text.Split(' ');
					string text2 = array2[0];
					string text3 = text.Replace(text2 + " ", string.Empty);
					jObject[text2] = text3;
					if (text2 == "BPM")
					{
						float value = 60f / float.Parse(text3) * 4f;
						JObject jObject2 = new JObject();
						jObject2["tick"] = 0f;
						jObject2["freq"] = value;
						list.Add(jObject2);
					}
					else if (text2.Contains("BPM"))
					{
						bpmTones.Add(text2.Replace("BPM", string.Empty), float.Parse(text3));
					}
				}
				else
				{
					if (!text.Contains(":") || !loadNotes)
					{
						continue;
					}
					string[] array3 = text.Split(':');
					string text4 = array3[0];
					string text5 = array3[1];
					int num = int.Parse(text4.Substring(0, 3));
					string text6 = text4.Substring(3, 2);
					if (text6 == "02")
					{
						JObject jObject3 = new JObject();
						jObject3["beat"] = num;
						jObject3["percent"] = float.Parse(text5);
						jArray2.Add(jObject3);
						continue;
					}
					int num2 = text5.Length / 2;
					for (int j = 0; j < num2; j++)
					{
						string text7 = text5.Substring(j * 2, 2);
						if (text7 == "00")
						{
							continue;
						}
						float theTick = (float)j / (float)num2 + (float)num;
						if (text6 == "03" || text6 == "08")
						{
							float value2 = 60f / ((!(text6 == "08") || !bpmTones.ContainsKey(text7)) ? ((float)Convert.ToInt32(text7, 16)) : bpmTones[text7]) * 4f;
							JObject jObject4 = new JObject();
							jObject4["tick"] = theTick;
							jObject4["freq"] = value2;
							list.Add(jObject4);
							list.Sort(delegate(JObject l, JObject r)
							{
								float num15 = (float)l["tick"];
								float num16 = (float)r["tick"];
								return (!(num15 > num16)) ? 1 : (-1);
							});
							continue;
						}
						float num3 = 0f;
						float num4 = 0f;
						List<JObject> list2 = list.FindAll((JObject b) => (float)b["tick"] < theTick);
						for (int num5 = list2.Count - 1; num5 >= 0; num5--)
						{
							JObject jObject5 = list2[num5];
							float num6 = 0f;
							float num7 = (float)jObject5["freq"];
							if (num5 - 1 >= 0)
							{
								JObject jObject6 = list2[num5 - 1];
								num6 = (float)jObject6["tick"] - (float)jObject5["tick"];
							}
							if (num5 == 0)
							{
								num6 = theTick - (float)jObject5["tick"];
							}
							float num8 = num4;
							num4 += num6;
							int num9 = Mathf.FloorToInt(num8);
							int num10 = Mathf.CeilToInt(num4);
							for (int k = num9; k < num10; k++)
							{
								int index = k;
								float num11 = 1f;
								if (k == num9)
								{
									num11 = (float)(k + 1) - num8;
								}
								if (k == num10 - 1)
								{
									num11 = num4 - (float)(num10 - 1);
								}
								if (num10 == num9 + 1)
								{
									num11 = num4 - num8;
								}
								JToken jToken = jArray2.Find((JToken pc) => (int)pc["beat"] == index);
								float num12 = (jToken == null) ? 1f : ((float)jToken["percent"]);
								num3 += (float)Mathf.RoundToInt(num11 * num12 * num7 / 1E-06f) * 1E-06f;
							}
						}
						JObject jObject7 = new JObject();
						jObject7["time"] = num3;
						jObject7["value"] = text7;
						jObject7["tone"] = text6;
						jArray.Add(jObject7);
					}
				}
			}
			jArray.Sort(delegate(JToken l, JToken r)
			{
				float num13 = (float)l["time"];
				float num14 = (float)r["time"];
				if (num13 > num14)
				{
					return 1;
				}
				return (num14 > num13) ? (-1) : 0;
			});
			BMS bMS = new BMS();
			bMS.info = jObject;
			bMS.notes = jArray;
			bMS.notesPercent = jArray2;
			bMS.md5 = md;
			BMS bMS2 = bMS;
			bMS2.info["NAME"] = bmsName;
			bMS2.info["NEW"] = true;
			List<string> list3 = Singleton<ConfigManager>.instance.GetObject<List<string>>("BmsNames") ?? new List<string>();
			if (list3.Contains(bmsName))
			{
				bMS2.info["NEW"] = false;
			}
			else
			{
				list3.Add(bmsName);
				Singleton<ConfigManager>.instance.SaveObject("BmsNames", list3);
			}
			if (bMS2.info.Properties().ToList().Find((JProperty p) => p.Name == "BANNER") == null)
			{
				bMS2.info["BANNER"] = "cover/none_cover.png";
			}
			else
			{
				bMS2.info["BANNER"] = "cover/" + (string)bMS2.info["BANNER"];
			}
			return bMS2;
		}
	}
}
