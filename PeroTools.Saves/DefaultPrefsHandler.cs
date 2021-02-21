using UnityEngine;

namespace PeroTools.Saves
{
	public class DefaultPrefsHandler : PrefsHandler
	{
		public override void SaveString(string key, string s)
		{
			PlayerPrefs.SetString(key, s);
		}

		public override string GetString(string key)
		{
			return PlayerPrefs.GetString(key);
		}

		public override void Delete(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public override void Save()
		{
			PlayerPrefs.Save();
		}

		public override void ClearAllPrefs()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}
