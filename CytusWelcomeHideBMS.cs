using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using GameLogic;
using UnityEngine;
using UnityEngine.UI;

public class CytusWelcomeHideBMS : MonoBehaviour
{
	public Button btnOnPlayAnim;

	private AudioSource m_AudioSource;

	private Animator m_Animator;

	private bool isOnHideSong;

	private float m_DelayTime = 1.3f;

	private float m_AudioVolume;

	private void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_AudioSource = GetComponent<AudioSource>();
		isOnHideSong = false;
		m_AudioVolume = Singleton<DataManager>.instance["GameConfig"]["BGMVolume"].GetResult<float>();
		Init();
	}

	private void Init()
	{
		btnOnPlayAnim.onClick.AddListener(delegate
		{
			isOnHideSong = true;
			GameGlobal.isCytusHideBMS = true;
			m_Animator.SetLayerWeight(1, 1f);
			m_Animator.Play("Click", 1, 0f);
			btnOnPlayAnim.gameObject.SetActive(false);
			Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].SetResult(2);
			Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult("music_package_33");
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(12);
			Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].SetResult("33-12");
			Singleton<EventManager>.instance.Invoke("UI/OnCytusHideSongTrigger");
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Singleton<EventManager>.instance.Invoke("UI/OnCytusHideSongShow");
			}, 1.9f);
		});
		btnOnPlayAnim.gameObject.SetActive(false);
	}

	public void OnFuckStart()
	{
		btnOnPlayAnim.gameObject.SetActive(true);
		if (!isOnHideSong)
		{
			m_AudioSource.DOFade(m_AudioVolume, m_DelayTime);
			m_AudioSource.volume = 0f;
			m_AudioSource.Play();
			Singleton<AudioManager>.instance.bgm.DOFade(m_AudioVolume * 0.3f, m_DelayTime);
		}
	}

	public void OnFuckEnd()
	{
		btnOnPlayAnim.gameObject.SetActive(false);
		if (!isOnHideSong)
		{
			m_AudioSource.DOFade(0f, m_DelayTime);
			Singleton<AudioManager>.instance.bgm.DOFade(m_AudioVolume, m_DelayTime);
		}
	}
}
