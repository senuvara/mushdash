using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class RankCell : MonoBehaviour
	{
		public Text txtNumber;

		public Text txtPlayerName;

		public Text txtScore;

		public Text txtAcc;

		public void SetValue(int number, string nickName, int score, float acc)
		{
			txtNumber.text = number.ToString("00");
			txtPlayerName.text = nickName;
			txtScore.text = score.ToString();
			txtAcc.text = acc.ToString("P2");
		}
	}
}
