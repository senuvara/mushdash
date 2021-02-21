using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using UnityEngine;
using UnityEngine.UI;

public class DLCInfo : MonoBehaviour
{
	public VariableBehaviour albumName;

	public GameObject unlockAllDlc;

	public GameObject normalDlc;

	public Image[] normalDlcImages;

	public GameObject specialDlc;

	public Image[] specialDlcImages;

	public GameObject cytusIIDlc;

	public Image[] cytusIIDlcImages;

	public GameObject charJKBase;

	public GameObject welcomeNanaBase;

	public GameObject charGCBase;

	public GameObject welcomeBadBase;

	private void OnEnable()
	{
		OnChangeStage();
	}

	public void OnChangeStage()
	{
		base.name = albumName.variable.GetResult<string>();
		SetDlcBase();
		normalDlc.SetActive(false);
		specialDlc.SetActive(false);
		cytusIIDlc.SetActive(false);
		unlockAllDlc.SetActive(false);
		if (string.IsNullOrEmpty(base.name))
		{
			unlockAllDlc.SetActive(true);
			return;
		}
		Sprite sprite = new Sprite();
		if (base.name == "ALBUM22")
		{
			for (int i = 0; i < specialDlcImages.Length; i++)
			{
				sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>((string)Singleton<ConfigManager>.instance.GetJson(base.name, false)[i]["cover"]);
				specialDlcImages[i].sprite = sprite;
			}
			specialDlc.SetActive(true);
		}
		else if (base.name == "ALBUM34")
		{
			for (int j = 0; j < cytusIIDlcImages.Length; j++)
			{
				sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>((string)Singleton<ConfigManager>.instance.GetJson(base.name, false)[j]["cover"]);
				cytusIIDlcImages[j].sprite = sprite;
			}
			cytusIIDlc.SetActive(true);
		}
		else
		{
			for (int k = 0; k < normalDlcImages.Length; k++)
			{
				sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>((string)Singleton<ConfigManager>.instance.GetJson(base.name, false)[k]["cover"]);
				normalDlcImages[k].sprite = sprite;
			}
			normalDlc.SetActive(true);
		}
	}

	private void SetDlcBase()
	{
		charJKBase.SetActive(false);
		welcomeNanaBase.SetActive(false);
		charGCBase.SetActive(false);
		welcomeBadBase.SetActive(false);
		if (base.name == "ALBUM26")
		{
			charJKBase.SetActive(true);
		}
		if (base.name == "ALBUM28")
		{
			welcomeNanaBase.SetActive(true);
		}
		if (base.name == "ALBUM30")
		{
			charGCBase.SetActive(true);
		}
		if (base.name == "ALBUM33")
		{
			welcomeBadBase.SetActive(true);
		}
	}
}
