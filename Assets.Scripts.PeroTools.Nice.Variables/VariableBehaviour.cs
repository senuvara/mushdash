using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class VariableBehaviour : SerializedMonoBehaviour, IVariable, IValue
	{
		public IVariable variable;

		public object result
		{
			get
			{
				return variable.result;
			}
			set
			{
				variable.result = value;
			}
		}
	}
}
