using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;
using UnityEngine.UI;

public class PnlPrivacyPolicy : MonoBehaviour
{
	public Button btnOnPrivacyPolicy;

	public Button btnOnPermission;

	private Coroutine m_PermissionCoroutine;

	private void Start()
	{
		if (Singleton<DataManager>.instance["GameConfig"]["PrivacyPolicy"].GetResult<bool>())
		{
			Object.Destroy(base.gameObject);
			Object.Destroy(btnOnPrivacyPolicy.gameObject);
			return;
		}
		btnOnPrivacyPolicy.onClick.AddListener(delegate
		{
			OnPrivacyPolicy();
		});
		btnOnPermission.onClick.AddListener(delegate
		{
			OnPermission();
		});
		Object.Destroy(base.gameObject);
		Object.Destroy(btnOnPrivacyPolicy.gameObject);
	}

	private void OnPrivacyPolicy()
	{
	}

	private void OnPermission()
	{
	}
}
