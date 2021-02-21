using System;

namespace Assets.Scripts.PeroTools.Nice.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class OnConstanceChangedAttribute : Attribute
	{
		public string methodName;

		public OnConstanceChangedAttribute(string name)
		{
			methodName = name;
		}
	}
}
