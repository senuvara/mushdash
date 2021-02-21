using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System;
using UnityEngine;

namespace GameLogic
{
	public struct MusicData
	{
		public short objId;

		public decimal tick;

		public MusicConfigData configData;

		[NonSerialized]
		public NoteConfigData noteData;

		public bool isLongPressing;

		public int doubleIdx;

		public bool isLongPressEnd;

		public decimal longPressPTick;

		public int endIndex;

		public decimal dt;

		public int longPressNum;

		public decimal showTick;

		public bool isLongPressType => isLongPressStart || isLongPressing || isLongPressEnd;

		public bool isAir => noteData.pathway == 1;

		public bool isMul => configData.length > 0m && noteData.type == 8;

		public bool isLongPressStart => configData.length > 0m && noteData.type == 3;

		public int longPressCount => Mathf.CeilToInt((float)(configData.length / 0.1m));

		public int GetMulHitLowThreshold()
		{
			if (!isMul)
			{
				return 0;
			}
			float num = (float)configData.length;
			return Mathf.FloorToInt(num * float.Parse(SingletonScriptableObject<ConstanceManager>.instance["mulHitLowThreshold"]));
		}

		public int GetMulHitMidThreshold()
		{
			if (!isMul)
			{
				return 0;
			}
			float num = (float)configData.length;
			return Mathf.FloorToInt(num * float.Parse(SingletonScriptableObject<ConstanceManager>.instance["mulHitMidThreshold"]));
		}

		public int GetMulHitHighThreshold()
		{
			if (!isMul)
			{
				return 0;
			}
			float num = (float)configData.length;
			return Mathf.FloorToInt(num * float.Parse(SingletonScriptableObject<ConstanceManager>.instance["mulHitHighThreshold"]));
		}
	}
}
