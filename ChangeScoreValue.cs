using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScoreValue : MonoBehaviour
{
	public Text text;

	public TextMeshProUGUI textMeshProUGUI;

	private EventManager m_EventManager;

	private void Awake()
	{
		Singleton<EventManager>.instance.RegEvent("Battle/OnScoreChanged").trigger += OnScoreChange;
		m_EventManager = Singleton<EventManager>.instance;
	}

	private void OnDestroy()
	{
		if (m_EventManager != null)
		{
			m_EventManager.RegEvent("Battle/OnScoreChanged").trigger -= OnScoreChange;
		}
	}

	private void OnScoreChange(object sender, object reciever, object[] args)
	{
		if (Singleton<BattleProperty>.instance.isGCScene)
		{
			textMeshProUGUI.text = string.Concat(Mathf.RoundToInt(Singleton<TaskStageTarget>.instance.GetScore()));
		}
		else
		{
			text.text = string.Concat(Mathf.RoundToInt(Singleton<TaskStageTarget>.instance.GetScore()));
		}
	}
}
