using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using NCalc;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Formula : IVariable, IValue
	{
		[SerializeField]
		private string m_Expression;

		[SerializeField]
		private List<IVariable> m_Params = new List<IVariable>();

		private IData m_Data;

		private Expression m_NcalExpression;

		private static MemberInfo m_SetDataMi;

		public object result
		{
			get
			{
				return Evaluate();
			}
			set
			{
			}
		}

		private object Evaluate()
		{
			if (string.IsNullOrEmpty(m_Expression))
			{
				return null;
			}
			if (m_NcalExpression == null)
			{
				m_NcalExpression = new Expression(m_Expression);
				m_NcalExpression.EvaluateFunction += ListFunction;
				m_NcalExpression.EvaluateFunction += OtherFunction;
				m_NcalExpression.EvaluateFunction += FieldFunction;
			}
			int num = 0;
			if (m_Params != null && m_Params.Count != 0)
			{
				int num2 = 97 + m_Params.Count;
				for (char c = 'a'; c < num2; c = (char)(c + 1))
				{
					m_NcalExpression.Parameters[c.ToString()] = m_Params[num++].result;
				}
			}
			if (m_Data != null)
			{
				Dictionary<string, IVariable> fields = m_Data.fields;
				foreach (KeyValuePair<string, IVariable> item in fields)
				{
					string key = item.Key;
					if (!m_Expression.Contains(key))
					{
						continue;
					}
					int num3 = m_Expression.IndexOf(key, StringComparison.Ordinal);
					int num4 = num3 + key.Length;
					if (num4 < m_Expression.Length)
					{
						char c2 = m_Expression[num4];
						if ((c2 >= 'a' && c2 <= 'z') || (c2 >= 'A' && c2 <= 'Z'))
						{
							continue;
						}
					}
					m_NcalExpression.Parameters[key] = item.Value.result;
				}
			}
			object result = null;
			try
			{
				result = m_NcalExpression.Evaluate();
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		private void FieldFunction(string name, FunctionArgs args)
		{
			if (name == "GetFieldValue")
			{
				Dictionary<string, IVariable> iDataFields = GetIDataFields(args.Parameters[0].Evaluate());
				args.Result = iDataFields[args.Parameters[1].Evaluate().ToString()].result;
			}
		}

		private void OtherFunction(string name, FunctionArgs args)
		{
			if (!(name == "Random"))
			{
				return;
			}
			if (args.Parameters.Length == 1)
			{
				object obj = args.Parameters[0].Evaluate();
				List<IValue> list = obj as List<IValue>;
				IList list2;
				if (list != null)
				{
					list2 = new List<object>();
					for (int i = 0; i < list.Count; i++)
					{
						list2.Add(list[i].result);
					}
				}
				else
				{
					list2 = (obj as IList);
				}
				args.Result = list2.LRandom();
			}
			else if (args.Parameters.Length == 2)
			{
				object obj2 = args.Parameters[0].Evaluate();
				object obj3 = args.Parameters[1].Evaluate();
				int result = 0;
				args.Result = ((!int.TryParse(obj2.ToString(), out result)) ? UnityEngine.Random.Range((float)obj2, (float)obj3) : ((float)UnityEngine.Random.Range((int)obj2, (int)obj3)));
			}
		}

		private void ListFunction(string name, FunctionArgs args)
		{
			switch (name)
			{
			case "Find":
			{
				Expression[] parameters3 = args.Parameters;
				IList list9 = parameters3[0].Evaluate() as IList;
				if (list9 == null)
				{
					break;
				}
				if (parameters3.Length == 3)
				{
					object obj12 = parameters3[1].Evaluate();
					object obj13 = parameters3[2].Evaluate();
					object obj14 = list9[(int)obj12];
					Dictionary<string, IVariable> iDataFields4 = GetIDataFields(obj14);
					args.Result = iDataFields4[(string)obj13].result;
				}
				else
				{
					if (parameters3.Length < 4)
					{
						break;
					}
					List<string> list10 = new List<string>();
					List<object> list11 = new List<object>();
					for (int num2 = 1; num2 < parameters3.Length - 1; num2 += 2)
					{
						list10.Add((string)parameters3[num2].Evaluate());
						list11.Add(parameters3[num2 + 1].Evaluate());
					}
					for (int num3 = 0; num3 < list9.Count; num3++)
					{
						Dictionary<string, IVariable> iDataFields5 = GetIDataFields(list9[num3]);
						for (int num4 = 0; num4 < list10.Count; num4++)
						{
							object obj15 = list11[num4];
							object obj16 = Convert.ChangeType(iDataFields5[list10[num4]].result, obj15.GetType());
							if (!obj15.Equals(obj16))
							{
								break;
							}
							if (num4 == list10.Count - 1)
							{
								args.Result = iDataFields5[(string)parameters3[parameters3.Length - 1].Evaluate()].result;
								return;
							}
						}
					}
				}
				break;
			}
			case "Get":
			{
				Expression[] parameters = args.Parameters;
				IList list2 = parameters[0].Evaluate() as IList;
				if (list2 == null)
				{
					break;
				}
				if (parameters.Length == 2)
				{
					object obj3 = parameters[1].Evaluate();
					args.Result = list2[(int)obj3];
				}
				else
				{
					if (parameters.Length < 3)
					{
						break;
					}
					object obj4 = parameters[1].Evaluate();
					object obj5 = parameters[2].Evaluate();
					if (obj4 is int)
					{
						object obj6 = list2[(int)obj4];
						Dictionary<string, IVariable> iDataFields = GetIDataFields(obj6);
						args.Result = iDataFields[(string)obj5];
						break;
					}
					List<string> list3 = new List<string>();
					List<object> list4 = new List<object>();
					for (int i = 1; i < parameters.Length - 1; i += 2)
					{
						list3.Add((string)parameters[i].Evaluate());
						list4.Add(parameters[i + 1].Evaluate());
					}
					for (int j = 0; j < list2.Count; j++)
					{
						Dictionary<string, IVariable> iDataFields2 = GetIDataFields(list2[j]);
						for (int k = 0; k < list3.Count; k++)
						{
							object obj7 = list4[k];
							object obj8 = Convert.ChangeType(iDataFields2[list3[k]].result, obj7.GetType());
							if (!obj7.Equals(obj8))
							{
								break;
							}
							if (k == list3.Count - 1)
							{
								args.Result = iDataFields2[(string)parameters[parameters.Length - 1].Evaluate()];
								return;
							}
						}
					}
				}
				break;
			}
			case "Exists":
			{
				Expression[] parameters5 = args.Parameters;
				IList list15 = parameters5[0].Evaluate() as IList;
				if (list15 == null)
				{
					break;
				}
				if (parameters5.Length == 2)
				{
					args.Result = false;
					object obj19 = parameters5[1].Evaluate();
					int num9 = 0;
					while (true)
					{
						if (num9 < list15.Count)
						{
							object value2 = list15[num9];
							object obj20 = Convert.ChangeType(value2, obj19.GetType());
							if (obj20.Equals(obj19))
							{
								break;
							}
							num9++;
							continue;
						}
						return;
					}
					args.Result = true;
					break;
				}
				List<string> list16 = new List<string>();
				List<object> list17 = new List<object>();
				for (int num10 = 1; num10 < parameters5.Length; num10 += 2)
				{
					list16.Add((string)parameters5[num10].Evaluate());
					list17.Add(parameters5[num10 + 1].Evaluate());
				}
				args.Result = false;
				for (int num11 = 0; num11 < list15.Count; num11++)
				{
					Dictionary<string, IVariable> iDataFields7 = GetIDataFields(list15[num11]);
					for (int num12 = 0; num12 < list16.Count; num12++)
					{
						object obj21 = list17[num12];
						object obj22 = Convert.ChangeType(iDataFields7[list16[num12]].result, obj21.GetType());
						if (!obj21.Equals(obj22))
						{
							break;
						}
						if (num12 == list16.Count - 1)
						{
							args.Result = true;
							return;
						}
					}
				}
				break;
			}
			case "Where":
			{
				Expression[] parameters2 = args.Parameters;
				IList list5 = parameters2[0].Evaluate() as IList;
				if (list5 == null)
				{
					break;
				}
				List<string> list6 = new List<string>();
				List<object> list7 = new List<object>();
				for (int l = 1; l < parameters2.Length - 1; l += 2)
				{
					list6.Add((string)parameters2[l].Evaluate());
					list7.Add(parameters2[l + 1].Evaluate());
				}
				List<object> list8 = new List<object>();
				for (int m = 0; m < list5.Count; m++)
				{
					Dictionary<string, IVariable> iDataFields3 = GetIDataFields(list5[m]);
					for (int n = 0; n < list6.Count; n++)
					{
						object obj9 = list7[n];
						IVariable variable = iDataFields3[list6[n]];
						object obj10 = null;
						try
						{
							obj10 = variable.result;
						}
						catch (Exception)
						{
						}
						if (obj10 != null)
						{
							object obj11 = Convert.ChangeType(obj10, obj9.GetType());
							if (!obj9.Equals(obj11))
							{
								break;
							}
							if (n == list6.Count - 1)
							{
								list8.Add(list5[m]);
							}
						}
					}
				}
				args.Result = list8;
				break;
			}
			case "Count":
			{
				Expression[] parameters4 = args.Parameters;
				int num5 = 0;
				IList list12 = parameters4[0].Evaluate() as IList;
				if (list12 != null)
				{
					if (args.Parameters.Length > 1)
					{
						List<string> list13 = new List<string>();
						List<object> list14 = new List<object>();
						for (int num6 = 1; num6 < parameters4.Length - 1; num6 += 2)
						{
							list13.Add((string)parameters4[num6].Evaluate());
							list14.Add(parameters4[num6 + 1].Evaluate());
						}
						List<object> result = new List<object>();
						for (int num7 = 0; num7 < list12.Count; num7++)
						{
							Dictionary<string, IVariable> iDataFields6 = GetIDataFields(list12[num7]);
							for (int num8 = 0; num8 < list13.Count; num8++)
							{
								object obj17 = list14[num8];
								object obj18 = Convert.ChangeType(iDataFields6[list13[num8]].result, obj17.GetType());
								if (!obj17.Equals(obj18))
								{
									break;
								}
								if (num8 == list13.Count - 1)
								{
									num5++;
								}
							}
						}
						args.Result = result;
					}
					else
					{
						num5 = list12.Count;
					}
				}
				else
				{
					num5 = 0;
				}
				args.Result = num5;
				break;
			}
			case "Contains":
			{
				args.Result = false;
				IList list = args.Parameters[0].Evaluate() as IList;
				object value = args.Parameters[1].Evaluate();
				if (list == null)
				{
					break;
				}
				int num = 0;
				while (true)
				{
					if (num < list.Count)
					{
						object obj = list[num];
						object obj2 = Convert.ChangeType(value, obj.GetType());
						if (obj.Equals(obj2))
						{
							break;
						}
						num++;
						continue;
					}
					return;
				}
				args.Result = true;
				break;
			}
			}
		}

		private static Dictionary<string, IVariable> GetIDataFields(object obj)
		{
			Data data = obj as Data;
			if (data != null)
			{
				return data.fields;
			}
			return (obj as IData)?.fields;
		}

		public static void SetFormulaData(object variable, IData data)
		{
			if (variable == null)
			{
				return;
			}
			if (m_SetDataMi == null)
			{
				m_SetDataMi = typeof(Formula).GetMember("m_Data", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField).First();
			}
			Type type = variable.GetType();
			if (variable is Formula)
			{
				m_SetDataMi.SetMemberValue(variable, data);
			}
			IEnumerable<MemberInfo> allMembers = type.GetAllMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
			allMembers.For(delegate(MemberInfo m)
			{
				Type returnType = m.GetReturnType();
				if (returnType == typeof(IVariable))
				{
					SetFormulaData(m.GetMemberValue(variable), data);
				}
				else if (typeof(IList).IsAssignableFrom(returnType))
				{
					IList list = m.GetMemberValue(variable) as IList;
					if (list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							object obj = list[i];
							if (obj is Formula)
							{
								SetFormulaData(obj, data);
							}
						}
					}
				}
			});
		}
	}
}
