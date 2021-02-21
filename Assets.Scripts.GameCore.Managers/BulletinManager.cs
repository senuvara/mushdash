using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class BulletinManager : Singleton<BulletinManager>
	{
		public class Bulletin
		{
			public string uid;

			public string title;

			public string content;

			public string imageUrl;

			public bool force;

			public Texture2D texture;

			public bool isNew
			{
				get
				{
					return !Singleton<DataManager>.instance["Account"]["Bulletins"].GetResult<List<string>>().Contains(uid);
				}
				set
				{
					List<string> result = Singleton<DataManager>.instance["Account"]["Bulletins"].GetResult<List<string>>();
					if (value)
					{
						if (!isNew)
						{
							result.Remove(uid);
							Singleton<DataManager>.instance["Account"].Save();
						}
					}
					else if (isNew)
					{
						result.Add(uid);
						Singleton<DataManager>.instance["Account"].Save();
					}
				}
			}

			public void GetTexture(Action<Texture2D> callback)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(LoadTexture(callback));
			}

			private IEnumerator LoadTexture(Action<Texture2D> callback)
			{
				if (texture != null)
				{
					callback(texture);
					yield break;
				}
				WWW www = new WWW(imageUrl);
				try
				{
					yield return new WaitForSeconds(2f);
					yield return www;
					yield return www.texture;
					if (www.texture != null)
					{
						texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);
						www.LoadImageIntoTexture(texture);
						callback(texture);
					}
				}
				finally
				{
					base._003C_003E__Finally0();
				}
			}
		}

		public Dictionary<string, List<Bulletin>> bulletins
		{
			get;
			private set;
		}

		public void RefreshBulletin(Action<Dictionary<string, List<Bulletin>>> callback = null)
		{
			string text = "musedash/announce";
			string key = "announce";
			text = "musedash/announce/pc";
			key = "pc_announce";
			Singleton<ServerManager>.instance.SendToUrl(text, "GET", null, delegate(JObject r)
			{
				Dictionary<string, List<Bulletin>> dictionary = new Dictionary<string, List<Bulletin>>();
				JToken jToken = r[key];
				foreach (JToken item in (IEnumerable<JToken>)jToken)
				{
					JToken jToken2 = item["platform"];
					if (jToken2.Count() > 0)
					{
						foreach (JToken item2 in (IEnumerable<JToken>)jToken2)
						{
							if ((string)item2 == "steam")
							{
								BulletinAdd(item, dictionary);
							}
						}
					}
				}
				bulletins = dictionary;
				if (callback != null)
				{
					callback(dictionary);
				}
			});
		}

		private void BulletinAdd(JToken announce, Dictionary<string, List<Bulletin>> dic)
		{
			string text = "announce_item";
			text = "pc_announce_item";
			JToken jToken = announce[text];
			foreach (JToken item2 in (IEnumerable<JToken>)jToken)
			{
				string key = (string)item2["language_name"];
				List<Bulletin> list = new List<Bulletin>();
				if (dic.ContainsKey(key))
				{
					list = dic[key];
				}
				else
				{
					dic.Add(key, list);
				}
				Bulletin bulletin = new Bulletin();
				bulletin.content = (string)item2["content"];
				bulletin.force = (bool)announce["is_pop"];
				bulletin.imageUrl = (string)item2["title_map"];
				bulletin.title = (string)item2["title"];
				bulletin.uid = (string)announce["uid"];
				Bulletin item = bulletin;
				list.Add(item);
			}
		}
	}
}
