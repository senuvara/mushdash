using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using TMPro;
using UnityEngine;

public class ChangeFxTextValue : MonoBehaviour
{
	public enum FxType
	{
		Hp,
		Hurt,
		Score
	}

	public FxType fxType;

	private void OnEnable()
	{
		string text = string.Empty;
		switch (fxType)
		{
		case FxType.Hp:
			text = Singleton<TaskStageTarget>.instance.GetRecover().ToString();
			break;
		case FxType.Hurt:
			text = BattleRoleAttributeComponent.instance.GetHurtValue().ToString();
			break;
		case FxType.Score:
			text = Singleton<TaskStageTarget>.instance.GetAddScore().ToString();
			break;
		}
		GetComponent<TextMeshPro>().SetText(text);
	}
}
