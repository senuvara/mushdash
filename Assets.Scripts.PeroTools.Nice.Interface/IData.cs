using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Nice.Interface
{
	public interface IData
	{
		Dictionary<string, IVariable> fields
		{
			get;
		}

		IVariable this[string uid]
		{
			get;
			set;
		}
	}
}
