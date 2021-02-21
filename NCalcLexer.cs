using Antlr.Runtime;
using System;
using System.CodeDom.Compiler;

[GeneratedCode("ANTLR", "3.3.0.7239")]
[CLSCompliant(false)]
public class NCalcLexer : Lexer
{
	private class DFA7 : DFA
	{
		private const string DFA7_eotS = "\u0004\uffff";

		private const string DFA7_eofS = "\u0004\uffff";

		private const string DFA7_minS = "\u0002.\u0002\uffff";

		private const string DFA7_maxS = "\u00019\u0001e\u0002\uffff";

		private const string DFA7_acceptS = "\u0002\uffff\u0001\u0001\u0001\u0002";

		private const string DFA7_specialS = "\u0004\uffff}>";

		private static readonly string[] DFA7_transitionS;

		private static readonly short[] DFA7_eot;

		private static readonly short[] DFA7_eof;

		private static readonly char[] DFA7_min;

		private static readonly char[] DFA7_max;

		private static readonly short[] DFA7_accept;

		private static readonly short[] DFA7_special;

		private static readonly short[][] DFA7_transition;

		public override string Description => "252:1: FLOAT : ( ( DIGIT )* '.' ( DIGIT )+ ( E )? | ( DIGIT )+ E );";

		static DFA7()
		{
			DFA7_transitionS = new string[4]
			{
				"\u0001\u0002\u0001\uffff\n\u0001",
				"\u0001\u0002\u0001\uffff\n\u0001\v\uffff\u0001\u0003\u001f\uffff\u0001\u0003",
				string.Empty,
				string.Empty
			};
			DFA7_eot = DFA.UnpackEncodedString("\u0004\uffff");
			DFA7_eof = DFA.UnpackEncodedString("\u0004\uffff");
			DFA7_min = DFA.UnpackEncodedStringToUnsignedChars("\u0002.\u0002\uffff");
			DFA7_max = DFA.UnpackEncodedStringToUnsignedChars("\u00019\u0001e\u0002\uffff");
			DFA7_accept = DFA.UnpackEncodedString("\u0002\uffff\u0001\u0001\u0001\u0002");
			DFA7_special = DFA.UnpackEncodedString("\u0004\uffff}>");
			int num = DFA7_transitionS.Length;
			DFA7_transition = new short[num][];
			for (int i = 0; i < num; i++)
			{
				DFA7_transition[i] = DFA.UnpackEncodedString(DFA7_transitionS[i]);
			}
		}

		public DFA7(BaseRecognizer recognizer)
		{
			base.recognizer = recognizer;
			decisionNumber = 7;
			eot = DFA7_eot;
			eof = DFA7_eof;
			min = DFA7_min;
			max = DFA7_max;
			accept = DFA7_accept;
			special = DFA7_special;
			transition = DFA7_transition;
		}

		public override void Error(NoViableAltException nvae)
		{
		}
	}

	private class DFA14 : DFA
	{
		private const string DFA14_eotS = "\u0001\uffff\u0001!\u0001\uffff\u0001#\b\uffff\u0001'\u0001)\u0001,\u0002\uffff\u0003\u001e\u00011\u0001\uffff\u0003\u001e\u00016\u0013\uffff\u0002\u001e\u00019\u0002\uffff\u0003\u001e\u0002\uffff\u0001<\u0001=\u0001\uffff\u0002\u001e\u0002\uffff\u0001@\u0001\u001e\u0001\uffff\u0001B\u0001\uffff";

		private const string DFA14_eofS = "C\uffff";

		private const string DFA14_minS = "\u0001\t\u0001=\u0001\uffff\u0001&\b\uffff\u0001<\u0002=\u0002\uffff\u0001n\u0001o\u0001r\u0001|\u0001\uffff\u0001r\u0001a\u0001+\u0001.\u0013\uffff\u0001d\u0001t\u00010\u0002\uffff\u0001u\u0001l\u00010\u0002\uffff\u00020\u0001\uffff\u0001e\u0001s\u0002\uffff\u00010\u0001e\u0001\uffff\u00010\u0001\uffff";

