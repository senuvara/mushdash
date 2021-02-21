using Assets.Scripts.GameCore.Skill;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore.Managers
{
	public class SkillManager : Singleton<SkillManager>
	{
		public ISkill characterSkill;

		public ISkill elfinSkill;

		public void Apply()
		{
			characterSkill = null;
			elfinSkill = null;
			int result = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
			int result2 = Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].GetResult<int>();
			List<Type> list = from t in typeof(ISkill).Assembly.GetTypes()
				where t.InheritsFrom(typeof(ISkill))
				select t;
			for (int i = 0; i < list.Count; i++)
			{
				Type type = list[i];
				if (type == typeof(ISkill))
				{
					continue;
				}
				ISkill skill = Activator.CreateInstance(type) as ISkill;
				if (skill != null)
				{
					string uid = skill.uid;
					if (uid == $"Character_{result}")
					{
						characterSkill = skill;
					}
					if (uid == $"Elfin_{result2}")
					{
						elfinSkill = skill;
					}
				}
				if (characterSkill != null && elfinSkill != null)
				{
					break;
				}
			}
			if (characterSkill != null)
			{
				characterSkill.Apply();
			}
			if (elfinSkill != null)
			{
				elfinSkill.Apply();
			}
		}
	}
}
