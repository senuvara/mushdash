using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using UnityEngine;

public class Artist2BtnSpecial : MonoBehaviour
{
	public VariableBehaviour m_Result;

	public GameObject btn1TipsImg;

	private GameObject m_Parent;

	private void Awake()
	{
		m_Parent = base.transform.parent.gameObject;
	}

	public void OnRefresh()
	{
		if (m_Result.GetResult<string>() == "steam://advertise/1049100")
		{
			m_Parent.SetActive(true);
		}
	}
}
