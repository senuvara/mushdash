using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Controls
{
	public class Chip : MonoBehaviour
	{
		public Transform character;

		public Transform elfin;

		public Transform trove;

		public Transform parent;

		public GameObject image;

		public particleHoming particleHoming;

		private ParticleSystem.Particle m_Particle;

		private GameObject m_GameObject;

		private static int m_DoneCount;

		private void OnDisable()
		{
			m_DoneCount = 0;
			List<IData> items = Singleton<DataManager>.instance["Account"]["RewardItems"].GetResult<List<IData>>();
			int siblingIndex = base.transform.GetSiblingIndex();
			if (siblingIndex >= items.Count)
			{
				return;
			}
			IData data = items[siblingIndex];
			string result = data["type"].GetResult<string>();
			Transform target = (result == "character") ? character : ((!(result == "elfin")) ? trove : elfin);
			particleHoming.gameObject.SetActive(false);
			particleHoming.target = target;
			particleHoming.transform.position = base.transform.position;
			particleHoming.gameObject.SetActive(true);
			m_GameObject = Object.Instantiate(image, base.transform.position, Quaternion.identity, parent);
			Canvas canvas = m_GameObject.AddComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.sortingLayerName = "UI";
			canvas.sortingOrder = 7;
			string uid = string.Empty;
			ParticleSystem ps = particleHoming.GetComponent<ParticleSystem>();
			Singleton<EventManager>.instance.Invoke("UI/DisableTouch");
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				uid = SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(delegate
				{
					if ((bool)m_GameObject)
					{
						ParticleSystem.Particle[] array = new ParticleSystem.Particle[1];
						ps.GetParticles(array);
						m_Particle = array.First();
						if (m_Particle.remainingLifetime > 0f)
						{
							m_GameObject.transform.position = m_Particle.position;
						}
						else
						{
							Singleton<AudioManager>.instance.PlayOneShot("sfx_crystal", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
							Object.Destroy(m_GameObject);
						}
					}
					else
					{
						SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop(uid);
						m_DoneCount++;
						if (m_DoneCount == items.Count || (items.Count > 3 && m_DoneCount == 2))
						{
							m_DoneCount = 0;
							Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
							Singleton<EventManager>.instance.Invoke("Battle/OnReward");
						}
					}
				}, UnityGameManager.LoopType.Update).uid;
			}, () => ps.particleCount > 0);
		}
	}
}
