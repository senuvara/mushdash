using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class ConstanceManager : SingletonScriptableObject<ConstanceManager>
	{
		[Serializable]
		public class ConstanceInfo
		{
			public string key;

			public string value;

			public string description;
		}

		public List<ConstanceInfo> constances = new List<ConstanceInfo>();

		public string this[string key]
		{
			get
			{
				ConstanceInfo constanceInfo = constances.Find((ConstanceInfo c) => c.key == key);
				if (constanceInfo == null)
				{
					Debug.LogErrorFormat("Unable to find Constance with key [{0}]", key);
					return null;
				}
				return constanceInfo.value;
			}
		}

		public ConstanceInfo Get(string key)
		{
			return constances.Find((ConstanceInfo c) => c.key == key);
		}

		public float GetFloat(string key)
		{
			return float.Parse(this[key]);
		}

		public int GetInt(string key)
		{
			return int.Parse(this[key]);
		}

		public decimal GetDecimal(string key)
		{
			return decimal.Parse(this[key]);
		}

		public bool GetBool(string key)
		{
			return bool.Parse(this[key]);
		}
	}
}
