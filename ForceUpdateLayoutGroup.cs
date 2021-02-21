using UnityEngine;
using UnityEngine.UI;

public class ForceUpdateLayoutGroup : MonoBehaviour
{
	private LayoutGroup m_LayoutGroup;

	private void Awake()
	{
		m_LayoutGroup = GetComponent<LayoutGroup>();
	}

	private void Update()
	{
		m_LayoutGroup.enabled = false;
		m_LayoutGroup.enabled = true;
	}
}
