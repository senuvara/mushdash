namespace PeroTools.Saves
{
	public abstract class PrefsHandler
	{
		public abstract void SaveString(string key, string s);

		public abstract string GetString(string key);

		public abstract void Delete(string key);

		public abstract void Save();

		public abstract void ClearAllPrefs();
	}
}
