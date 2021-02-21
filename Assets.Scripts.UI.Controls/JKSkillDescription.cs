using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.PreWarm;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class JKSkillDescription : MonoBehaviour, IPreWarm
	{
		private Text m_Text;

		public void SetText()
		{
			string str = string.Empty;
			string text = "steam";
			Scheme scheme = SingletonScriptableObject<LocalizationSettings>.instance.GetScheme("Language");
			switch (scheme.activeOption.name)
			{
			case "English":
				switch (text)
				{
				case "ios":
				case "google":
					str = "Unlock by purchasing [MUSE RADIO].";
					break;
				case "taptap":
					str = "Unlock by purchasing [MUSE RADIO]，You can use character after logging in.";
					break;
				case "steam":
					str = "Unlock by purchasing [Just as planned].";
					break;
				}
				break;
			case "ChineseS":
				switch (text)
				{
				case "ios":
				case "google":
					str = "购买「暮色电台」曲包后解锁。";
					break;
				case "taptap":
					str = "购买「暮色电台」曲包后解锁，只有在登陆状态下才能使用。";
					break;
				case "steam":
					str = "购买「计划通」后解锁。";
					break;
				}
				break;
			case "ChineseT":
				switch (text)
				{
				case "ios":
				case "google":
					str = "購買「暮色電台」曲包後解鎖。";
					break;
				case "taptap":
					str = "購買“暮色電台”曲包後解鎖，只有在登陸狀態下才能使用。";
					break;
				case "steam":
					str = "購買“計劃通”後解鎖。";
					break;
				}
				break;
			case "Japanese":
				switch (text)
				{
				case "ios":
				case "google":
					str = "「ミューズラジオ」を購入して解禁。";
					break;
				case "taptap":
					str = "「ミューズラジオ」を購入して解禁，ログイン時にのみ使用可能。";
					break;
				case "steam":
					str = "「計画通り」を購入して解禁。";
					break;
				}
				break;
			case "Korean":
				switch (text)
				{
				case "ios":
				case "google":
					str = "「뮤즈 라디오」 구입 후 해금.";
					break;
				case "taptap":
					str = "「뮤즈 라디오」 구입 후 해금, 로그인 상태에서만 사용할 수 있습니다.";
					break;
				case "steam":
					str = "「계획대로」 구입 후 해금.";
					break;
				}
				break;
			}
			Text text2 = m_Text;
			text2.text = text2.text + "\n" + str;
		}

		public void PreWarm(int slice)
		{
			m_Text = GetComponent<Text>();
		}
	}
}
