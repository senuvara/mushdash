using UnityEngine;
using UnityEngine.UI;

public class ControllerCell : MonoBehaviour
{
	private GameObject m_Cover;

	private GameObject m_Reverse;

	private Image m_Selected;

	private void Awake()
	{
		m_Cover = base.transform.Find("ImgCover").gameObject;
		m_Reverse = base.transform.Find("ImgCoverReverse").gameObject;
		m_Selected = base.transform.Find("ImgBaseSelected").GetComponent<Image>();
	}

	public void SetSelectColor(Color c)
	{
		if (m_Selected == null)
		{
			Debug.LogError("Didn`t find image on " + base.gameObject.name);
		}
		else
		{
			m_Selected.color = c;
		}
	}

	public void SetReverse(bool isReverse)
	{
		m_Reverse.SetActive(isReverse);
		m_Cover.SetActive(!isReverse);
	}
}
