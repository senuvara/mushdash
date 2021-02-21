using Assets.Scripts.GameCore.HostComponent;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class CheatAndDo : MonoBehaviour
	{
		public void DeadObscured()
		{
			BattleRoleAttributeComponent.instance.Dead();
		}

		public void DeadSpeedHack()
		{
			BattleRoleAttributeComponent.instance.Dead();
		}
	}
}
