using GameLogic;
using System;

namespace FormulaBase
{
	public class TimeNodeOrder
	{
		public short idx;

		public byte result;

		public bool enableJump;

		public bool isPerfectNode;

		public bool isLongPressStart;

		public bool isLongPressEnd;

		public bool isLongPressing;

		public bool isMulStart;

		public bool isMuling;

		public bool isAir;

		public bool isFucked;

		public bool isLast;

		public bool isFirst;

		public bool isRight;

		[NonSerialized]
		public MusicData md;

		public bool isLongPressType => isLongPressStart || isLongPressing || isLongPressEnd;

		public bool isMulType => isMulStart || isMuling;
	}
}
