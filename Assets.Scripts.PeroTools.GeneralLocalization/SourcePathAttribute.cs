using System;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	[AttributeUsage(AttributeTargets.Class)]
	public class SourcePathAttribute : Attribute
	{
		public string path
		{
			get;
			private set;
		}

		public int order
		{
			get;
			private set;
		}

		public SourcePathAttribute(string path, int order = 0)
		{
			this.path = path;
			this.order = order;
		}
	}
}
