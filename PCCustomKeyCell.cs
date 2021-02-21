using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCCustomKeyCell : MonoBehaviour
{
	public bool dontFadeBaseImg;

	public Image objImg;

	public Image imgSpecialButton;

	public Text txtKey;

	public Text txtKeySetting;

	public Text txtKeyNo;

	public Image imgSelected;

	private PnlInputPc.keyType m_BtnType;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClickToSendInfo);
		CheckKeyType();
	}

	public void OnClickToSendInfo()
	{
		PnlInputPc.Instance().customizeSelect.defaultSelect = base.gameObject;
		PnlInputPc.Instance().OnClickBtnCustom();
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			PnlInputPc.Instance().SetCurrentCustomKey(base.gameObject);
			SetKeyToSetting();
		}, 0.1f);
	}

	public PnlInputPc.keyType GetBtnType()
	{
		return m_BtnType;
	}

	public void SetKeyToKey(string keyName)
	{
		CheckActive(txtKey.gameObject);
		m_BtnType = PnlInputPc.keyType.NormalKey;
		objImg.DOFade(1f, 0f);
		txtKey.text = keyName;
	}

	public void SetKeyToSetting()
	{
		CheckActive(txtKeySetting.gameObject);
		m_BtnType = PnlInputPc.keyType.SettingKey;
		objImg.DOFade(0f, 0f);
	}

	public void SetKeyToNo()
	{
		CheckActive(txtKeyNo.gameObject);
		m_BtnType = PnlInputPc.keyType.NoKey;
		objImg.DOFade(0f, 0f);
	}

	public void SetKeyToSpecial(string keyName)
	{
		CheckActive(imgSpecialButton.gameObject);
		m_BtnType = PnlInputPc.keyType.SpecialKey;
		objImg.DOFade(1f, 0f);
		imgSpecialButton.sprite = PnlInputPc.Instance().specialBtnImgDict[keyName];
	}

	private void CheckActive(GameObject showObj)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(txtKey.gameObject);
		list.Add(txtKeyNo.gameObject);
		list.Add(txtKeySetting.gameObject);
		list.Add(imgSpecialButton.gameObject);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == showObj)
			{
				list[i].SetActive(true);
			}
			else
			{
				list[i].SetActive(false);
			}
		}
	}

	private void CheckKeyType()
	{
		if (txtKey.gameObject.activeSelf)
		{
			m_BtnType = PnlInputPc.keyType.NormalKey;
		}
		else if (txtKeyNo.gameObject.activeSelf)
		{
			m_BtnType = PnlInputPc.keyType.NoKey;
		}
		else if (txtKeySetting.gameObject.activeSelf)
		{
			m_BtnType = PnlInputPc.keyType.SettingKey;
		}
		else if (imgSpecialButton.gameObject.activeSelf)
		{
			m_BtnType = PnlInputPc.keyType.SpecialKey;
		}
	}
}
