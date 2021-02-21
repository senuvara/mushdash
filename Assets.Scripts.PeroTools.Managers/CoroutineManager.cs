using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class CoroutineManager : SingletonMonoBehaviour<CoroutineManager>
	{
		[HideInInspector]
		public bool isCoroutineActive = true;

		public Coroutine Delay(Action callback, float seconds)
		{
			if (seconds <= 0f)
			{
				callback();
				return null;
			}
			if (Application.isPlaying)
			{
				return StartCoroutine(CoroutineSecondsEnumerator(callback, seconds));
			}
			return null;
		}

		public Coroutine Delay(Action callback, decimal seconds)
		{
			seconds = ((!(seconds < 0.01m)) ? seconds : 0m);
			return Delay(callback, (int)decimal.Round(seconds / 0.01m, 0), true);
		}

		public Coroutine Delay(Action callback, int frames, bool isFixed = false)
		{
			if (frames <= 0)
			{
				callback();
				return null;
			}
			if (isFixed && Application.isPlaying)
			{
				return StartCoroutine(CoroutineFixedFramesEnumerator(callback, frames));
			}
			if (Application.isPlaying)
			{
				return StartCoroutine(CoroutineFramesEnumerator(callback, frames));
			}
			return null;
		}

		public Coroutine EndFrameAction(Action callback)
		{
			if (Application.isPlaying)
			{
				return StartCoroutine(CoroutineEndOfFrame(callback));
			}
			return null;
		}

		public Coroutine StartCoroutine(Action callback, Func<bool> boolFunc, float failTime = -1f, Action failCallback = null)
		{
			if (Application.isPlaying)
			{
				bool isDone = false;
				Coroutine couroutine = StartCoroutine(CoroutineUtilEnumerator(delegate
				{
					isDone = true;
					callback();
				}, boolFunc));
				if (failTime > 0f)
				{
					Delay(delegate
					{
						if (!isDone)
						{
							if (couroutine != null)
							{
								StopCoroutine(couroutine);
							}
							if (failCallback != null)
							{
								failCallback();
							}
						}
					}, failTime);
				}
				return couroutine;
			}
			return null;
		}

		public Coroutine StartCoroutine(Func<UnityEngine.Object> callback)
		{
			if (Application.isPlaying)
			{
				return StartCoroutine(CoroutineObjectEnumerator(callback));
			}
			return null;
		}

		private IEnumerator CoroutineUtilEnumerator(Action callback, Func<bool> boolFunc)
		{
			while (!isCoroutineActive)
			{
				yield return null;
			}
			yield return new WaitUntil(boolFunc);
			callback();
		}

		private IEnumerator CoroutineEndOfFrame(Action callback)
		{
			while (!isCoroutineActive)
			{
				yield return null;
			}
			yield return new WaitForEndOfFrame();
			callback();
		}

		private IEnumerator CoroutineFramesEnumerator(Action callback, int frames)
		{
			for (int i = 0; i < frames; i++)
			{
				if (!isCoroutineActive)
				{
					i--;
					yield return null;
				}
				yield return new WaitForEndOfFrame();
			}
			callback();
		}

		private IEnumerator CoroutineFixedFramesEnumerator(Action callback, int frames)
		{
			for (int i = 0; i < frames; i++)
			{
				if (!isCoroutineActive)
				{
					i--;
					yield return null;
				}
				yield return new WaitForFixedUpdate();
			}
			callback();
		}

		private IEnumerator CoroutineObjectEnumerator(Func<UnityEngine.Object> callback)
		{
			while (!isCoroutineActive)
			{
				yield return null;
			}
			yield return callback();
		}

		private IEnumerator CoroutineSecondsEnumerator(Action callback, float seconds)
		{
			float time = 0f;
			while (time < seconds)
			{
				if (isCoroutineActive)
				{
					time += Time.deltaTime;
				}
				yield return null;
			}
			callback();
		}

		private void Init()
		{
			isCoroutineActive = true;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			base.gameObject.hideFlags = HideFlags.HideInHierarchy;
		}
	}
}
