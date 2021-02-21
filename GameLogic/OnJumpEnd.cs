using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Spine;
using UnityEngine;

namespace GameLogic
{
	public class OnJumpEnd : DoNothing
	{
		private SpineActionController m_Sac;

		private Animator m_Animator;

		public override void Init()
		{
			if ((bool)gameObject)
			{
				m_Sac = gameObject.GetComponent<SpineActionController>();
				m_Animator = gameObject.GetComponent<Animator>();
			}
		}

		public override void Do(TrackEntry entry)
		{
			Singleton<EffectManager>.instance.Play("dust_fx");
			if ((bool)GirlActionController.instance)
			{
				SpineActionController.Play("char_run", gameObject, 0f, m_Sac, m_Animator);
				SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
			}
		}
	}
}
