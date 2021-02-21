using Assets.Scripts.PeroTools.Nice.Variables;
using UnityEngine;
using UnityEngine.UI;

public class AlbumDifficultyCell : MonoBehaviour
{
	private VariableBehaviour m_VariableBehaviour;

	public Button btn
	{
		get;
		private set;
	}

	public string uid
	{
		get;
		private set;
	}

	public int GetDataIndex()
	{
		return (int)m_VariableBehaviour.result;
	}

	private void Awake()
	{
		m_VariableBehaviour = GetComponent<VariableBehaviour>();
		btn = GetComponent<Button>();
		btn.onClick.AddListener(delegate
		{
		});
	}
}
