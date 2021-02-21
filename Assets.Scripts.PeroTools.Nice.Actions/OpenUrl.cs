using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class OpenUrl : Action
	{
		[Variable("http://www.google.com", null, false)]
		public IVariable url;

		public override void Execute()
		{
			Application.OpenURL(url.GetResult<string>());
		}
	}
}