		private const string DFA14_maxS = "\u0001~\u0001=\u0001\uffff\u0001&\b\uffff\u0001>\u0001=\u0001>\u0002\uffff\u0001n\u0001o\u0001r\u0001|\u0001\uffff\u0001r\u0001a\u00019\u0001e\u0013\uffff\u0001d\u0001t\u0001z\u0002\uffff\u0001u\u0001l\u00019\u0002\uffff\u0002z\u0001\uffff\u0001e\u0001s\u0002\uffff\u0001z\u0001e\u0001\uffff\u0001z\u0001\uffff";

		private const string DFA14_acceptS = "\u0002\uffff\u0001\u0003\u0001\uffff\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\n\u0001\v\u0001\f\u0001\r\u0003\uffff\u0001\u0017\u0001\u0018\u0004\uffff\u0001\u001e\u0004\uffff\u0001#\u0001$\u0001%\u0001&\u0001!\u0001(\u0001\u0002\u0001\u0001\u0001\u0004\u0001\u0005\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u000e\u0001\u0013\u0001\u0012\u0001\u0015\u0001\u0016\u0001\u0014\u0003\uffff\u0001\u001d\u0001\u001c\u0003\uffff\u0001'\u0001\"\u0002\uffff\u0001\u001b\u0002\uffff\u0001\u0019\u0001\u001a\u0002\uffff\u0001\u001f\u0001\uffff\u0001 ";

		private const string DFA14_specialS = "C\uffff}>";

		private static readonly string[] DFA14_transitionS;

		private static readonly short[] DFA14_eot;

		private static readonly short[] DFA14_eof;

		private static readonly char[] DFA14_min;

		private static readonly char[] DFA14_max;

		private static readonly short[] DFA14_accept;

		private static readonly short[] DFA14_special;

		private static readonly short[][] DFA14_transition;

		public override string Description => "1:1: Tokens : ( T__19 | T__20 | T__21 | T__22 | T__23 | T__24 | T__25 | T__26 | T__27 | T__28 | T__29 | T__30 | T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | TRUE | FALSE | ID | INTEGER | FLOAT | STRING | DATETIME | NAME | E | WS );";

