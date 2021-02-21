using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Values;
using Assets.Scripts.PeroTools.Nice.Variables;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Scripts.PeroTools.Nice.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class VariableAttribute : Attribute
	{
		public Type variableType;

		public Type valueType;

		public string methodName;

		public object defualtValue;

		public Type[] applyTypes;

		public bool hideArrow;

		public VariableAttribute(params Type[] types)
		{
			variableType = typeof(Constance);
			valueType = typeof(Assets.Scripts.PeroTools.Nice.Values.Object);
			applyTypes = types;
		}

		public VariableAttribute(Type type, string method = null, bool hide = false)
		{
			variableType = typeof(Constance);
			valueType = typeof(Assets.Scripts.PeroTools.Nice.Values.Object);
			hideArrow = hide;
			if (typeof(IVariable).IsAssignableFrom(type))
			{
				variableType = type;
			}
			else
			{
				Type iValueType = typeof(IValue);
				List<Type> list = (from t in iValueType.Assembly.GetTypes()
					where t.InheritsFrom(iValueType)
					select t).ToList();
				valueType = list.Find(delegate(Type t)
				{
					FieldInfo field = t.GetField("m_Result", BindingFlags.Instance | BindingFlags.NonPublic);
					return field != null && field.FieldType == type;
				});
			}
			methodName = method;
		}

		public VariableAttribute(object value, string method = null, bool hide = false)
		{
			variableType = typeof(Constance);
			Type type = typeof(IValue);
			hideArrow = hide;
			List<Type> list = (from t in type.Assembly.GetTypes()
				where t.InheritsFrom(type)
				select t).ToList();
			valueType = list.Find(delegate(Type t)
			{
				FieldInfo field = t.GetField("value");
				return field != null && field.FieldType == value.GetType();
			});
			defualtValue = value;
			methodName = method;
		}
	}
}
