using System;

namespace NCalc
{
	[Flags]
	public enum EvaluateOptions
	{
		None = 0x1,
		IgnoreCase = 0x2,
		NoCache = 0x4,
		IterateParameters = 0x8,
		RoundAwayFromZero = 0x10
	}
}
