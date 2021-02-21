using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore.Managers
{
	public class StageAchievementManager : Singleton<StageAchievementManager>
	{
		private SingletonDataObject m_StageAchievement;

		private List<string> m_StageAchievements;

		private void Init()
		{
			m_StageAchievement = Singleton<DataManager>.instance["StageAchievement"];
			m_StageAchievements = m_StageAchievement["stage_achievements"].GetResult<List<string>>();
		}

		public void Do()
		{
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			int diffcult = (int)Singleton<StageBattleComponent>.instance.GetDiffcult();
			int evaluateNum = Singleton<StageBattleComponent>.instance.evaluateNum;
			int score = Singleton<TaskStageTarget>.instance.GetScore();
			int combo = Singleton<TaskStageTarget>.instance.GetComboMax();
			bool isFullCombo = Singleton<TaskStageTarget>.instance.IsFullCombo();
			int miss = Singleton<TaskStageTarget>.instance.GetComboMiss();
			int hurtMiss = Singleton<TaskStageTarget>.instance.GetMiss();
			int fever = FeverManager.Instance.feverCount;
			if (result == "0-48")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 40000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 80);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "0-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 50000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "0-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				}
			}
			if (result == "0-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 35000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "0-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 80);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => score >= 100000);
					break;
				}
			}
			if (result == "0-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					DoStageAchievement(result, diffcult, 1, () => combo >= 150);
					break;
				}
			}
			if (result == "0-6")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 55000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "0-37")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => score >= 85000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "0-7")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-8")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 65000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "0-9")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "0-49")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-10")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => score >= 110000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "0-11")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "0-12")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => hurtMiss >= 20);
					break;
				}
			}
			if (result == "0-40")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 180000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-13")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 75000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-14")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 170000);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"01",
						"02"
					}, 4));
					break;
				}
			}
			if (result == "0-15")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 110000);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-16")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score > 180000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "0-17")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => isFullCombo);
					break;
				}
			}
			if (result == "0-18")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 70000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "0-19")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-20")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 180000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"OH",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "0-21")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 90000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => miss <= 10);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-22")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				}
			}
			if (result == "0-42")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "0-23")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 280000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-24")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-50")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 201);
					DoStageAchievement(result, diffcult, 1, () => miss <= 9);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 12);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => fever >= 5);
					break;
				}
			}
			if (result == "0-51")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 122500);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 19);
					DoStageAchievement(result, diffcult, 1, () => miss <= 8);
					DoStageAchievement(result, diffcult, 2, () => fever >= 8);
					break;
				}
			}
			if (result == "0-25")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 140000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => combo >= 500);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "0-26")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 340000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"OH",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-27")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 200000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-28")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 500);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => miss <= 5);
					break;
				}
			}
			if (result == "0-29")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 150000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "0-30")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 45000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "0-43")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-31")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 80000);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-32")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-33")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 115000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "0-44")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "0-34")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => score >= 300000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "0-35")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "0-36")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-41")
			{
				switch (diffcult)
				{
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-38")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => score >= 110000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 250);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "0-39")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 180);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 260000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-45")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 130000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "0-46")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 85000);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "0-47")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 12);
					DoStageAchievement(result, diffcult, 1, () => miss <= 25);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "1-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 10);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "1-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "1-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "1-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 70000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "1-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => score >= 100000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "1-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => miss <= 5);
					break;
				}
			}
			if (result == "2-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 240000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 600);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "2-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 300000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "2-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => isFullCombo);
					break;
				}
			}
			if (result == "2-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "2-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 340000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "2-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 700);
					DoStageAchievement(result, diffcult, 1, () => fever >= 9);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "3-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 55000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 140);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "3-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 450);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "3-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => score >= 200000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "3-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 110000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "3-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 140000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "3-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 120000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 500);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "4-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => score >= 240000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "4-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 140000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "4-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 110000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "4-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 150000);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "4-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "4-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 500);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "5-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "5-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 330000);
					DoStageAchievement(result, diffcult, 1, () => fever >= 8);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "5-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => score >= 90000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "5-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "5-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "5-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 200000);
					DoStageAchievement(result, diffcult, 1, () => combo >= 350);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "6-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 220000);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "6-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 90000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 350);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "6-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 500);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "6-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "6-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => combo >= 150);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 150000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "6-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "7-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "7-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => fever >= 9);
					DoStageAchievement(result, diffcult, 2, () => hurtMiss >= 20);
					break;
				}
			}
			if (result == "7-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 85000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "7-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => score >= 350000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "7-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "7-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 250000);
					DoStageAchievement(result, diffcult, 1, () => fever >= 7);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 500);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "8-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => score >= 170000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "8-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "8-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 95000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "8-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 450);
					DoStageAchievement(result, diffcult, 1, () => score >= 330000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "8-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => score >= 270000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "8-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 180000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 8);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 600);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "9-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 65000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => combo >= 350);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "9-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => score >= 230000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "9-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "9-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 230000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "9-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "9-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 310000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 600);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "10-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "10-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 8);
					DoStageAchievement(result, diffcult, 1, () => score >= 310000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "10-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 480000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "10-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 8);
					DoStageAchievement(result, diffcult, 1, () => combo >= 500);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "10-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "10-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 270000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "11-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 150);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => score >= 150000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "11-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "11-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "11-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => score >= 180000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "11-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => combo >= 500);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "11-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "12-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 210000);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "12-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "12-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 110000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "12-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => combo >= 450);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "12-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 10);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "12-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 270000);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "13-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "13-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => combo >= 120);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 90000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "13-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 130000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "13-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 60000);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "13-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "13-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "14-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "14-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 170);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "14-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "14-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 130000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => miss <= 10);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "14-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 170000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "14-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 450);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "15-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 135000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "15-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 70000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 180);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "15-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "15-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => score >= 120000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "15-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => isFullCombo);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "15-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "16-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "16-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 280000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "16-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "16-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => score >= 200000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "16-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 300000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "16-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "17-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 160000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => combo >= 300);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "17-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 271);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "17-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 140000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "17-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => combo >= 250);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "17-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 115000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "17-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 210000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "18-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => score >= 110000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "18-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 70000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "18-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "18-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 280000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "18-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "18-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "19-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "19-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 271);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 400000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "19-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 300000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "19-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 114514);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 19);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 19);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 8);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "19-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 350);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "19-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 550);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "20-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 90000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 200);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "20-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 2);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "20-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 210000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "20-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "20-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 280000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "20-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 550);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "21-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					DoStageAchievement(result, diffcult, 1, () => score >= 110000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "21-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "21-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 150000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "22-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 180000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "22-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 222);
					DoStageAchievement(result, diffcult, 1, () => miss <= 22);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "22-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 300000);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "22-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 240000);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "22-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "22-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "23-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => score >= 140000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "23-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "23-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => fever >= 3);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "23-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 70000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "23-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "23-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "24-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 150000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "24-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => hurtMiss >= 20);
					break;
				}
			}
			if (result == "24-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "24-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 180000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "24-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "24-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 350000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "25-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "25-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 70000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "25-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 200000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "25-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "25-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 180000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "25-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "26-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => score >= 200000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "26-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 260000);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "26-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 271);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "26-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 135000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 7);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 600);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "26-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "26-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 9);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "27-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => score >= 233333);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "27-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "27-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "27-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 177770);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "27-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 177770);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 8);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "27-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => miss <= 5);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => combo >= 550);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "28-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 220);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 340000);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "28-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 135000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 450);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "28-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "28-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "28-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "28-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 180000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "29-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 4);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "29-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 300);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 333333);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "29-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "29-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 111111);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "29-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 222222);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 1, () => miss <= 8);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "29-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 2);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "30-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 130000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "30-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "30-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "30-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => score >= 200000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "30-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "30-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "31-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 160000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "31-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 90000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => fever >= 5);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "31-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => fever >= 6);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "31-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => combo >= 350);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "31-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 250);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => score >= 270000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "31-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "32-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 75000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => combo >= 130);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "32-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "32-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 150);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "32-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 100);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "32-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 100000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "32-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 8);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => score >= 200000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "33-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 110000);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 1);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "33-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => combo >= 280);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 2);
					DoStageAchievement(result, diffcult, 1, () => score >= 240000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "33-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 3);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "33-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 4);
					DoStageAchievement(result, diffcult, 1, () => score >= 400000);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "33-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					DoStageAchievement(result, diffcult, 1, () => fever >= 10);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "33-5")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 6);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				}
			}
			if (result == "33-6")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => fever >= 4);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 7);
					DoStageAchievement(result, diffcult, 1, () => combo >= 350);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "33-7")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 150000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 8);
					DoStageAchievement(result, diffcult, 1, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				}
			}
			if (result == "33-8")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => fever >= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => isFullCombo);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 9);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "33-9")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => score >= 85000);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 10);
					DoStageAchievement(result, diffcult, 1, () => fever >= 8);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				}
			}
			if (result == "33-10")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => combo >= 400);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 11);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				}
			}
			if (result == "33-11")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => combo >= 200);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => score >= 234567);
					DoStageAchievement(result, diffcult, 1, () => hurtMiss >= 20);
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => miss <= 12);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				}
			}
			if (result == "34-0")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 5);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"01",
						"02",
						"03"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 13);
					DoStageAchievement(result, diffcult, 1, () => combo >= 600);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0A",
						"0B"
					}, 4));
					break;
				}
			}
			if (result == "34-1")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 4);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0C",
						"0D"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 14);
					DoStageAchievement(result, diffcult, 1, () => combo >= 700);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0E"
					}, 4));
					break;
				}
			}
			if (result == "34-2")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 3);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"0F"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 15);
					DoStageAchievement(result, diffcult, 1, () => combo >= 800);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"0H",
						"18"
					}, 4));
					break;
				}
			}
			if (result == "34-3")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 2);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[3]
					{
						"13",
						"14",
						"15"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 16);
					DoStageAchievement(result, diffcult, 1, () => combo >= 400);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[2]
					{
						"11",
						"12"
					}, 4));
					break;
				}
			}
			if (result == "34-4")
			{
				switch (diffcult)
				{
				case 1:
					DoStageAchievement(result, diffcult, 0, () => miss <= 1);
					break;
				case 2:
					DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 4, 2, 1, (int value, int target) => value >= target);
					DoStageAchievement(result, diffcult, 1, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"21"
					}, 4));
					break;
				case 3:
					DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 17);
					DoStageAchievement(result, diffcult, 1, () => combo >= 500);
					DoStageAchievement(result, diffcult, 2, () => Singleton<TaskStageTarget>.instance.IsAll(new string[1]
					{
						"23"
					}, 4));
					break;
				}
			}
			if (!(result == "34-5"))
			{
				return;
			}
			switch (diffcult)
			{
			case 1:
				DoStageAchievement(result, diffcult, 0, () => isFullCombo);
				break;
			case 2:
				DoStageAchievement(result, diffcult, 0, () => evaluateNum >= 3, 3, 1, (int value, int target) => value >= target);
				DoStageAchievement(result, diffcult, 1, () => fever >= 10);
				break;
			case 3:
				DoStageAchievement(result, diffcult, 0, () => hurtMiss >= 18);
				DoStageAchievement(result, diffcult, 1, () => combo >= 900);
				DoStageAchievement(result, diffcult, 2, () => score >= 600000);
				break;
			}
		}

		private void DoStageAchievement(string musicName, int difficulty, int uid, Func<bool> isDone, int target = -1, int value = -1, Func<int, int, bool> compareFunc = null)
		{
			object arg;
			switch (difficulty)
			{
			case 1:
				arg = "easy";
				break;
			case 2:
				arg = "normal";
				break;
			default:
				arg = "hard";
				break;
			}
			string text = $"{musicName}-{arg}-{uid}";
			if (m_StageAchievements.Contains(text) || !isDone())
			{
				return;
			}
			if (target == -1)
			{
				Reward(text);
				return;
			}
			int result = m_StageAchievement[text].GetResult<int>();
			result += value;
			m_StageAchievement[text].SetResult(result);
			if (compareFunc != null && compareFunc(result, target))
			{
				Reward(text);
			}
		}

		private void Reward(string uid)
		{
			m_StageAchievements.Add(uid);
			SingletonMonoBehaviour<MessageManager>.instance.Send("stage_achievement", uid);
		}
	}
}
