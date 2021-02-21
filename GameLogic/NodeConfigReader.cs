using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;

namespace GameLogic
{
	public class NodeConfigReader : BaseConfigReader
	{
		public static NodeConfigReader Instance = new NodeConfigReader();

		private NodeConfigReader()
		{
		}

		public new List<NoteConfigData> GetData()
		{
			return SingletonScriptableObject<NoteDataMananger>.instance.noteDatas;
		}
	}
}