		static DFA14()
		{
			DFA14_transitionS = new string[67]
			{
				"\u0002\u001f\u0001\uffff\u0002\u001f\u0012\uffff\u0001\u001f\u0001\u0001\u0001\uffff\u0001\u001c\u0001\uffff\u0001\u0002\u0001\u0003\u0001\u001b\u0001\u0004\u0001\u0005\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\u001a\u0001\n\n\u0019\u0001\v\u0001\uffff\u0001\f\u0001\r\u0001\u000e\u0001\u000f\u0001\uffff\u0004\u001e\u0001\u0018\u0015\u001e\u0001\u001d\u0002\uffff\u0001\u0010\u0001\u001e\u0001\uffff\u0001\u0011\u0003\u001e\u0001\u0018\u0001\u0017\a\u001e\u0001\u0012\u0001\u0013\u0004\u001e\u0001\u0016\u0006\u001e\u0001\uffff\u0001\u0014\u0001\uffff\u0001\u0015",
				"\u0001 ",
				string.Empty,
				"\u0001\"",
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				"\u0001$\u0001%\u0001&",
				"\u0001(",
				"\u0001*\u0001+",
				string.Empty,
				string.Empty,
				"\u0001-",
				"\u0001.",
				"\u0001/",
				"\u00010",
				string.Empty,
				"\u00012",
				"\u00013",
				"\u00015\u0001\uffff\u00015\u0002\uffff\n4",
				"\u0001\u001a\u0001\uffff\n\u0019\v\uffff\u0001\u001a\u001f\uffff\u0001\u001a",
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				"\u00017",
				"\u00018",
				"\n\u001e\a\uffff\u001a\u001e\u0004\uffff\u0001\u001e\u0001\uffff\u001a\u001e",
				string.Empty,
				string.Empty,
				"\u0001:",
				"\u0001;",
				"\n4",
				string.Empty,
				string.Empty,
				"\n\u001e\a\uffff\u001a\u001e\u0004\uffff\u0001\u001e\u0001\uffff\u001a\u001e",
				"\n\u001e\a\uffff\u001a\u001e\u0004\uffff\u0001\u001e\u0001\uffff\u001a\u001e",
				string.Empty,
				"\u0001>",
				"\u0001?",
				string.Empty,
				string.Empty,
				"\n\u001e\a\uffff\u001a\u001e\u0004\uffff\u0001\u001e\u0001\uffff\u001a\u001e",
				"\u0001A",
				string.Empty,
				"\n\u001e\a\uffff\u001a\u001e\u0004\uffff\u0001\u001e\u0001\uffff\u001a\u001e",
				string.Empty
			};
			DFA14_eot = DFA.UnpackEncodedString("\u0001\uffff\u0001!\u0001\uffff\u0001#\b\uffff\u0001'\u0001)\u0001,\u0002\uffff\u0003\u001e\u00011\u0001\uffff\u0003\u001e\u00016\u0013\uffff\u0002\u001e\u00019\u0002\uffff\u0003\u001e\u0002\uffff\u0001<\u0001=\u0001\uffff\u0002\u001e\u0002\uffff\u0001@\u0001\u001e\u0001\uffff\u0001B\u0001\uffff");
			DFA14_eof = DFA.UnpackEncodedString("C\uffff");
			DFA14_min = DFA.UnpackEncodedStringToUnsignedChars("\u0001\t\u0001=\u0001\uffff\u0001&\b\uffff\u0001<\u0002=\u0002\uffff\u0001n\u0001o\u0001r\u0001|\u0001\uffff\u0001r\u0001a\u0001+\u0001.\u0013\uffff\u0001d\u0001t\u00010\u0002\uffff\u0001u\u0001l\u00010\u0002\uffff\u00020\u0001\uffff\u0001e\u0001s\u0002\uffff\u00010\u0001e\u0001\uffff\u00010\u0001\uffff");
			DFA14_max = DFA.UnpackEncodedStringToUnsignedChars("\u0001~\u0001=\u0001\uffff\u0001&\b\uffff\u0001>\u0001=\u0001>\u0002\uffff\u0001n\u0001o\u0001r\u0001|\u0001\uffff\u0001r\u0001a\u00019\u0001e\u0013\uffff\u0001d\u0001t\u0001z\u0002\uffff\u0001u\u0001l\u00019\u0002\uffff\u0002z\u0001\uffff\u0001e\u0001s\u0002\uffff\u0001z\u0001e\u0001\uffff\u0001z\u0001\uffff");
			DFA14_accept = DFA.UnpackEncodedString("\u0002\uffff\u0001\u0003\u0001\uffff\u0001\u0006\u0001\a\u0001\b\u0001\t\u0001\n\u0001\v\u0001\f\u0001\r\u0003\uffff\u0001\u0017\u0001\u0018\u0004\uffff\u0001\u001e\u0004\uffff\u0001#\u0001$\u0001%\u0001&\u0001!\u0001(\u0001\u0002\u0001\u0001\u0001\u0004\u0001\u0005\u0001\u000f\u0001\u0010\u0001\u0011\u0001\u000e\u0001\u0013\u0001\u0012\u0001\u0015\u0001\u0016\u0001\u0014\u0003\uffff\u0001\u001d\u0001\u001c\u0003\uffff\u0001'\u0001\"\u0002\uffff\u0001\u001b\u0002\uffff\u0001\u0019\u0001\u001a\u0002\uffff\u0001\u001f\u0001\uffff\u0001 ");
			DFA14_special = DFA.UnpackEncodedString("C\uffff}>");
			int num = DFA14_transitionS.Length;
			DFA14_transition = new short[num][];
			for (int i = 0; i < num; i++)
			{
				DFA14_transition[i] = DFA.UnpackEncodedString(DFA14_transitionS[i]);
			}
		}

		public DFA14(BaseRecognizer recognizer)
		{
			base.recognizer = recognizer;
			decisionNumber = 14;
			eot = DFA14_eot;
			eof = DFA14_eof;
			min = DFA14_min;
			max = DFA14_max;
			accept = DFA14_accept;
			special = DFA14_special;
			transition = DFA14_transition;
		}

		public override void Error(NoViableAltException nvae)
		{
		}
	}

	public const int EOF = -1;

	public const int DATETIME = 4;

	public const int DIGIT = 5;

	public const int E = 6;

	public const int EscapeSequence = 7;

	public const int FALSE = 8;

	public const int FLOAT = 9;

	public const int HexDigit = 10;

	public const int ID = 11;

	public const int INTEGER = 12;

	public const int LETTER = 13;

	public const int NAME = 14;

	public const int STRING = 15;

	public const int TRUE = 16;

	public const int UnicodeEscape = 17;

	public const int WS = 18;

