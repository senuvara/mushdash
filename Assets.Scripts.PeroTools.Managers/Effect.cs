using Assets.Scripts.PeroTools.Commons;
using DG.Tweening;
using DG.Tweening.Core;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class Effect
	{
		public float dieTime;

		public bool isAutoDestroy = true;

		private List<GameObject> m_GameObjects;

		private readonly Dictionary<GameObject, ParticleSystem[]> m_Pars = new Dictionary<GameObject, ParticleSystem[]>();

		private readonly Dictionary<GameObject, DOTweenAnimation[]> m_Tweens = new Dictionary<GameObject, DOTweenAnimation[]>();

		private readonly Dictionary<GameObject, Animator[]> m_Animators = new Dictionary<GameObject, Animator[]>();

		private readonly Dictionary<GameObject, UnityEngine.Animation[]> m_Animations = new Dictionary<GameObject, UnityEngine.Animation[]>();

		private readonly Dictionary<GameObject, SkeletonAnimation[]> m_SAnimations = new Dictionary<GameObject, SkeletonAnimation[]>();

		private readonly Dictionary<GameObject, Sequence> m_DestroySeqs = new Dictionary<GameObject, Sequence>();

		public string uid
		{
			get;
			private set;
		}

		public FastPool pool
		{
			get;
			private set;
		}

		public Effect(GameObject go, int preload, int capacity = -1, Transform parent = null)
		{
			uid = go.name;
			parent = (parent ?? Singleton<EffectManager>.instance.center.transform);
			pool = Singleton<PoolManager>.instance.MakeFastPool(go, preload, capacity, parent);
			Init();
		}

		public Effect(string u, int preload, int capacity = -1, Transform parent = null)
		{
			uid = u;
			parent = (parent ?? Singleton<EffectManager>.instance.center.transform);
			pool = Singleton<PoolManager>.instance.MakeFastPool(uid, preload, capacity, parent);
			Init();
		}

		public GameObject CreateInstance()
		{
			GameObject gameObject = pool.FastInstantiate(pool.sourcePrefab.transform.position, pool.sourcePrefab.transform.rotation, pool.parentTransform ?? Singleton<EffectManager>.instance.center.transform);
			Reset(gameObject);
			if (isAutoDestroy)
			{
				Sequence value = DOTweenUtils.Delay(delegate
				{
					Singleton<PoolManager>.instance.FastDestroy(gameObject);
				}, dieTime);
				m_DestroySeqs[gameObject] = value;
			}
			return gameObject;
		}

		public void Reset(GameObject go)
		{
			if (m_Pars.ContainsKey(go))
			{
				ParticleSystem[] array = m_Pars[go];
				array.For(delegate(ParticleSystem p)
				{
					p.time = 0f;
				});
			}
			if (m_Tweens.ContainsKey(go))
			{
				DOTweenAnimation[] array2 = m_Tweens[go];
				array2.For(delegate(DOTweenAnimation t)
				{
					t.tween.fullPosition = 0f;
				});
			}
			if (m_Animators.ContainsKey(go))
			{
				Animator[] array3 = m_Animators[go];
				array3.For(delegate(Animator a)
				{
					a.SetTime(0f);
				});
			}
			if (m_Animations.ContainsKey(go))
			{
				UnityEngine.Animation[] array4 = m_Animations[go];
				array4.For(delegate(UnityEngine.Animation a)
				{
					UnityEngine.AnimationState animationState = (UnityEngine.AnimationState)a.GetEnumerator().Current;
					if (animationState != null)
					{
						animationState.time = 0f;
					}
				});
			}
			if (!m_SAnimations.ContainsKey(go))
			{
				return;
			}
			SkeletonAnimation[] array5 = m_SAnimations[go];
			array5.For(delegate(SkeletonAnimation sa)
			{
				TrackEntry current = sa.state.GetCurrent(0);
				if (current != null)
				{
					current.TrackTime = 0f;
				}
			});
		}

		public void Pause()
		{
			m_DestroySeqs.Values.For(delegate(Sequence s)
			{
				s.Pause();
			});
			List<GameObject> array = new List<GameObject>(m_GameObjects);
			array.For(delegate(GameObject g)
			{
				if (!g)
				{
					m_GameObjects.Remove(g);
				}
				else
				{
					if (m_Pars.ContainsKey(g))
					{
						ParticleSystem[] array2 = m_Pars[g];
						array2.For(delegate(ParticleSystem p)
						{
							p.Pause(true);
						});
					}
					if (m_Tweens.ContainsKey(g))
					{
						DOTweenAnimation[] array3 = m_Tweens[g];
						array3.For(delegate(DOTweenAnimation t)
						{
							t.tween.Pause();
						});
					}
					if (m_Animators.ContainsKey(g))
					{
						Animator[] array4 = m_Animators[g];
						array4.For(delegate(Animator a)
						{
							a.enabled = false;
						});
					}
					if (m_Animations.ContainsKey(g))
					{
						UnityEngine.Animation[] array5 = m_Animations[g];
						array5.For(delegate(UnityEngine.Animation a)
						{
							a.enabled = false;
						});
					}
					if (m_SAnimations.ContainsKey(g))
					{
						SkeletonAnimation[] array6 = m_SAnimations[g];
						array6.For(delegate(SkeletonAnimation sa)
						{
							sa.enabled = false;
						});
					}
				}
			});
		}

		public void Resume()
		{
			for (int i = 0; i < m_GameObjects.Count; i++)
			{
				GameObject gameObject = m_GameObjects[i];
				if (!gameObject)
				{
					m_GameObjects.Remove(gameObject);
					return;
				}
				if (m_Pars.ContainsKey(gameObject))
				{
					ParticleSystem[] array = m_Pars[gameObject];
					array.For(delegate(ParticleSystem p)
					{
						p.Play(true);
					});
				}
				if (m_Tweens.ContainsKey(gameObject))
				{
					DOTweenAnimation[] array2 = m_Tweens[gameObject];
					array2.For(delegate(DOTweenAnimation t)
					{
						t.tween.Play();
					});
				}
				if (m_Animators.ContainsKey(gameObject))
				{
					Animator[] array3 = m_Animators[gameObject];
					array3.For(delegate(Animator a)
					{
						a.enabled = true;
					});
				}
				if (m_Animations.ContainsKey(gameObject))
				{
					UnityEngine.Animation[] array4 = m_Animations[gameObject];
					array4.For(delegate(UnityEngine.Animation a)
					{
						a.enabled = true;
					});
				}
				if (m_SAnimations.ContainsKey(gameObject))
				{
					SkeletonAnimation[] array5 = m_SAnimations[gameObject];
					array5.For(delegate(SkeletonAnimation sa)
					{
						sa.enabled = true;
					});
				}
			}
			m_DestroySeqs.Values.For(delegate(Sequence s)
			{
				s.Play();
			});
		}

		public void Clear()
		{
			m_DestroySeqs.Values.For(delegate(Sequence s)
			{
				s.Kill();
			});
			m_DestroySeqs.Clear();
			m_Tweens.Clear();
			m_Pars.Clear();
			m_Animators.Clear();
			m_Animations.Clear();
			m_SAnimations.Clear();
			m_GameObjects.Clear();
			pool.FastDestroyAll();
			pool.ClearCache();
			Singleton<PoolManager>.instance.DestroyFastPool(pool);
		}

		private void Init()
		{
			m_GameObjects = pool.cached;
			m_GameObjects.For(delegate(GameObject gameObject)
			{
				gameObject.SetActive(true);
				ParticleSystem[] array = gameObject.GetAllComponents<ParticleSystem>().ToArray();
				if (!m_Pars.ContainsKey(gameObject))
				{
					m_Pars.Add(gameObject, array);
				}
				DOTweenAnimation[] array2 = gameObject.GetAllComponents<DOTweenAnimation>().ToArray();
				if (!m_Tweens.ContainsKey(gameObject))
				{
					m_Tweens.Add(gameObject, array2);
				}
				array2.For(delegate(DOTweenAnimation t)
				{
					t.autoPlay = true;
					t.autoKill = false;
					DOTweenVisualManager orAddComponent = t.GetOrAddComponent<DOTweenVisualManager>();
					orAddComponent.onEnableBehaviour = OnEnableBehaviour.Restart;
					orAddComponent.onDisableBehaviour = OnDisableBehaviour.Pause;
				});
				Animator[] array3 = gameObject.GetAllComponents<Animator>().ToArray();
				if (!m_Animators.ContainsKey(gameObject))
				{
					m_Animators.Add(gameObject, array3);
				}
				UnityEngine.Animation[] array4 = gameObject.GetAllComponents<UnityEngine.Animation>().ToArray();
				if (!m_Animations.ContainsKey(gameObject))
				{
					m_Animations.Add(gameObject, array4);
				}
				SkeletonAnimation[] array5 = gameObject.GetAllComponents<SkeletonAnimation>().ToArray();
				if (!m_SAnimations.ContainsKey(gameObject))
				{
					m_SAnimations.Add(gameObject, array5);
				}
				if (Math.Abs(dieTime) <= 0f)
				{
					List<float> list = (from d in array2
						where d.loops != -1
						select d.duration).ToList();
					list.AddRange(array.Where((ParticleSystem p) => !p.main.loop).Select(delegate(ParticleSystem p)
					{
						switch (p.main.startLifetime.mode)
						{
						case ParticleSystemCurveMode.Constant:
							return p.main.startLifetime.constant;
						case ParticleSystemCurveMode.Curve:
							return p.main.startLifetime.curve.keys.Max((Keyframe k) => k.value);
						case ParticleSystemCurveMode.TwoCurves:
							return Mathf.Max(p.main.startLifetime.curveMax.keys.Max((Keyframe k) => k.value), p.main.startLifetime.curveMin.keys.Max((Keyframe k) => k.value));
						case ParticleSystemCurveMode.TwoConstants:
							return Mathf.Max(p.main.startLifetime.constantMax, p.main.startLifetime.constantMin);
						default:
							return p.main.duration;
						}
					}));
					list.AddRange(from a in array3
						where !a.GetCurrentAnimatorStateInfo(0).loop
						select a.GetCurrentAnimatorStateInfo(0).length);
					list.AddRange(from a in array4
						where !a.clip.isLooping
						select a.clip.length);
					list.AddRange(from sa in array5
						where !sa.loop
						select sa.skeletonDataAsset.GetSkeletonData(true).Animations.Find((Spine.Animation a) => a.Name == sa.AnimationName)?.Duration ?? 0f);
					if (list.Count > 0)
					{
						list.Sort(delegate(float l, float r)
						{
							float num = l - r;
							if (num > 0f)
							{
								return 1;
							}
							return (num < 0f) ? (-1) : 0;
						});
						dieTime = list[list.Count - 1];
					}
				}
				Reset(gameObject);
				gameObject.SetActive(false);
			});
		}
	}
}
