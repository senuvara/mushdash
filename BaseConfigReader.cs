using GameLogic;
using LitJson;
using System.Collections;

public class BaseConfigReader
{
	protected ArrayList data = new ArrayList();

	public virtual void Init()
	{
	}

	public virtual void Init(string filename)
	{
	}

	public virtual ArrayList GetData()
	{
		return data;
	}

	public virtual object GetData(int idx)
	{
		return null;
	}

	public virtual ArrayList GetData(string key)
	{
		return null;
	}

	public virtual int GetDataCount()
	{
		return data.Count;
	}

	public virtual int GetDataCount(int idx)
	{
		return -1;
	}

	public virtual void SetData(ArrayList list)
	{
		data = list;
	}

	public virtual void ClearData()
	{
		data.Clear();
	}

	public virtual int GetDataCount(string key)
	{
		return -1;
	}

	public void Add(object obj)
	{
		data.Add(obj);
	}

	public JsonData GetJsonConfig(string filename)
	{
		JsonData jsonData = GameGlobal.gConfigLoader.LoadAsJsonData(filename);
		if (jsonData == null)
		{
			return null;
		}
		return GameGlobal.gConfigLoader.JsonFromExcelParse(jsonData);
	}
}
