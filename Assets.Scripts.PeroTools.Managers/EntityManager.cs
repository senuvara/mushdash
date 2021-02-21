using Assets.Scripts.Common;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Managers
{
	public class EntityManager : Singleton<EntityManager>
	{
		public readonly Dictionary<string, object> entities = CustomDefines.entities;

		public object this[string key]
		{
			get
			{
				if (entities.ContainsKey(key))
				{
					return entities[key];
				}
				return null;
			}
		}
	}
}
