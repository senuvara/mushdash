using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DYUnityLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectController : SingletonMonoBehaviour<SceneObjectController>
{
	private int idx;

	private int timeEventIndex;

	private float runtime;

	private StageEvent eventData;

	private Hashtable sceneObjectPool;

	private AudioSource sceneAudio;

	private List<GameObject> dymObjectList;

	public Hashtable SceneObjectPool => sceneObjectPool;

	public void TimerStepTrigger(object sender, uint triggerId, params object[] args)
	{
		if (FixUpdateTimer.IsPausing() || runtime < 0f || timeEventIndex >= eventData.timeEvents.Length)
		{
			return;
		}
		runtime = (float)(int)((decimal)args[0] * 100m) * 0.01f;
		StageTimeEvent stageTimeEvent = eventData.timeEvents[timeEventIndex];
		Debug.Log("tEvent " + timeEventIndex + " time is " + stageTimeEvent.time + " real time is " + runtime);
		timeEventIndex++;
		for (int i = 0; i < stageTimeEvent.eventItems.Length; i++)
		{
			StageTimeEventItem stageTimeEventItem = stageTimeEvent.eventItems[i];
			if (stageTimeEventItem.sceneObject == null)
			{
				break;
			}
			string text = EditorData.Instance.SpineActionKeys[stageTimeEventItem.actionIndex];
			if (text != null)
			{
				GameObject gameObject = CreateObj(stageTimeEventItem.sceneObject);
				gameObject.SetActive(true);
				SpineActionController component = gameObject.GetComponent<SpineActionController>();
				if (component != null)
				{
					component.OnControllerStart();
				}
				SpineActionController.Play(text, gameObject);
				ResetParticle(gameObject);
			}
		}
	}

	public void InitController(int idx, string audioLayerName = "BossLayer")
	{
		this.idx = idx;
		Debug.Log("Use scene event config " + this.idx);
		runtime = -1f;
		timeEventIndex = 0;
		sceneObjectPool = new Hashtable();
		eventData = EditorData.Instance.GetStageEventDataById(this.idx);
		sceneAudio = GameObject.Find(audioLayerName).GetComponent<AudioSource>();
		PreLoad();
	}

	public void Run()
	{
		if (eventData.timeEvents != null && eventData.timeEvents.Length != 0)
		{
			runtime = 0f;
		}
	}

	public void PreLoad()
	{
	}

	public void DoObjectComeoutEvent(string nodeId)
	{
		if (eventData.actionEvents == null || eventData.actionEvents.Length == 0)
		{
			return;
		}
		for (int i = 0; i < eventData.actionEvents.Length; i++)
		{
			StageActionEvent stageActionEvent = eventData.actionEvents[i];
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("notedata", "id", "uid", stageActionEvent.nodeIndex);
			if (!(configStringValue != nodeId) && !(stageActionEvent.sceneObject == null))
			{
				string actionKey = EditorData.Instance.SpineActionKeys[stageActionEvent.bornActionIndex];
				GameObject gameObject = CreateObj(stageActionEvent.sceneObject);
				gameObject.SetActive(true);
				SpineActionController.Play(actionKey, gameObject);
				ResetParticle(gameObject);
			}
		}
	}

	public void DoObjectOnAttackedEvent(string nodeId)
	{
		if (eventData.actionEvents == null || eventData.actionEvents.Length == 0)
		{
			return;
		}
		for (int i = 0; i < eventData.actionEvents.Length; i++)
		{
			StageActionEvent stageActionEvent = eventData.actionEvents[i];
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("notedata", "id", "uid", stageActionEvent.nodeIndex);
			if (!(configStringValue != nodeId) && !(stageActionEvent.sceneObject == null))
			{
				string actionKey = EditorData.Instance.SpineActionKeys[stageActionEvent.hittedActionIndex];
				GameObject gameObject = CreateObj(stageActionEvent.sceneObject);
				gameObject.SetActive(true);
				SpineActionController.Play(actionKey, gameObject);
				ResetParticle(gameObject);
			}
		}
	}

	public void DoObjectOnMissedEvent(string nodeId)
	{
		if (eventData.actionEvents == null || eventData.actionEvents.Length == 0)
		{
			return;
		}
		for (int i = 0; i < eventData.actionEvents.Length; i++)
		{
			StageActionEvent stageActionEvent = eventData.actionEvents[i];
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("notedata", "id", "uid", stageActionEvent.nodeIndex);
			if (!(configStringValue != nodeId) && !(stageActionEvent.sceneObject == null))
			{
				string actionKey = EditorData.Instance.SpineActionKeys[stageActionEvent.missActionIndex];
				GameObject gameObject = CreateObj(stageActionEvent.sceneObject);
				gameObject.SetActive(true);
				SpineActionController.Play(actionKey, gameObject);
				ResetParticle(gameObject);
			}
		}
	}

	public float GetRunTime()
	{
		return runtime;
	}

	public AudioSource GetAudioSource()
	{
		return sceneAudio;
	}

	public List<GameObject> GetAllSceneEventObjects()
	{
		return dymObjectList;
	}

	public void ActiveObject(int idx)
	{
		if (dymObjectList != null && dymObjectList.Count >= idx)
		{
			GameObject gameObject = dymObjectList[idx];
			if (!(gameObject == null))
			{
				gameObject.SetActive(true);
			}
		}
	}

	private GameObject CreateObj(GameObject sourceObject)
	{
		return null;
	}

	private void ResetParticle(GameObject sceneObject)
	{
		if (!(sceneObject == null))
		{
			ParticleSystem component = sceneObject.GetComponent<ParticleSystem>();
			if (!(component == null))
			{
				component.Stop();
				component.Clear();
				component.Play();
			}
		}
	}
}
