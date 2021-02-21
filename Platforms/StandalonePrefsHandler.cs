using Assets.Scripts.PeroTools.Commons;
using PeroTools.Saves;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Platforms
{
	public class StandalonePrefsHandler : PrefsHandler
	{
		private string m_SaveLocation;

		private Dictionary<string, string> m_Content;

		public StandalonePrefsHandler()
		{
			m_SaveLocation = Path.Combine(Application.persistentDataPath, "Saves").Replace("\\", "/");
			if (!File.Exists(m_SaveLocation))
			{
				File.Create(m_SaveLocation);
			}
			LoadContent();
		}

		private void LoadContent()
		{
			m_Content = JsonUtils.Deserialize<Dictionary<string, string>>(File.ReadAllText(m_SaveLocation));
			if (m_Content == null)
			{
				m_Content = new Dictionary<string, string>();
			}
		}

		private void SaveContent()
		{
			File.WriteAllText(m_SaveLocation, JsonUtils.Serialize(m_Content));
		}

		public override void SaveString(string key, string s)
		{
			m_Content[key] = s;
		}

		public override string GetString(string key)
		{
			string value;
			m_Content.TryGetValue(key, out value);
			return value;
		}

		public override void Delete(string key)
		{
			if (m_Content.ContainsKey(key))
			{
				m_Content.Remove(key);
			}
		}

		public override void Save()
		{
			SaveContent();
		}

		public override void ClearAllPrefs()
		{
			m_Content.Clear();
			Save();
		}
	}
}
