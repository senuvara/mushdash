using Assets.Scripts.PeroTools.AssetBundles;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class GameUtils
	{
		public static string GetPathInScene(this Transform transform)
		{
			string text = string.Empty;
			while ((bool)transform)
			{
				text = transform.gameObject.name + "/" + text;
				transform = transform.parent;
			}
			return text;
		}

		public static T GetOrAddComponent<T>(this Component cmp) where T : Component
		{
			GameObject gameObject = cmp.gameObject;
			return gameObject.GetOrAddComponent<T>();
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T val = gameObject.GetComponent<T>();
			if (!(UnityEngine.Object)val)
			{
				val = gameObject.AddComponent<T>();
			}
			return val;
		}

		public static Component GetOrAddComponent(this GameObject gameObject, Type type)
		{
			Component component = gameObject.GetComponent(type);
			if (!component)
			{
				component = gameObject.AddComponent(type);
			}
			return component;
		}

		public static List<Component> EnableAllComponents(this GameObject go, bool enabled = true, params Type[] types)
		{
			Component[] componentsInChildren = go.GetComponentsInChildren<Component>();
			componentsInChildren.AddRange(go.GetComponents<Component>());
			return EnableAllComponents(componentsInChildren.ToArray(), enabled, types);
		}

		public static List<Component> EnableAllComponents(Component[] cs, bool enabled = true, params Type[] types)
		{
			List<Component> components = new List<Component>();
			cs.For(delegate(Component c)
			{
				if ((bool)c)
				{
					Type type = c.GetType();
					if (!types.Contains(type))
					{
						PropertyInfo property = type.GetProperty("enabled");
						if (property != null)
						{
							components.Add(c);
							property.SetValue(c, enabled, null);
						}
					}
				}
			});
			return components;
		}

		public static void SetVisible(this GameObject go, bool isVisisble)
		{
			List<Renderer> array = FindObjectsOfType<Renderer>(go.transform);
			array.For(delegate(Renderer r)
			{
				r.enabled = isVisisble;
			});
			List<SpriteRenderer> array2 = FindObjectsOfType<SpriteRenderer>(go.transform);
			array2.For(delegate(SpriteRenderer s)
			{
				s.enabled = isVisisble;
			});
			List<Graphic> array3 = FindObjectsOfType<Graphic>(go.transform);
			array3.For(delegate(Graphic s)
			{
				s.enabled = isVisisble;
			});
		}

		public static void Enable(this GameObject gameObject, bool isTo = true, bool includeInactive = true, params Type[] components)
		{
			components.For(delegate(Type c)
			{
				gameObject.GetComponentsInChildren(c, includeInactive).For(delegate(Component cpn)
				{
					Type type = cpn.GetType();
					type.GetProperty("enabled")?.SetValue(cpn, isTo, null);
				});
			});
		}

		public static List<T> GetAllComponents<T>(this GameObject go, bool inactive = true) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			T component = go.GetComponent<T>();
			if ((bool)(UnityEngine.Object)component)
			{
				list.Add(component);
			}
			T[] componentsInChildren = go.GetComponentsInChildren<T>(inactive);
			list.AddRange(componentsInChildren);
			return list;
		}

		public static List<T> FindObjectsOfType<T>(Transform parent = null, bool includeInactive = true) where T : UnityEngine.Object
		{
			List<T> list = new List<T>();
			List<Transform> list2;
			if (parent == null)
			{
				list2 = (from t in UnityEngine.Object.FindObjectsOfType<Transform>()
					where t.parent == null
					select t).ToList();
			}
			else
			{
				List<Transform> list3 = new List<Transform>();
				list3.Add(parent);
				list2 = list3;
			}
			List<Transform> array = list2;
			array.For(delegate(Transform t)
			{
				T[] componentsInChildren = t.GetComponentsInChildren<T>(includeInactive);
				T[] components = t.GetComponents<T>();
				list.AddRange(components);
				componentsInChildren.For(delegate(T c)
				{
					if (!list.Contains(c))
					{
						list.Add(c);
					}
				});
			});
			return list;
		}

		public static T FindObjectOfType<T>(Transform parent = null, bool includeInactive = true) where T : UnityEngine.Object
		{
			List<T> list = FindObjectsOfType<T>(parent, includeInactive);
			return (list.Count <= 0) ? ((T)null) : list.First();
		}

		public static List<GameObject> FindGameObjectsWithTag(string tag)
		{
			List<GameObject> list = new List<GameObject>();
			List<Transform> array = (from t in UnityEngine.Object.FindObjectsOfType<Transform>()
				where t.parent == null
				select t).ToList();
			array.For(delegate(Transform transform)
			{
				list.AddRange(from t in transform.GetComponentsInChildren<Transform>(true)
					where t.gameObject.tag == tag
					select t.gameObject);
			});
			return list;
		}

		public static T GetObject<T>(object obj) where T : UnityEngine.Object
		{
			if (obj == null || obj.ToString() == "null")
			{
				return (T)null;
			}
			T val = obj as T;
			if ((bool)(UnityEngine.Object)val)
			{
				return val;
			}
			UnityEngine.Object @object = obj as UnityEngine.Object;
			if ((bool)@object)
			{
				return @object.GetObject<T>();
			}
			return Singleton<AssetBundleManager>.instance.LoadFromName<T>(obj.ToString());
		}

		public static Sprite CreateSpriteFromTexture(Texture2D texture)
		{
			return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}

		public static T GetObject<T>(this UnityEngine.Object obj) where T : UnityEngine.Object
		{
			T val = obj as T;
			if ((UnityEngine.Object)val != (UnityEngine.Object)null)
			{
				return val;
			}
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				return gameObject.GetComponent<T>();
			}
			Component component = obj as Component;
			if (component != null)
			{
				if (typeof(T) == typeof(GameObject))
				{
					return component.gameObject as T;
				}
				return component.gameObject.GetComponent<T>();
			}
			return (T)null;
		}

		public static UnityEngine.Object GetObject(this UnityEngine.Object obj, string type)
		{
			GameObject gameObject = obj as GameObject;
			if (gameObject != null)
			{
				return gameObject.GetComponent(type);
			}
			Component component = obj as Component;
			if (component != null)
			{
				return component.gameObject.GetComponent(type);
			}
			return null;
		}

		public static void SortChildren(this Transform t, Comparison<Transform> comparison, bool includeInActive = false)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < t.childCount; i++)
			{
				if (t.GetChild(i).gameObject.activeSelf || includeInActive)
				{
					list.Add(t.GetChild(i));
				}
			}
			list.Sort(comparison);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].SetSiblingIndex(j);
			}
		}

		public static bool TryParse(string s, out Vector2 res)
		{
			string[] array = s.Replace("(", string.Empty).Replace(")", string.Empty).Split(',');
			if (array.Length < 2)
			{
				res = Vector2.zero;
				return false;
			}
			float result;
			float result2;
			if (!float.TryParse(array[0], out result) || !float.TryParse(array[1], out result2))
			{
				res = Vector2.zero;
				return false;
			}
			res = new Vector2(result, result2);
			return true;
		}

		public static bool TryParse(string s, out Vector3 res)
		{
			string[] array = s.Replace("(", string.Empty).Replace(")", string.Empty).Split(',');
			if (array.Length < 3)
			{
				res = Vector3.zero;
				return false;
			}
			float result;
			float result2;
			float result3;
			if (!float.TryParse(array[0], out result) || !float.TryParse(array[1], out result2) || !float.TryParse(array[2], out result3))
			{
				res = Vector3.zero;
				return false;
			}
			res = new Vector3(result, result2, result3);
			return true;
		}

		public static float GetTime(this Animator animator)
		{
			if (!animator.gameObject.activeInHierarchy)
			{
				return 0f;
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			float length = currentAnimatorStateInfo.length;
			if (length > 0f)
			{
				return currentAnimatorStateInfo.normalizedTime % 1f * length;
			}
			return 0f;
		}

		public static void SetTime(this Animator animator, float time)
		{
			if (animator.gameObject.activeInHierarchy)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				float length = currentAnimatorStateInfo.length;
				if (length > 0f && animator.gameObject.activeSelf)
				{
					animator.Play(currentAnimatorStateInfo.fullPathHash, 0, time / length);
				}
			}
		}

		public static bool IsComplete(this Animator animator)
		{
			if (!animator.gameObject.activeInHierarchy)
			{
				return true;
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.loop)
			{
				return true;
			}
			return currentAnimatorStateInfo.normalizedTime >= 0.99f;
		}

		public static string GetCurrentAnimName(this Animator animator)
		{
			if (!animator.gameObject.activeInHierarchy)
			{
				return string.Empty;
			}
			AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
			AnimationClip animationClip = animator.runtimeAnimatorController.animationClips.Find((AnimationClip a) => Animator.StringToHash(a.name) == state.shortNameHash);
			if (animationClip != null)
			{
				return animationClip.name;
			}
			return string.Empty;
		}
	}
}