	public const int T__19 = 19;

	public const int T__20 = 20;

	public const int T__21 = 21;

	public const int T__22 = 22;

	public const int T__23 = 23;

	public const int T__24 = 24;

	public const int T__25 = 25;

	public const int T__26 = 26;

	public const int T__27 = 27;

	public const int T__28 = 28;

	public const int T__29 = 29;

	public const int T__30 = 30;

	public const int T__31 = 31;

	public const int T__32 = 32;

	public const int T__33 = 33;

	public const int T__34 = 34;

	public const int T__35 = 35;

	public const int T__36 = 36;

	public const int T__37 = 37;

	public const int T__38 = 38;

	public const int T__39 = 39;

	public const int T__40 = 40;

	public const int T__41 = 41;

	public const int T__42 = 42;

	public const int T__43 = 43;

	public const int T__44 = 44;

	public const int T__45 = 45;

	public const int T__46 = 46;

	public const int T__47 = 47;

	public const int T__48 = 48;

	private static readonly bool[] decisionCanBacktrack = new bool[0];

	private DFA7 dfa7;

	private DFA14 dfa14;

	public override string GrammarFileName => "C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g";

	public NCalcLexer()
	{
	}

	public NCalcLexer(ICharStream input)
		: this(input, new RecognizerSharedState())
	{
	}

