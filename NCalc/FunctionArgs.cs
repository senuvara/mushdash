using System;

namespace NCalc
{
	public class FunctionArgs : EventArgs
	{
		private object _result;

		private Expression[] _parameters = new Expression[0];

		public object Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
				HasResult = true;
			}
		}

		public bool HasResult
		{
			get;
			set;
		}

		public Expression[] Parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters = value;
			}
		}

		public object[] EvaluateParameters()
		{
			object[] array = new object[_parameters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = _parameters[i].Evaluate();
			}
			return array;
		}
	}
}
