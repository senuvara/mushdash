using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.UI;
using Assets.Scripts.UI.Controls;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine;

public class PnlRankScrollRect : ReuseScorllRect
{
	private JToken m_Token;

	public float axisSpeed;

	public string axisName;

	public void SetToken(JToken t)
	{
		if (t == null)
		{
			return;
		}
		m_Token = t;
		int num = m_Token.Count();
		if (num > 0)
		{
			SetContentCount(num);
			InitList();
			int num2 = (num < seeCount) ? num : seeCount;
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = content.GetChild(i).gameObject;
				gameObject.SetActive(true);
				SetCell(gameObject, i);
			}
			for (int j = num2; j < content.childCount; j++)
			{
				content.GetChild(j).gameObject.SetActive(false);
			}
		}
	}

	public override void SetCell(GameObject obj, int index)
	{
		JToken jToken = m_Token[index];
		string nickName = (string)jToken["user"]["nickname"];
		int score = (int)jToken["play"]["score"];
		float acc = (float)jToken["play"]["acc"] / 100f;
		int number = index + 1;
		RankCell component = obj.GetComponent<RankCell>();
		component.SetValue(number, nickName, score, acc);
	}

	public override void OnUpdate()
	{
		float num = 0f;
		num = Input.GetAxisRaw(axisName);
		if (num == 0f)
		{
			num = Singleton<InputManager>.instance.RewiredGetAxisRaw(axisName);
		}
		if (num > 0.7f)
		{
			scrollbar.value += axisSpeed;
		}
		else if (num < -0.7f)
		{
			scrollbar.value -= axisSpeed;
		}
	}
}
