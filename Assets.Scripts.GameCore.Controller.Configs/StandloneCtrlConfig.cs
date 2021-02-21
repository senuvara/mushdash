using Assets.Scripts.PeroTools.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.Controller.Configs
{
	[CreateAssetMenu(fileName = "New CtrlConfig", menuName = "PeroPeroGames/ControllerConfig/Standlone", order = 1)]
	public class StandloneCtrlConfig : ControllerConfig
	{
		public string CurrentProposal = InputManager.MDButtonProposalName.Default;

		[SerializeField]
		private Dictionary<string, Dictionary<string, List<KeyCode>>> m_ButtonKeyEnties;

		public Dictionary<string, Dictionary<string, List<KeyCode>>> buttonKeyEnties => m_ButtonKeyEnties;
	}
}