	public NCalcLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state)
	{
	}

	[GrammarRule("T__19")]
	private void mT__19()
	{
		try
		{
			int type = 19;
			int channel = 0;
			Match(33);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__20")]
	private void mT__20()
	{
		try
		{
			int type = 20;
			int channel = 0;
			Match("!=");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__21")]
	private void mT__21()
	{
		try
		{
			int type = 21;
			int channel = 0;
			Match(37);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__22")]
	private void mT__22()
	{
		try
		{
			int type = 22;
			int channel = 0;
			Match("&&");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__23")]
	private void mT__23()
	{
		try
		{
			int type = 23;
			int channel = 0;
			Match(38);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__24")]
	private void mT__24()
	{
		try
		{
			int type = 24;
			int channel = 0;
			Match(40);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__25")]
	private void mT__25()
	{
		try
		{
			int type = 25;
			int channel = 0;
			Match(41);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__26")]
	private void mT__26()
	{
		try
		{
			int type = 26;
			int channel = 0;
			Match(42);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__27")]
	private void mT__27()
	{
		try
		{
			int type = 27;
			int channel = 0;
			Match(43);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__28")]
	private void mT__28()
	{
		try
		{
			int type = 28;
			int channel = 0;
			Match(44);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__29")]
	private void mT__29()
	{
		try
		{
			int type = 29;
			int channel = 0;
			Match(45);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__30")]
	private void mT__30()
	{
		try
		{
			int type = 30;
			int channel = 0;
			Match(47);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__31")]
	private void mT__31()
	{
		try
		{
			int type = 31;
			int channel = 0;
			Match(58);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__32")]
	private void mT__32()
	{
		try
		{
			int type = 32;
			int channel = 0;
			Match(60);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__33")]
	private void mT__33()
	{
		try
		{
			int type = 33;
			int channel = 0;
			Match("<<");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__34")]
	private void mT__34()
	{
		try
		{
			int type = 34;
			int channel = 0;
			Match("<=");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__35")]
	private void mT__35()
	{
		try
		{
			int type = 35;
			int channel = 0;
			Match("<>");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__36")]
	private void mT__36()
	{
		try
		{
			int type = 36;
			int channel = 0;
			Match(61);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__37")]
	private void mT__37()
	{
		try
		{
			int type = 37;
			int channel = 0;
			Match("==");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__38")]
	private void mT__38()
	{
		try
		{
			int type = 38;
			int channel = 0;
			Match(62);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__39")]
	private void mT__39()
	{
		try
		{
			int type = 39;
			int channel = 0;
			Match(">=");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__40")]
	private void mT__40()
	{
		try
		{
			int type = 40;
			int channel = 0;
			Match(">>");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__41")]
	private void mT__41()
	{
		try
		{
			int type = 41;
			int channel = 0;
			Match(63);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__42")]
	private void mT__42()
	{
		try
		{
			int type = 42;
			int channel = 0;
			Match(94);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__43")]
	private void mT__43()
	{
		try
		{
			int type = 43;
			int channel = 0;
			Match("and");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__44")]
	private void mT__44()
	{
		try
		{
			int type = 44;
			int channel = 0;
			Match("not");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__45")]
	private void mT__45()
	{
		try
		{
			int type = 45;
			int channel = 0;
			Match("or");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__46")]
	private void mT__46()
	{
		try
		{
			int type = 46;
			int channel = 0;
			Match(124);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__47")]
	private void mT__47()
	{
		try
		{
			int type = 47;
			int channel = 0;
			Match("||");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("T__48")]
	private void mT__48()
	{
		try
		{
			int type = 48;
			int channel = 0;
			Match(126);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("TRUE")]
	private void mTRUE()
	{
		try
		{
			int type = 16;
			int channel = 0;
			Match("true");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("FALSE")]
	private void mFALSE()
	{
		try
		{
			int type = 8;
			int channel = 0;
			Match("false");
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("ID")]
	private void mID()
	{
		try
		{
			int type = 11;
			int channel = 0;
			mLETTER();
			try
			{
				while (true)
				{
					int num = 2;
					try
					{
						int num2 = input.LA(1);
						if ((num2 < 48 || num2 > 57) && (num2 < 65 || num2 > 90))
						{
							switch (num2)
							{
							default:
								goto end_IL_000d;
							case 95:
							case 97:
							case 98:
							case 99:
							case 100:
							case 101:
							case 102:
							case 103:
							case 104:
							case 105:
							case 106:
							case 107:
							case 108:
							case 109:
							case 110:
							case 111:
							case 112:
							case 113:
							case 114:
							case 115:
							case 116:
							case 117:
							case 118:
							case 119:
							case 120:
							case 121:
							case 122:
								break;
							}
						}
						num = 1;
						end_IL_000d:;
					}
					finally
					{
					}
					if (num != 1)
					{
						break;
					}
					input.Consume();
				}
			}
			finally
			{
			}
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("INTEGER")]
	private void mINTEGER()
	{
		try
		{
			int type = 12;
			int channel = 0;
			int num = 0;
			try
			{
				while (true)
				{
					int num2 = 2;
					try
					{
						int num3 = input.LA(1);
						if (num3 >= 48 && num3 <= 57)
						{
							num2 = 1;
						}
					}
					finally
					{
					}
					if (num2 != 1)
					{
						break;
					}
					input.Consume();
					num++;
				}
				if (num < 1)
				{
					EarlyExitException ex = new EarlyExitException(2, input);
					throw ex;
				}
			}
			finally
			{
			}
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("FLOAT")]
	private void mFLOAT()
	{
		try
		{
			int type = 9;
			int channel = 0;
			int num = 2;
			try
			{
				num = dfa7.Predict(input);
			}
			catch (NoViableAltException)
			{
				throw;
			}
			finally
			{
			}
			switch (num)
			{
			case 1:
			{
				try
				{
					while (true)
					{
						int num5 = 2;
						try
						{
							int num6 = input.LA(1);
							if (num6 >= 48 && num6 <= 57)
							{
								num5 = 1;
							}
						}
						finally
						{
						}
						if (num5 != 1)
						{
							break;
						}
						input.Consume();
					}
				}
				finally
				{
				}
				Match(46);
				int num7 = 0;
				try
				{
					while (true)
					{
						int num8 = 2;
						try
						{
							int num9 = input.LA(1);
							if (num9 >= 48 && num9 <= 57)
							{
								num8 = 1;
							}
						}
						finally
						{
						}
						if (num8 != 1)
						{
							break;
						}
						input.Consume();
						num7++;
					}
					if (num7 < 1)
					{
						EarlyExitException ex3 = new EarlyExitException(4, input);
						throw ex3;
					}
				}
				finally
				{
				}
				int num10 = 2;
				try
				{
					try
					{
						int num11 = input.LA(1);
						if (num11 == 69 || num11 == 101)
						{
							num10 = 1;
						}
					}
					finally
					{
					}
					if (num10 == 1)
					{
						mE();
					}
				}
				finally
				{
				}
				break;
			}
			case 2:
			{
				int num2 = 0;
				try
				{
					while (true)
					{
						int num3 = 2;
						try
						{
							int num4 = input.LA(1);
							if (num4 >= 48 && num4 <= 57)
							{
								num3 = 1;
							}
						}
						finally
						{
						}
						if (num3 != 1)
						{
							break;
						}
						input.Consume();
						num2++;
					}
					if (num2 < 1)
					{
						EarlyExitException ex2 = new EarlyExitException(6, input);
						throw ex2;
					}
				}
				finally
				{
				}
				mE();
				break;
			}
			}
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("STRING")]
	private void mSTRING()
	{
		try
		{
			int type = 15;
			int channel = 0;
			Match(39);
			try
			{
				while (true)
				{
					int num = 3;
					try
					{
						int num2 = input.LA(1);
						switch (num2)
						{
						case 92:
							num = 1;
							goto end_IL_000f;
						default:
							if ((num2 >= 40 && num2 <= 91) || (num2 >= 93 && num2 <= 65535))
							{
								break;
							}
							goto end_IL_000f;
						case 32:
						case 33:
						case 34:
						case 35:
						case 36:
						case 37:
						case 38:
							break;
						}
						num = 2;
						end_IL_000f:;
					}
					finally
					{
					}
					switch (num)
					{
					case 1:
						mEscapeSequence();
						continue;
					case 2:
						input.Consume();
						continue;
					}
					break;
				}
			}
			finally
			{
			}
			Match(39);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("DATETIME")]
	private void mDATETIME()
	{
		try
		{
			int type = 4;
			int channel = 0;
			Match(35);
			try
			{
				while (true)
				{
					int num = 2;
					try
					{
						int num2 = input.LA(1);
						if ((num2 >= 0 && num2 <= 34) || (num2 >= 36 && num2 <= 65535))
						{
							num = 1;
						}
					}
					finally
					{
					}
					if (num != 1)
					{
						break;
					}
					input.Consume();
				}
			}
			finally
			{
			}
			Match(35);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("NAME")]
	private void mNAME()
	{
		try
		{
			int type = 14;
			int channel = 0;
			Match(91);
			try
			{
				while (true)
				{
					int num = 2;
					try
					{
						int num2 = input.LA(1);
						if ((num2 >= 0 && num2 <= 92) || (num2 >= 94 && num2 <= 65535))
						{
							num = 1;
						}
					}
					finally
					{
					}
					if (num != 1)
					{
						break;
					}
					input.Consume();
				}
			}
			finally
			{
			}
			Match(93);
			state.type = type;
			state.channel = channel;
		}
		finally
		{
		}
	}

	[GrammarRule("E")]
	private void mE()
	{
		try
		{
			int type = 6;
			int channel = 0;
			if (input.LA(1) == 69 || input.LA(1) == 101)
			{
				input.Consume();
				int num = 2;
				try
				{
					try
					{
						int num2 = input.LA(1);
						if (num2 == 43 || num2 == 45)
						{
							num = 1;
						}
					}
					finally
					{
					}
					if (num == 1)
					{
						input.Consume();
					}
				}
				finally
				{
				}
				int num3 = 0;
				try
				{
					while (true)
					{
						int num4 = 2;
						try
						{
							int num5 = input.LA(1);
							if (num5 >= 48 && num5 <= 57)
							{
								num4 = 1;
							}
						}
						finally
						{
						}
						if (num4 != 1)
						{
							break;
						}
						input.Consume();
						num3++;
					}
					if (num3 < 1)
					{
						EarlyExitException ex = new EarlyExitException(12, input);
						throw ex;
					}
				}
				finally
				{
				}
				state.type = type;
				state.channel = channel;
				return;
			}
			MismatchedSetException ex2 = new MismatchedSetException(null, input);
			Recover(ex2);
			throw ex2;
		}
		finally
		{
		}
	}

	[GrammarRule("LETTER")]
	private void mLETTER()
	{
		try
		{
			if ((input.LA(1) >= 65 && input.LA(1) <= 90) || input.LA(1) == 95 || (input.LA(1) >= 97 && input.LA(1) <= 122))
			{
				input.Consume();
				return;
			}
			MismatchedSetException ex = new MismatchedSetException(null, input);
			Recover(ex);
			throw ex;
		}
		finally
		{
		}
	}

	[GrammarRule("DIGIT")]
	private void mDIGIT()
	{
		try
		{
			if (input.LA(1) >= 48 && input.LA(1) <= 57)
			{
				input.Consume();
				return;
			}
			MismatchedSetException ex = new MismatchedSetException(null, input);
			Recover(ex);
			throw ex;
		}
		finally
		{
		}
	}

	[GrammarRule("EscapeSequence")]
	private void mEscapeSequence()
	{
		try
		{
			Match(92);
			int num = 6;
			try
			{
				try
				{
					switch (input.LA(1))
					{
					case 110:
						num = 1;
						break;
					case 114:
						num = 2;
						break;
					case 116:
						num = 3;
						break;
					case 39:
						num = 4;
						break;
					case 92:
						num = 5;
						break;
					case 117:
						num = 6;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException(string.Empty, 13, 0, input);
						throw ex;
					}
					}
				}
				finally
				{
				}
				switch (num)
				{
				case 1:
					Match(110);
					break;
				case 2:
					Match(114);
					break;
				case 3:
					Match(116);
					break;
				case 4:
					Match(39);
					break;
				case 5:
					Match(92);
					break;
				case 6:
					mUnicodeEscape();
					break;
				}
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("HexDigit")]
	private void mHexDigit()
	{
		try
		{
			if ((input.LA(1) >= 48 && input.LA(1) <= 57) || (input.LA(1) >= 65 && input.LA(1) <= 70) || (input.LA(1) >= 97 && input.LA(1) <= 102))
			{
				input.Consume();
				return;
			}
			MismatchedSetException ex = new MismatchedSetException(null, input);
			Recover(ex);
			throw ex;
		}
		finally
		{
		}
	}

	[GrammarRule("UnicodeEscape")]
	private void mUnicodeEscape()
	{
		try
		{
			Match(117);
			mHexDigit();
			mHexDigit();
			mHexDigit();
			mHexDigit();
		}
		finally
		{
		}
	}

	[GrammarRule("WS")]
	private void mWS()
	{
		try
		{
			int type = 18;
			int num = 0;
			if ((input.LA(1) >= 9 && input.LA(1) <= 10) || (input.LA(1) >= 12 && input.LA(1) <= 13) || input.LA(1) == 32)
			{
				input.Consume();
				num = 99;
				state.type = type;
				state.channel = num;
				return;
			}
			MismatchedSetException ex = new MismatchedSetException(null, input);
			Recover(ex);
			throw ex;
		}
		finally
		{
		}
	}

	public override void mTokens()
	{
		int num = 40;
		try
		{
			num = dfa14.Predict(input);
		}
		catch (NoViableAltException)
		{
			throw;
		}
		finally
		{
		}
		switch (num)
		{
		case 1:
			mT__19();
			break;
		case 2:
			mT__20();
			break;
		case 3:
			mT__21();
			break;
		case 4:
			mT__22();
			break;
		case 5:
			mT__23();
			break;
		case 6:
			mT__24();
			break;
		case 7:
			mT__25();
			break;
		case 8:
			mT__26();
			break;
		case 9:
			mT__27();
			break;
		case 10:
			mT__28();
			break;
		case 11:
			mT__29();
			break;
		case 12:
			mT__30();
			break;
		case 13:
			mT__31();
			break;
		case 14:
			mT__32();
			break;
		case 15:
			mT__33();
			break;
		case 16:
			mT__34();
			break;
		case 17:
			mT__35();
			break;
		case 18:
			mT__36();
			break;
		case 19:
			mT__37();
			break;
		case 20:
			mT__38();
			break;
		case 21:
			mT__39();
			break;
		case 22:
			mT__40();
			break;
		case 23:
			mT__41();
			break;
		case 24:
			mT__42();
			break;
		case 25:
			mT__43();
			break;
		case 26:
			mT__44();
			break;
		case 27:
			mT__45();
			break;
		case 28:
			mT__46();
			break;
		case 29:
			mT__47();
			break;
		case 30:
			mT__48();
			break;
		case 31:
			mTRUE();
			break;
		case 32:
			mFALSE();
			break;
		case 33:
			mID();
			break;
		case 34:
			mINTEGER();
			break;
		case 35:
			mFLOAT();
			break;
		case 36:
			mSTRING();
			break;
		case 37:
			mDATETIME();
			break;
		case 38:
			mNAME();
			break;
		case 39:
			mE();
			break;
		case 40:
			mWS();
			break;
		}
	}

	protected override void InitDFAs()
	{
		base.InitDFAs();
		dfa7 = new DFA7(this);
		dfa14 = new DFA14(this);
	}
}
