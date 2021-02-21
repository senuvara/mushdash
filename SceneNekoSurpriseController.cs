using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using System.Collections.Generic;
using UnityEngine;

public class SceneNekoSurpriseController : MonoBehaviour
{
	public List<GameObject> commonObj;

	public List<GameObject> cytusSurpriseObj;

	public bool isUIBarrage;

	private void Start()
	{
		if (isUIBarrage && !Singleton<BattleProperty>.instance.isNekoCharacter)
		{
			return;
		}
		if (Singleton<BattleProperty>.instance.isNekoCharacter || Singleton<StageBattleComponent>.instance.GetAlbumName() == "ALBUM34")
		{
			foreach (GameObject item in commonObj)
			{
				item.SetActive(false);
			}
			foreach (GameObject item2 in cytusSurpriseObj)
			{
				item2.SetActive(true);
			}
			return;
		}
		foreach (GameObject item3 in cytusSurpriseObj)
		{
			item3.SetActive(false);
		}
		foreach (GameObject item4 in commonObj)
		{
			item4.SetActive(true);
		}
	}
}
