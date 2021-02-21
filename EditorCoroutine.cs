using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class EditorCoroutine
{
	private class Coroutine
	{
		public ICoroutineYield CurrentYield = new YieldDefault();

		public IEnumerator Routine;

		public string RoutineUniqueHash;

		public string OwnerUniqueHash;

		public string MethodName = string.Empty;

		public int OwnerHash;

		public string OwnerType;

		public bool Finished;

		public Coroutine(IEnumerator routine, int ownerHash, string ownerType)
		{
			Routine = routine;
			OwnerHash = ownerHash;
			OwnerType = ownerType;
			OwnerUniqueHash = ownerHash + "_" + ownerType;
			if (routine != null)
			{
				string[] array = routine.ToString().Split('<', '>');
				if (array.Length == 3)
				{
					MethodName = array[1];
				}
			}
			RoutineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
		}

		public Coroutine(string methodName, int ownerHash, string ownerType)
		{
			MethodName = methodName;
			OwnerHash = ownerHash;
			OwnerType = ownerType;
			OwnerUniqueHash = ownerHash + "_" + ownerType;
			RoutineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
		}
	}

	private abstract class ICoroutineYield
	{
		public abstract bool IsDone(float deltaTime);
	}

	private class YieldDefault : ICoroutineYield
	{
		public override bool IsDone(float deltaTime)
		{
			return true;
		}
	}

	private class YieldWaitForSeconds : ICoroutineYield
	{
		public float TimeLeft;

		public override bool IsDone(float deltaTime)
		{
			TimeLeft -= deltaTime;
			return TimeLeft < 0f;
		}
	}

	private class YieldWWW : ICoroutineYield
	{
		public WWW Www;

		public override bool IsDone(float deltaTime)
		{
			return Www.isDone;
		}
	}

	private class YieldAsync : ICoroutineYield
	{
		public AsyncOperation asyncOperation;

		public override bool IsDone(float deltaTime)
		{
			return asyncOperation.isDone;
		}
	}

	private static EditorCoroutine instance;

	private Dictionary<string, List<Coroutine>> coroutineDict = new Dictionary<string, List<Coroutine>>();

	private Dictionary<string, Dictionary<string, Coroutine>> coroutineOwnerDict = new Dictionary<string, Dictionary<string, Coroutine>>();

	private DateTime previousTimeSinceStartup;

	public static void StartCoroutine(IEnumerator routine, object thisReference)
	{
		CreateInstanceIfNeeded();
		instance.GoStartCoroutine(routine, thisReference);
	}

	public static void StartCoroutine(string methodName, object thisReference)
	{
		StartCoroutine(methodName, null, thisReference);
	}

	public static void StartCoroutine(string methodName, object value, object thisReference)
	{
		MethodInfo method = thisReference.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			Debug.LogError("Coroutine '" + methodName + "' couldn't be started, the method doesn't exist!");
			return;
		}
		object obj = (value != null) ? method.Invoke(thisReference, new object[1]
		{
			value
		}) : method.Invoke(thisReference, null);
		if (obj is IEnumerator)
		{
			CreateInstanceIfNeeded();
			instance.GoStartCoroutine((IEnumerator)obj, thisReference);
		}
		else
		{
			Debug.LogError("Coroutine '" + methodName + "' couldn't be started, the method doesn't return an IEnumerator!");
		}
	}

	public static void StopCoroutine(IEnumerator routine, object thisReference)
	{
		CreateInstanceIfNeeded();
		instance.GoStopCoroutine(routine, thisReference);
	}

	public static void StopCoroutine(string methodName, object thisReference)
	{
		CreateInstanceIfNeeded();
		instance.GoStopCoroutine(methodName, thisReference);
	}

	public static void StopAllCoroutines(object thisReference)
	{
		CreateInstanceIfNeeded();
		instance.GoStopAllCoroutines(thisReference);
	}

	private static void CreateInstanceIfNeeded()
	{
		if (instance == null)
		{
			instance = new EditorCoroutine();
			instance.Initialize();
		}
	}

	private void Initialize()
	{
		previousTimeSinceStartup = DateTime.Now;
	}

	private void GoStopCoroutine(IEnumerator routine, object thisReference)
	{
		GoStopActualRoutine(CreateCoroutine(routine, thisReference));
	}

	private void GoStopCoroutine(string methodName, object thisReference)
	{
		GoStopActualRoutine(CreateCoroutineFromString(methodName, thisReference));
	}

	private void GoStopActualRoutine(Coroutine routine)
	{
		if (coroutineDict.ContainsKey(routine.RoutineUniqueHash))
		{
			coroutineOwnerDict[routine.OwnerUniqueHash].Remove(routine.RoutineUniqueHash);
			coroutineDict.Remove(routine.RoutineUniqueHash);
		}
	}

	private void GoStopAllCoroutines(object thisReference)
	{
		Coroutine coroutine = CreateCoroutine(null, thisReference);
		if (!coroutineOwnerDict.ContainsKey(coroutine.OwnerUniqueHash))
		{
			return;
		}
		foreach (KeyValuePair<string, Coroutine> item in coroutineOwnerDict[coroutine.OwnerUniqueHash])
		{
			coroutineDict.Remove(item.Value.RoutineUniqueHash);
		}
		coroutineOwnerDict.Remove(coroutine.OwnerUniqueHash);
	}

	private void GoStartCoroutine(IEnumerator routine, object thisReference)
	{
		if (routine == null)
		{
			Debug.LogException(new Exception("IEnumerator is null!"), null);
		}
		Coroutine coroutine = CreateCoroutine(routine, thisReference);
		GoStartCoroutine(coroutine);
	}

	private void GoStartCoroutine(Coroutine coroutine)
	{
		if (!coroutineDict.ContainsKey(coroutine.RoutineUniqueHash))
		{
			List<Coroutine> value = new List<Coroutine>();
			coroutineDict.Add(coroutine.RoutineUniqueHash, value);
		}
		coroutineDict[coroutine.RoutineUniqueHash].Add(coroutine);
		if (!coroutineOwnerDict.ContainsKey(coroutine.OwnerUniqueHash))
		{
			Dictionary<string, Coroutine> value2 = new Dictionary<string, Coroutine>();
			coroutineOwnerDict.Add(coroutine.OwnerUniqueHash, value2);
		}
		if (!coroutineOwnerDict[coroutine.OwnerUniqueHash].ContainsKey(coroutine.RoutineUniqueHash))
		{
			coroutineOwnerDict[coroutine.OwnerUniqueHash].Add(coroutine.RoutineUniqueHash, coroutine);
		}
		MoveNext(coroutine);
	}

	private Coroutine CreateCoroutine(IEnumerator routine, object thisReference)
	{
		return new Coroutine(routine, thisReference.GetHashCode(), thisReference.GetType().ToString());
	}

	private Coroutine CreateCoroutineFromString(string methodName, object thisReference)
	{
		return new Coroutine(methodName, thisReference.GetHashCode(), thisReference.GetType().ToString());
	}

	private void OnUpdate()
	{
		float deltaTime = (float)(DateTime.Now.Subtract(previousTimeSinceStartup).TotalMilliseconds / 1000.0);
		previousTimeSinceStartup = DateTime.Now;
		if (coroutineDict.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < coroutineDict.Count; i++)
		{
			KeyValuePair<string, List<Coroutine>> keyValuePair = coroutineDict.ToList()[i];
			List<Coroutine> value = keyValuePair.Value;
			for (int num = value.Count - 1; num >= 0; num--)
			{
				Coroutine coroutine = value[num];
				if (coroutine.CurrentYield.IsDone(deltaTime) && !MoveNext(coroutine))
				{
					value.RemoveAt(num);
					coroutine.CurrentYield = null;
					coroutine.Finished = true;
				}
			}
			if (value.Count == 0)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			coroutineDict.Remove(list[j]);
		}
	}

	private bool MoveNext(Coroutine coroutine)
	{
		if (coroutine.Routine.MoveNext())
		{
			return Process(coroutine);
		}
		return false;
	}

	private bool Process(Coroutine coroutine)
	{
		object current = coroutine.Routine.Current;
		if (current == null)
		{
			return false;
		}
		if (current is WaitForSeconds)
		{
			float num = float.Parse(GetInstanceField(typeof(WaitForSeconds), current, "m_Seconds").ToString());
			coroutine.CurrentYield = new YieldWaitForSeconds
			{
				TimeLeft = num
			};
		}
		else if (current is WWW)
		{
			coroutine.CurrentYield = new YieldWWW
			{
				Www = (WWW)current
			};
		}
		else if (current is WaitForFixedUpdate)
		{
			coroutine.CurrentYield = new YieldDefault();
		}
		else if (current is AsyncOperation)
		{
			coroutine.CurrentYield = new YieldAsync
			{
				asyncOperation = (AsyncOperation)current
			};
		}
		else
		{
			coroutine.CurrentYield = new YieldDefault();
		}
		return true;
	}

	private static object GetInstanceField(Type type, object instance, string fieldName)
	{
		BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		FieldInfo field = type.GetField(fieldName, bindingAttr);
		return field.GetValue(instance);
	}

	private static void Test()
	{
		Type typeFromHandle = typeof(UnityEngine.Coroutine);
		MemberInfo[] members = typeFromHandle.GetMembers();
		MemberInfo[] array = members;
		foreach (MemberInfo value in array)
		{
			Console.WriteLine(value);
		}
	}
}
