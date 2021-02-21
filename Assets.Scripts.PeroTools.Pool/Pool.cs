using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Pool
{
	public class Pool<T>
	{
		private readonly Stack<T> _caches;

		private readonly Func<T> _newFunc;

		private readonly Action<T> _releaseAction;

		private static Pool<T> _pools;

		public Pool(Func<T> newFunc, Action<T> releaseAction)
		{
			_caches = new Stack<T>();
			_newFunc = newFunc;
			_releaseAction = releaseAction;
		}

		public static T Get(Func<T> newFunc, Action<T> releaseAction)
		{
			if (_pools == null)
			{
				_pools = new Pool<T>(newFunc, releaseAction);
			}
			return _pools.Pop();
		}

		public static void Release(T t)
		{
			if (_pools != null)
			{
				_pools.Push(t);
			}
		}

		public T Pop()
		{
			if (_caches.Count == 0)
			{
				return _newFunc();
			}
			return _caches.Pop();
		}

		public void Push(T t)
		{
			_releaseAction(t);
			_caches.Push(t);
		}
	}
}
