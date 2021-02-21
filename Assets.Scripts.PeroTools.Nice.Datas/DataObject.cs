using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Datas
{
	[CreateAssetMenu(fileName = "NewData", menuName = "Nice/Data/Data", order = 0)]
	[HideMonoScript]
	public class DataObject : SerializedScriptableObject, IData
	{
		[SerializeField]
		protected Dictionary<string, IVariable> m_Fields = new Dictionary<string, IVariable>();

		public Dictionary<string, IVariable> fields => m_Fields;

		public IVariable this[string uid]
		{
			get
			{
				return this.Get(uid);
			}
			set
			{
				this.Set(uid, value);
			}
		}
	}
}
