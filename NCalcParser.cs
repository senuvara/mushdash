using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NCalc.Domain;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

[GeneratedCode("ANTLR", "3.3.0.7239")]
[CLSCompliant(false)]
public class NCalcParser : Parser
{
	public class ncalcExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class logicalExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class conditionalExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class booleanAndExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class bitwiseOrExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class bitwiseXOrExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class bitwiseAndExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class equalityExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class relationalExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class shiftExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class additiveExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class multiplicativeExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class unaryExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class primaryExpression_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public LogicalExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class value_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public ValueExpression value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class identifier_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public Identifier value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class expressionList_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public List<LogicalExpression> value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	public class arguments_return : ParserRuleReturnScope<IToken>, IAstRuleReturnScope<CommonTree>
	{
		public List<LogicalExpression> value;

		private CommonTree _tree;

		public CommonTree Tree
		{
			get
			{
				return _tree;
			}
			set
			{
				_tree = value;
			}
		}
	}

	private static class Follow
	{
		public static readonly BitSet _logicalExpression_in_ncalcExpression56 = new BitSet(new ulong[1]);

		public static readonly BitSet _EOF_in_ncalcExpression58 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _conditionalExpression_in_logicalExpression78 = new BitSet(new ulong[1]
		{
			2199023255554uL
		});

		public static readonly BitSet _41_in_logicalExpression84 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _conditionalExpression_in_logicalExpression88 = new BitSet(new ulong[1]
		{
			2147483648uL
		});

		public static readonly BitSet _31_in_logicalExpression90 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _conditionalExpression_in_logicalExpression94 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _booleanAndExpression_in_conditionalExpression121 = new BitSet(new ulong[1]
		{
			175921860444162uL
		});

		public static readonly BitSet _set_in_conditionalExpression130 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _conditionalExpression_in_conditionalExpression146 = new BitSet(new ulong[1]
		{
			175921860444162uL
		});

		public static readonly BitSet _bitwiseOrExpression_in_booleanAndExpression180 = new BitSet(new ulong[1]
		{
			8796097216514uL
		});

		public static readonly BitSet _set_in_booleanAndExpression189 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _bitwiseOrExpression_in_booleanAndExpression205 = new BitSet(new ulong[1]
		{
			8796097216514uL
		});

		public static readonly BitSet _bitwiseXOrExpression_in_bitwiseOrExpression237 = new BitSet(new ulong[1]
		{
			70368744177666uL
		});

		public static readonly BitSet _46_in_bitwiseOrExpression246 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _bitwiseOrExpression_in_bitwiseOrExpression256 = new BitSet(new ulong[1]
		{
			70368744177666uL
		});

		public static readonly BitSet _bitwiseAndExpression_in_bitwiseXOrExpression290 = new BitSet(new ulong[1]
		{
			4398046511106uL
		});

		public static readonly BitSet _42_in_bitwiseXOrExpression299 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _bitwiseAndExpression_in_bitwiseXOrExpression309 = new BitSet(new ulong[1]
		{
			4398046511106uL
		});

		public static readonly BitSet _equalityExpression_in_bitwiseAndExpression341 = new BitSet(new ulong[1]
		{
			8388610uL
		});

		public static readonly BitSet _23_in_bitwiseAndExpression350 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _equalityExpression_in_bitwiseAndExpression360 = new BitSet(new ulong[1]
		{
			8388610uL
		});

		public static readonly BitSet _relationalExpression_in_equalityExpression394 = new BitSet(new ulong[1]
		{
			240519217154uL
		});

		public static readonly BitSet _set_in_equalityExpression405 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _set_in_equalityExpression422 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _relationalExpression_in_equalityExpression441 = new BitSet(new ulong[1]
		{
			240519217154uL
		});

		public static readonly BitSet _shiftExpression_in_relationalExpression474 = new BitSet(new ulong[1]
		{
			846108557314uL
		});

		public static readonly BitSet _32_in_relationalExpression485 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _34_in_relationalExpression495 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _38_in_relationalExpression506 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _39_in_relationalExpression516 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _shiftExpression_in_relationalExpression528 = new BitSet(new ulong[1]
		{
			846108557314uL
		});

		public static readonly BitSet _additiveExpression_in_shiftExpression560 = new BitSet(new ulong[1]
		{
			1108101562370uL
		});

		public static readonly BitSet _33_in_shiftExpression571 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _40_in_shiftExpression581 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _additiveExpression_in_shiftExpression593 = new BitSet(new ulong[1]
		{
			1108101562370uL
		});

		public static readonly BitSet _multiplicativeExpression_in_additiveExpression625 = new BitSet(new ulong[1]
		{
			671088642uL
		});

		public static readonly BitSet _27_in_additiveExpression636 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _29_in_additiveExpression646 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _multiplicativeExpression_in_additiveExpression658 = new BitSet(new ulong[1]
		{
			671088642uL
		});

		public static readonly BitSet _unaryExpression_in_multiplicativeExpression690 = new BitSet(new ulong[1]
		{
			1142947842uL
		});

		public static readonly BitSet _26_in_multiplicativeExpression701 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _30_in_multiplicativeExpression711 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _21_in_multiplicativeExpression721 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _unaryExpression_in_multiplicativeExpression733 = new BitSet(new ulong[1]
		{
			1142947842uL
		});

		public static readonly BitSet _primaryExpression_in_unaryExpression760 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _set_in_unaryExpression771 = new BitSet(new ulong[1]
		{
			16898832uL
		});

		public static readonly BitSet _primaryExpression_in_unaryExpression779 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _48_in_unaryExpression791 = new BitSet(new ulong[1]
		{
			16898832uL
		});

		public static readonly BitSet _primaryExpression_in_unaryExpression794 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _29_in_unaryExpression805 = new BitSet(new ulong[1]
		{
			16898832uL
		});

		public static readonly BitSet _primaryExpression_in_unaryExpression807 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _24_in_primaryExpression829 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _logicalExpression_in_primaryExpression831 = new BitSet(new ulong[1]
		{
			33554432uL
		});

		public static readonly BitSet _25_in_primaryExpression833 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _value_in_primaryExpression843 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _identifier_in_primaryExpression851 = new BitSet(new ulong[1]
		{
			16777218uL
		});

		public static readonly BitSet _arguments_in_primaryExpression856 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _INTEGER_in_value876 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _FLOAT_in_value884 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _STRING_in_value892 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _DATETIME_in_value901 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _TRUE_in_value908 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _FALSE_in_value916 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _ID_in_identifier934 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _NAME_in_identifier942 = new BitSet(new ulong[1]
		{
			2uL
		});

		public static readonly BitSet _logicalExpression_in_expressionList966 = new BitSet(new ulong[1]
		{
			268435458uL
		});

		public static readonly BitSet _28_in_expressionList973 = new BitSet(new ulong[1]
		{
			299067717049104uL
		});

		public static readonly BitSet _logicalExpression_in_expressionList977 = new BitSet(new ulong[1]
		{
			268435458uL
		});

		public static readonly BitSet _24_in_arguments1006 = new BitSet(new ulong[1]
		{
			299067750603536uL
		});

		public static readonly BitSet _expressionList_in_arguments1010 = new BitSet(new ulong[1]
		{
			33554432uL
		});

		public static readonly BitSet _25_in_arguments1017 = new BitSet(new ulong[1]
		{
			2uL
		});
	}

	internal static readonly string[] tokenNames = new string[49]
	{
		"<invalid>",
		"<EOR>",
		"<DOWN>",
		"<UP>",
		"DATETIME",
		"DIGIT",
		"E",
		"EscapeSequence",
		"FALSE",
		"FLOAT",
		"HexDigit",
		"ID",
		"INTEGER",
		"LETTER",
		"NAME",
		"STRING",
		"TRUE",
		"UnicodeEscape",
		"WS",
		"'!'",
		"'!='",
		"'%'",
		"'&&'",
		"'&'",
		"'('",
		"')'",
		"'*'",
		"'+'",
		"','",
		"'-'",
		"'/'",
		"':'",
		"'<'",
		"'<<'",
		"'<='",
		"'<>'",
		"'='",
		"'=='",
		"'>'",
		"'>='",
		"'>>'",
		"'?'",
		"'^'",
		"'and'",
		"'not'",
		"'or'",
		"'|'",
		"'||'",
		"'~'"
	};

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

	private ITreeAdaptor adaptor;

	private const char BS = '\\';

	private static NumberFormatInfo numberFormatInfo = new NumberFormatInfo();

	public ITreeAdaptor TreeAdaptor
	{
		get
		{
			return adaptor;
		}
		set
		{
			adaptor = value;
		}
	}

	public override string[] TokenNames => tokenNames;

	public override string GrammarFileName => "C:\\Users\\sebros\\My Projects\\NCalc\\Grammar\\NCalc.g";

	public List<string> Errors
	{
		get;
		private set;
	}

	public NCalcParser(ITokenStream input)
		: this(input, new RecognizerSharedState())
	{
	}

	public NCalcParser(ITokenStream input, RecognizerSharedState state)
		: base(input, state)
	{
		ITreeAdaptor treeAdaptor = null;
		TreeAdaptor = (treeAdaptor ?? new CommonTreeAdaptor());
	}

	private string extractString(string text)
	{
		StringBuilder stringBuilder = new StringBuilder(text);
		int startIndex = 1;
		int num = -1;
		while ((num = stringBuilder.ToString().IndexOf('\\', startIndex)) != -1)
		{
			char c = stringBuilder[num + 1];
			switch (c)
			{
			case 'u':
			{
				string value = string.Concat(stringBuilder[num + 4], stringBuilder[num + 5]);
				string value2 = string.Concat(stringBuilder[num + 2], stringBuilder[num + 3]);
				char value3 = Encoding.Unicode.GetChars(new byte[2]
				{
					Convert.ToByte(value, 16),
					Convert.ToByte(value2, 16)
				})[0];
				stringBuilder.Remove(num, 6).Insert(num, value3);
				break;
			}
			case 'n':
				stringBuilder.Remove(num, 2).Insert(num, '\n');
				break;
			case 'r':
				stringBuilder.Remove(num, 2).Insert(num, '\r');
				break;
			case 't':
				stringBuilder.Remove(num, 2).Insert(num, '\t');
				break;
			case '\'':
				stringBuilder.Remove(num, 2).Insert(num, '\'');
				break;
			case '\\':
				stringBuilder.Remove(num, 2).Insert(num, '\\');
				break;
			default:
				throw new RecognitionException("Unvalid escape sequence: \\" + c);
			}
			startIndex = num + 1;
		}
		stringBuilder.Remove(0, 1);
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		return stringBuilder.ToString();
	}

	public override void DisplayRecognitionError(string[] tokenNames, RecognitionException e)
	{
		base.DisplayRecognitionError(tokenNames, e);
		if (Errors == null)
		{
			Errors = new List<string>();
		}
		string errorHeader = GetErrorHeader(e);
		string errorMessage = GetErrorMessage(e, tokenNames);
		Errors.Add(errorMessage + " at " + errorHeader);
	}

	[GrammarRule("ncalcExpression")]
	public ncalcExpression_return ncalcExpression()
	{
		ncalcExpression_return ncalcExpression_return = new ncalcExpression_return();
		ncalcExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		logicalExpression_return logicalExpression_return = null;
		CommonTree commonTree2 = null;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._logicalExpression_in_ncalcExpression56);
				logicalExpression_return = logicalExpression();
				PopFollow();
				adaptor.AddChild(commonTree, logicalExpression_return.Tree);
				token = (IToken)Match(input, -1, Follow._EOF_in_ncalcExpression58);
				ncalcExpression_return.value = logicalExpression_return?.value;
				ncalcExpression_return.Stop = input.LT(-1);
				ncalcExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(ncalcExpression_return.Tree, ncalcExpression_return.Start, ncalcExpression_return.Stop);
				return ncalcExpression_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				ncalcExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, ncalcExpression_return.Start, input.LT(-1), ex);
				return ncalcExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("logicalExpression")]
	private logicalExpression_return logicalExpression()
	{
		logicalExpression_return logicalExpression_return = new logicalExpression_return();
		logicalExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		conditionalExpression_return conditionalExpression_return = null;
		conditionalExpression_return conditionalExpression_return2 = null;
		conditionalExpression_return conditionalExpression_return3 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._conditionalExpression_in_logicalExpression78);
				conditionalExpression_return = conditionalExpression();
				PopFollow();
				adaptor.AddChild(commonTree, conditionalExpression_return.Tree);
				logicalExpression_return.value = conditionalExpression_return?.value;
				int num = 2;
				try
				{
					try
					{
						int num2 = input.LA(1);
						if (num2 == 41)
						{
							num = 1;
						}
					}
					finally
					{
					}
					if (num == 1)
					{
						token = (IToken)Match(input, 41, Follow._41_in_logicalExpression84);
						commonTree2 = (CommonTree)adaptor.Create(token);
						adaptor.AddChild(commonTree, commonTree2);
						PushFollow(Follow._conditionalExpression_in_logicalExpression88);
						conditionalExpression_return2 = conditionalExpression();
						PopFollow();
						adaptor.AddChild(commonTree, conditionalExpression_return2.Tree);
						token2 = (IToken)Match(input, 31, Follow._31_in_logicalExpression90);
						commonTree3 = (CommonTree)adaptor.Create(token2);
						adaptor.AddChild(commonTree, commonTree3);
						PushFollow(Follow._conditionalExpression_in_logicalExpression94);
						conditionalExpression_return3 = conditionalExpression();
						PopFollow();
						adaptor.AddChild(commonTree, conditionalExpression_return3.Tree);
						logicalExpression_return.value = new TernaryExpression(conditionalExpression_return?.value, conditionalExpression_return2?.value, conditionalExpression_return3?.value);
					}
				}
				finally
				{
				}
				logicalExpression_return.Stop = input.LT(-1);
				logicalExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(logicalExpression_return.Tree, logicalExpression_return.Start, logicalExpression_return.Stop);
				return logicalExpression_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				logicalExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, logicalExpression_return.Start, input.LT(-1), ex);
				return logicalExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("conditionalExpression")]
	private conditionalExpression_return conditionalExpression()
	{
		conditionalExpression_return conditionalExpression_return = new conditionalExpression_return();
		conditionalExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		booleanAndExpression_return booleanAndExpression_return = null;
		conditionalExpression_return conditionalExpression_return2 = null;
		CommonTree commonTree2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._booleanAndExpression_in_conditionalExpression121);
				booleanAndExpression_return = booleanAndExpression();
				PopFollow();
				adaptor.AddChild(commonTree, booleanAndExpression_return.Tree);
				conditionalExpression_return.value = booleanAndExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 45 || num2 == 47)
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
						token = input.LT(1);
						if (input.LA(1) == 45 || input.LA(1) == 47)
						{
							input.Consume();
							adaptor.AddChild(commonTree, (CommonTree)adaptor.Create(token));
							state.errorRecovery = false;
							binaryExpressionType = BinaryExpressionType.Or;
							PushFollow(Follow._conditionalExpression_in_conditionalExpression146);
							conditionalExpression_return2 = conditionalExpression();
							PopFollow();
							adaptor.AddChild(commonTree, conditionalExpression_return2.Tree);
							conditionalExpression_return.value = new BinaryExpression(binaryExpressionType, conditionalExpression_return.value, conditionalExpression_return2?.value);
							continue;
						}
						MismatchedSetException ex = new MismatchedSetException(null, input);
						throw ex;
					}
				}
				finally
				{
				}
				conditionalExpression_return.Stop = input.LT(-1);
				conditionalExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(conditionalExpression_return.Tree, conditionalExpression_return.Start, conditionalExpression_return.Stop);
				return conditionalExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				conditionalExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, conditionalExpression_return.Start, input.LT(-1), ex2);
				return conditionalExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("booleanAndExpression")]
	private booleanAndExpression_return booleanAndExpression()
	{
		booleanAndExpression_return booleanAndExpression_return = new booleanAndExpression_return();
		booleanAndExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		bitwiseOrExpression_return bitwiseOrExpression_return = null;
		bitwiseOrExpression_return bitwiseOrExpression_return2 = null;
		CommonTree commonTree2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._bitwiseOrExpression_in_booleanAndExpression180);
				bitwiseOrExpression_return = bitwiseOrExpression();
				PopFollow();
				adaptor.AddChild(commonTree, bitwiseOrExpression_return.Tree);
				booleanAndExpression_return.value = bitwiseOrExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 22 || num2 == 43)
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
						token = input.LT(1);
						if (input.LA(1) == 22 || input.LA(1) == 43)
						{
							input.Consume();
							adaptor.AddChild(commonTree, (CommonTree)adaptor.Create(token));
							state.errorRecovery = false;
							binaryExpressionType = BinaryExpressionType.And;
							PushFollow(Follow._bitwiseOrExpression_in_booleanAndExpression205);
							bitwiseOrExpression_return2 = bitwiseOrExpression();
							PopFollow();
							adaptor.AddChild(commonTree, bitwiseOrExpression_return2.Tree);
							booleanAndExpression_return.value = new BinaryExpression(binaryExpressionType, booleanAndExpression_return.value, bitwiseOrExpression_return2?.value);
							continue;
						}
						MismatchedSetException ex = new MismatchedSetException(null, input);
						throw ex;
					}
				}
				finally
				{
				}
				booleanAndExpression_return.Stop = input.LT(-1);
				booleanAndExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(booleanAndExpression_return.Tree, booleanAndExpression_return.Start, booleanAndExpression_return.Stop);
				return booleanAndExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				booleanAndExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, booleanAndExpression_return.Start, input.LT(-1), ex2);
				return booleanAndExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("bitwiseOrExpression")]
	private bitwiseOrExpression_return bitwiseOrExpression()
	{
		bitwiseOrExpression_return bitwiseOrExpression_return = new bitwiseOrExpression_return();
		bitwiseOrExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		bitwiseXOrExpression_return bitwiseXOrExpression_return = null;
		bitwiseOrExpression_return bitwiseOrExpression_return2 = null;
		CommonTree commonTree2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._bitwiseXOrExpression_in_bitwiseOrExpression237);
				bitwiseXOrExpression_return = bitwiseXOrExpression();
				PopFollow();
				adaptor.AddChild(commonTree, bitwiseXOrExpression_return.Tree);
				bitwiseOrExpression_return.value = bitwiseXOrExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 46)
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
						token = (IToken)Match(input, 46, Follow._46_in_bitwiseOrExpression246);
						commonTree2 = (CommonTree)adaptor.Create(token);
						adaptor.AddChild(commonTree, commonTree2);
						binaryExpressionType = BinaryExpressionType.BitwiseOr;
						PushFollow(Follow._bitwiseOrExpression_in_bitwiseOrExpression256);
						bitwiseOrExpression_return2 = bitwiseOrExpression();
						PopFollow();
						adaptor.AddChild(commonTree, bitwiseOrExpression_return2.Tree);
						bitwiseOrExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseOrExpression_return.value, bitwiseOrExpression_return2?.value);
					}
				}
				finally
				{
				}
				bitwiseOrExpression_return.Stop = input.LT(-1);
				bitwiseOrExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(bitwiseOrExpression_return.Tree, bitwiseOrExpression_return.Start, bitwiseOrExpression_return.Stop);
				return bitwiseOrExpression_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseOrExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, bitwiseOrExpression_return.Start, input.LT(-1), ex);
				return bitwiseOrExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("bitwiseXOrExpression")]
	private bitwiseXOrExpression_return bitwiseXOrExpression()
	{
		bitwiseXOrExpression_return bitwiseXOrExpression_return = new bitwiseXOrExpression_return();
		bitwiseXOrExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		bitwiseAndExpression_return bitwiseAndExpression_return = null;
		bitwiseAndExpression_return bitwiseAndExpression_return2 = null;
		CommonTree commonTree2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._bitwiseAndExpression_in_bitwiseXOrExpression290);
				bitwiseAndExpression_return = bitwiseAndExpression();
				PopFollow();
				adaptor.AddChild(commonTree, bitwiseAndExpression_return.Tree);
				bitwiseXOrExpression_return.value = bitwiseAndExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 42)
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
						token = (IToken)Match(input, 42, Follow._42_in_bitwiseXOrExpression299);
						commonTree2 = (CommonTree)adaptor.Create(token);
						adaptor.AddChild(commonTree, commonTree2);
						binaryExpressionType = BinaryExpressionType.BitwiseXOr;
						PushFollow(Follow._bitwiseAndExpression_in_bitwiseXOrExpression309);
						bitwiseAndExpression_return2 = bitwiseAndExpression();
						PopFollow();
						adaptor.AddChild(commonTree, bitwiseAndExpression_return2.Tree);
						bitwiseXOrExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseXOrExpression_return.value, bitwiseAndExpression_return2?.value);
					}
				}
				finally
				{
				}
				bitwiseXOrExpression_return.Stop = input.LT(-1);
				bitwiseXOrExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(bitwiseXOrExpression_return.Tree, bitwiseXOrExpression_return.Start, bitwiseXOrExpression_return.Stop);
				return bitwiseXOrExpression_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseXOrExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, bitwiseXOrExpression_return.Start, input.LT(-1), ex);
				return bitwiseXOrExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("bitwiseAndExpression")]
	private bitwiseAndExpression_return bitwiseAndExpression()
	{
		bitwiseAndExpression_return bitwiseAndExpression_return = new bitwiseAndExpression_return();
		bitwiseAndExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		equalityExpression_return equalityExpression_return = null;
		equalityExpression_return equalityExpression_return2 = null;
		CommonTree commonTree2 = null;
		BinaryExpressionType binaryExpressionType = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._equalityExpression_in_bitwiseAndExpression341);
				equalityExpression_return = equalityExpression();
				PopFollow();
				adaptor.AddChild(commonTree, equalityExpression_return.Tree);
				bitwiseAndExpression_return.value = equalityExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 23)
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
						token = (IToken)Match(input, 23, Follow._23_in_bitwiseAndExpression350);
						commonTree2 = (CommonTree)adaptor.Create(token);
						adaptor.AddChild(commonTree, commonTree2);
						binaryExpressionType = BinaryExpressionType.BitwiseAnd;
						PushFollow(Follow._equalityExpression_in_bitwiseAndExpression360);
						equalityExpression_return2 = equalityExpression();
						PopFollow();
						adaptor.AddChild(commonTree, equalityExpression_return2.Tree);
						bitwiseAndExpression_return.value = new BinaryExpression(binaryExpressionType, bitwiseAndExpression_return.value, equalityExpression_return2?.value);
					}
				}
				finally
				{
				}
				bitwiseAndExpression_return.Stop = input.LT(-1);
				bitwiseAndExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(bitwiseAndExpression_return.Tree, bitwiseAndExpression_return.Start, bitwiseAndExpression_return.Stop);
				return bitwiseAndExpression_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				bitwiseAndExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, bitwiseAndExpression_return.Start, input.LT(-1), ex);
				return bitwiseAndExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("equalityExpression")]
	private equalityExpression_return equalityExpression()
	{
		equalityExpression_return equalityExpression_return = new equalityExpression_return();
		equalityExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		relationalExpression_return relationalExpression_return = null;
		relationalExpression_return relationalExpression_return2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._relationalExpression_in_equalityExpression394);
				relationalExpression_return = relationalExpression();
				PopFollow();
				adaptor.AddChild(commonTree, relationalExpression_return.Tree);
				equalityExpression_return.value = relationalExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							switch (input.LA(1))
							{
							case 20:
							case 35:
							case 36:
							case 37:
								num = 1;
								break;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num2 = 2;
						try
						{
							try
							{
								int num3 = input.LA(1);
								if (num3 >= 36 && num3 <= 37)
								{
									num2 = 1;
								}
								else
								{
									if (num3 != 20 && num3 != 35)
									{
										NoViableAltException ex = new NoViableAltException(string.Empty, 7, 0, input);
										throw ex;
									}
									num2 = 2;
								}
							}
							finally
							{
							}
							switch (num2)
							{
							case 1:
							{
								token = input.LT(1);
								if (input.LA(1) >= 36 && input.LA(1) <= 37)
								{
									input.Consume();
									adaptor.AddChild(commonTree, (CommonTree)adaptor.Create(token));
									state.errorRecovery = false;
									type = BinaryExpressionType.Equal;
									break;
								}
								MismatchedSetException ex3 = new MismatchedSetException(null, input);
								throw ex3;
							}
							case 2:
							{
								token2 = input.LT(1);
								if (input.LA(1) == 20 || input.LA(1) == 35)
								{
									input.Consume();
									adaptor.AddChild(commonTree, (CommonTree)adaptor.Create(token2));
									state.errorRecovery = false;
									type = BinaryExpressionType.NotEqual;
									break;
								}
								MismatchedSetException ex2 = new MismatchedSetException(null, input);
								throw ex2;
							}
							}
						}
						finally
						{
						}
						PushFollow(Follow._relationalExpression_in_equalityExpression441);
						relationalExpression_return2 = relationalExpression();
						PopFollow();
						adaptor.AddChild(commonTree, relationalExpression_return2.Tree);
						equalityExpression_return.value = new BinaryExpression(type, equalityExpression_return.value, relationalExpression_return2?.value);
					}
				}
				finally
				{
				}
				equalityExpression_return.Stop = input.LT(-1);
				equalityExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(equalityExpression_return.Tree, equalityExpression_return.Start, equalityExpression_return.Stop);
				return equalityExpression_return;
			}
			catch (RecognitionException ex4)
			{
				ReportError(ex4);
				Recover(input, ex4);
				equalityExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, equalityExpression_return.Start, input.LT(-1), ex4);
				return equalityExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("relationalExpression")]
	private relationalExpression_return relationalExpression()
	{
		relationalExpression_return relationalExpression_return = new relationalExpression_return();
		relationalExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		IToken token4 = null;
		shiftExpression_return shiftExpression_return = null;
		shiftExpression_return shiftExpression_return2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		CommonTree commonTree4 = null;
		CommonTree commonTree5 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._shiftExpression_in_relationalExpression474);
				shiftExpression_return = shiftExpression();
				PopFollow();
				adaptor.AddChild(commonTree, shiftExpression_return.Tree);
				relationalExpression_return.value = shiftExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							switch (input.LA(1))
							{
							case 32:
							case 34:
							case 38:
							case 39:
								num = 1;
								break;
							}
						}
						finally
						{
						}
						if (num != 1)
						{
							break;
						}
						int num2 = 4;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 32:
									num2 = 1;
									break;
								case 34:
									num2 = 2;
									break;
								case 38:
									num2 = 3;
									break;
								case 39:
									num2 = 4;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException(string.Empty, 9, 0, input);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num2)
							{
							case 1:
								token = (IToken)Match(input, 32, Follow._32_in_relationalExpression485);
								commonTree2 = (CommonTree)adaptor.Create(token);
								adaptor.AddChild(commonTree, commonTree2);
								type = BinaryExpressionType.Lesser;
								break;
							case 2:
								token2 = (IToken)Match(input, 34, Follow._34_in_relationalExpression495);
								commonTree3 = (CommonTree)adaptor.Create(token2);
								adaptor.AddChild(commonTree, commonTree3);
								type = BinaryExpressionType.LesserOrEqual;
								break;
							case 3:
								token3 = (IToken)Match(input, 38, Follow._38_in_relationalExpression506);
								commonTree4 = (CommonTree)adaptor.Create(token3);
								adaptor.AddChild(commonTree, commonTree4);
								type = BinaryExpressionType.Greater;
								break;
							case 4:
								token4 = (IToken)Match(input, 39, Follow._39_in_relationalExpression516);
								commonTree5 = (CommonTree)adaptor.Create(token4);
								adaptor.AddChild(commonTree, commonTree5);
								type = BinaryExpressionType.GreaterOrEqual;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._shiftExpression_in_relationalExpression528);
						shiftExpression_return2 = shiftExpression();
						PopFollow();
						adaptor.AddChild(commonTree, shiftExpression_return2.Tree);
						relationalExpression_return.value = new BinaryExpression(type, relationalExpression_return.value, shiftExpression_return2?.value);
					}
				}
				finally
				{
				}
				relationalExpression_return.Stop = input.LT(-1);
				relationalExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(relationalExpression_return.Tree, relationalExpression_return.Start, relationalExpression_return.Stop);
				return relationalExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				relationalExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, relationalExpression_return.Start, input.LT(-1), ex2);
				return relationalExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("shiftExpression")]
	private shiftExpression_return shiftExpression()
	{
		shiftExpression_return shiftExpression_return = new shiftExpression_return();
		shiftExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		additiveExpression_return additiveExpression_return = null;
		additiveExpression_return additiveExpression_return2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._additiveExpression_in_shiftExpression560);
				additiveExpression_return = additiveExpression();
				PopFollow();
				adaptor.AddChild(commonTree, additiveExpression_return.Tree);
				shiftExpression_return.value = additiveExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 33 || num2 == 40)
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
						int num3 = 2;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 33:
									num3 = 1;
									break;
								case 40:
									num3 = 2;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException(string.Empty, 11, 0, input);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 33, Follow._33_in_shiftExpression571);
								commonTree2 = (CommonTree)adaptor.Create(token);
								adaptor.AddChild(commonTree, commonTree2);
								type = BinaryExpressionType.LeftShift;
								break;
							case 2:
								token2 = (IToken)Match(input, 40, Follow._40_in_shiftExpression581);
								commonTree3 = (CommonTree)adaptor.Create(token2);
								adaptor.AddChild(commonTree, commonTree3);
								type = BinaryExpressionType.RightShift;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._additiveExpression_in_shiftExpression593);
						additiveExpression_return2 = additiveExpression();
						PopFollow();
						adaptor.AddChild(commonTree, additiveExpression_return2.Tree);
						shiftExpression_return.value = new BinaryExpression(type, shiftExpression_return.value, additiveExpression_return2?.value);
					}
				}
				finally
				{
				}
				shiftExpression_return.Stop = input.LT(-1);
				shiftExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(shiftExpression_return.Tree, shiftExpression_return.Start, shiftExpression_return.Stop);
				return shiftExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				shiftExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, shiftExpression_return.Start, input.LT(-1), ex2);
				return shiftExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("additiveExpression")]
	private additiveExpression_return additiveExpression()
	{
		additiveExpression_return additiveExpression_return = new additiveExpression_return();
		additiveExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		multiplicativeExpression_return multiplicativeExpression_return = null;
		multiplicativeExpression_return multiplicativeExpression_return2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._multiplicativeExpression_in_additiveExpression625);
				multiplicativeExpression_return = multiplicativeExpression();
				PopFollow();
				adaptor.AddChild(commonTree, multiplicativeExpression_return.Tree);
				additiveExpression_return.value = multiplicativeExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 27 || num2 == 29)
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
						int num3 = 2;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 27:
									num3 = 1;
									break;
								case 29:
									num3 = 2;
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
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 27, Follow._27_in_additiveExpression636);
								commonTree2 = (CommonTree)adaptor.Create(token);
								adaptor.AddChild(commonTree, commonTree2);
								type = BinaryExpressionType.Plus;
								break;
							case 2:
								token2 = (IToken)Match(input, 29, Follow._29_in_additiveExpression646);
								commonTree3 = (CommonTree)adaptor.Create(token2);
								adaptor.AddChild(commonTree, commonTree3);
								type = BinaryExpressionType.Minus;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._multiplicativeExpression_in_additiveExpression658);
						multiplicativeExpression_return2 = multiplicativeExpression();
						PopFollow();
						adaptor.AddChild(commonTree, multiplicativeExpression_return2.Tree);
						additiveExpression_return.value = new BinaryExpression(type, additiveExpression_return.value, multiplicativeExpression_return2?.value);
					}
				}
				finally
				{
				}
				additiveExpression_return.Stop = input.LT(-1);
				additiveExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(additiveExpression_return.Tree, additiveExpression_return.Start, additiveExpression_return.Stop);
				return additiveExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				additiveExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, additiveExpression_return.Start, input.LT(-1), ex2);
				return additiveExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("multiplicativeExpression")]
	private multiplicativeExpression_return multiplicativeExpression()
	{
		multiplicativeExpression_return multiplicativeExpression_return = new multiplicativeExpression_return();
		multiplicativeExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		unaryExpression_return unaryExpression_return = null;
		unaryExpression_return unaryExpression_return2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		CommonTree commonTree4 = null;
		BinaryExpressionType type = BinaryExpressionType.Unknown;
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._unaryExpression_in_multiplicativeExpression690);
				unaryExpression_return = unaryExpression();
				PopFollow();
				adaptor.AddChild(commonTree, unaryExpression_return.Tree);
				multiplicativeExpression_return.value = unaryExpression_return?.value;
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 21 || num2 == 26 || num2 == 30)
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
						int num3 = 3;
						try
						{
							try
							{
								switch (input.LA(1))
								{
								case 26:
									num3 = 1;
									break;
								case 30:
									num3 = 2;
									break;
								case 21:
									num3 = 3;
									break;
								default:
								{
									NoViableAltException ex = new NoViableAltException(string.Empty, 15, 0, input);
									throw ex;
								}
								}
							}
							finally
							{
							}
							switch (num3)
							{
							case 1:
								token = (IToken)Match(input, 26, Follow._26_in_multiplicativeExpression701);
								commonTree2 = (CommonTree)adaptor.Create(token);
								adaptor.AddChild(commonTree, commonTree2);
								type = BinaryExpressionType.Times;
								break;
							case 2:
								token2 = (IToken)Match(input, 30, Follow._30_in_multiplicativeExpression711);
								commonTree3 = (CommonTree)adaptor.Create(token2);
								adaptor.AddChild(commonTree, commonTree3);
								type = BinaryExpressionType.Div;
								break;
							case 3:
								token3 = (IToken)Match(input, 21, Follow._21_in_multiplicativeExpression721);
								commonTree4 = (CommonTree)adaptor.Create(token3);
								adaptor.AddChild(commonTree, commonTree4);
								type = BinaryExpressionType.Modulo;
								break;
							}
						}
						finally
						{
						}
						PushFollow(Follow._unaryExpression_in_multiplicativeExpression733);
						unaryExpression_return2 = unaryExpression();
						PopFollow();
						adaptor.AddChild(commonTree, unaryExpression_return2.Tree);
						multiplicativeExpression_return.value = new BinaryExpression(type, multiplicativeExpression_return.value, unaryExpression_return2?.value);
					}
				}
				finally
				{
				}
				multiplicativeExpression_return.Stop = input.LT(-1);
				multiplicativeExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(multiplicativeExpression_return.Tree, multiplicativeExpression_return.Start, multiplicativeExpression_return.Stop);
				return multiplicativeExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				multiplicativeExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, multiplicativeExpression_return.Start, input.LT(-1), ex2);
				return multiplicativeExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("unaryExpression")]
	private unaryExpression_return unaryExpression()
	{
		unaryExpression_return unaryExpression_return = new unaryExpression_return();
		unaryExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		primaryExpression_return primaryExpression_return = null;
		primaryExpression_return primaryExpression_return2 = null;
		primaryExpression_return primaryExpression_return3 = null;
		primaryExpression_return primaryExpression_return4 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		CommonTree commonTree4 = null;
		try
		{
			try
			{
				int num = 4;
				try
				{
					switch (input.LA(1))
					{
					case 4:
					case 8:
					case 9:
					case 11:
					case 12:
					case 14:
					case 15:
					case 16:
					case 24:
						num = 1;
						break;
					case 19:
					case 44:
						num = 2;
						break;
					case 48:
						num = 3;
						break;
					case 29:
						num = 4;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException(string.Empty, 17, 0, input);
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
					commonTree = (CommonTree)adaptor.Nil();
					PushFollow(Follow._primaryExpression_in_unaryExpression760);
					primaryExpression_return = primaryExpression();
					PopFollow();
					adaptor.AddChild(commonTree, primaryExpression_return.Tree);
					unaryExpression_return.value = primaryExpression_return?.value;
					break;
				case 2:
				{
					commonTree = (CommonTree)adaptor.Nil();
					token = input.LT(1);
					if (input.LA(1) == 19 || input.LA(1) == 44)
					{
						input.Consume();
						adaptor.AddChild(commonTree, (CommonTree)adaptor.Create(token));
						state.errorRecovery = false;
						PushFollow(Follow._primaryExpression_in_unaryExpression779);
						primaryExpression_return2 = primaryExpression();
						PopFollow();
						adaptor.AddChild(commonTree, primaryExpression_return2.Tree);
						unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.Not, primaryExpression_return2?.value);
						break;
					}
					MismatchedSetException ex2 = new MismatchedSetException(null, input);
					throw ex2;
				}
				case 3:
					commonTree = (CommonTree)adaptor.Nil();
					token2 = (IToken)Match(input, 48, Follow._48_in_unaryExpression791);
					commonTree3 = (CommonTree)adaptor.Create(token2);
					adaptor.AddChild(commonTree, commonTree3);
					PushFollow(Follow._primaryExpression_in_unaryExpression794);
					primaryExpression_return3 = primaryExpression();
					PopFollow();
					adaptor.AddChild(commonTree, primaryExpression_return3.Tree);
					unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.BitwiseNot, primaryExpression_return3?.value);
					break;
				case 4:
					commonTree = (CommonTree)adaptor.Nil();
					token3 = (IToken)Match(input, 29, Follow._29_in_unaryExpression805);
					commonTree4 = (CommonTree)adaptor.Create(token3);
					adaptor.AddChild(commonTree, commonTree4);
					PushFollow(Follow._primaryExpression_in_unaryExpression807);
					primaryExpression_return4 = primaryExpression();
					PopFollow();
					adaptor.AddChild(commonTree, primaryExpression_return4.Tree);
					unaryExpression_return.value = new UnaryExpression(UnaryExpressionType.Negate, primaryExpression_return4?.value);
					break;
				}
				unaryExpression_return.Stop = input.LT(-1);
				unaryExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(unaryExpression_return.Tree, unaryExpression_return.Start, unaryExpression_return.Stop);
				return unaryExpression_return;
			}
			catch (RecognitionException ex3)
			{
				ReportError(ex3);
				Recover(input, ex3);
				unaryExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, unaryExpression_return.Start, input.LT(-1), ex3);
				return unaryExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("primaryExpression")]
	private primaryExpression_return primaryExpression()
	{
		primaryExpression_return primaryExpression_return = new primaryExpression_return();
		primaryExpression_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		value_return value_return = null;
		logicalExpression_return logicalExpression_return = null;
		identifier_return identifier_return = null;
		arguments_return arguments_return = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		try
		{
			try
			{
				int num = 3;
				try
				{
					switch (input.LA(1))
					{
					case 24:
						num = 1;
						break;
					case 4:
					case 8:
					case 9:
					case 12:
					case 15:
					case 16:
						num = 2;
						break;
					case 11:
					case 14:
						num = 3;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException(string.Empty, 19, 0, input);
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
					commonTree = (CommonTree)adaptor.Nil();
					token = (IToken)Match(input, 24, Follow._24_in_primaryExpression829);
					commonTree2 = (CommonTree)adaptor.Create(token);
					adaptor.AddChild(commonTree, commonTree2);
					PushFollow(Follow._logicalExpression_in_primaryExpression831);
					logicalExpression_return = logicalExpression();
					PopFollow();
					adaptor.AddChild(commonTree, logicalExpression_return.Tree);
					token2 = (IToken)Match(input, 25, Follow._25_in_primaryExpression833);
					commonTree3 = (CommonTree)adaptor.Create(token2);
					adaptor.AddChild(commonTree, commonTree3);
					primaryExpression_return.value = logicalExpression_return?.value;
					break;
				case 2:
					commonTree = (CommonTree)adaptor.Nil();
					PushFollow(Follow._value_in_primaryExpression843);
					value_return = value();
					PopFollow();
					adaptor.AddChild(commonTree, value_return.Tree);
					primaryExpression_return.value = value_return?.value;
					break;
				case 3:
				{
					commonTree = (CommonTree)adaptor.Nil();
					PushFollow(Follow._identifier_in_primaryExpression851);
					identifier_return = identifier();
					PopFollow();
					adaptor.AddChild(commonTree, identifier_return.Tree);
					primaryExpression_return.value = identifier_return?.value;
					int num2 = 2;
					try
					{
						try
						{
							int num3 = input.LA(1);
							if (num3 == 24)
							{
								num2 = 1;
							}
						}
						finally
						{
						}
						if (num2 == 1)
						{
							PushFollow(Follow._arguments_in_primaryExpression856);
							arguments_return = arguments();
							PopFollow();
							adaptor.AddChild(commonTree, arguments_return.Tree);
							primaryExpression_return.value = new Function(identifier_return?.value, (arguments_return?.value).ToArray());
						}
					}
					finally
					{
					}
					break;
				}
				}
				primaryExpression_return.Stop = input.LT(-1);
				primaryExpression_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(primaryExpression_return.Tree, primaryExpression_return.Start, primaryExpression_return.Stop);
				return primaryExpression_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				primaryExpression_return.Tree = (CommonTree)adaptor.ErrorNode(input, primaryExpression_return.Start, input.LT(-1), ex2);
				return primaryExpression_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("value")]
	private value_return value()
	{
		value_return value_return = new value_return();
		value_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		IToken token3 = null;
		IToken token4 = null;
		IToken token5 = null;
		IToken token6 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		CommonTree commonTree4 = null;
		CommonTree commonTree5 = null;
		CommonTree commonTree6 = null;
		CommonTree commonTree7 = null;
		try
		{
			try
			{
				int num = 6;
				try
				{
					switch (input.LA(1))
					{
					case 12:
						num = 1;
						break;
					case 9:
						num = 2;
						break;
					case 15:
						num = 3;
						break;
					case 4:
						num = 4;
						break;
					case 16:
						num = 5;
						break;
					case 8:
						num = 6;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException(string.Empty, 20, 0, input);
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
					commonTree = (CommonTree)adaptor.Nil();
					token = (IToken)Match(input, 12, Follow._INTEGER_in_value876);
					commonTree2 = (CommonTree)adaptor.Create(token);
					adaptor.AddChild(commonTree, commonTree2);
					try
					{
						value_return.value = new ValueExpression(int.Parse(token?.Text));
					}
					catch (OverflowException)
					{
						value_return.value = new ValueExpression(long.Parse(token?.Text));
					}
					break;
				case 2:
					commonTree = (CommonTree)adaptor.Nil();
					token2 = (IToken)Match(input, 9, Follow._FLOAT_in_value884);
					commonTree3 = (CommonTree)adaptor.Create(token2);
					adaptor.AddChild(commonTree, commonTree3);
					value_return.value = new ValueExpression(double.Parse(token2?.Text, NumberStyles.Float, numberFormatInfo));
					break;
				case 3:
					commonTree = (CommonTree)adaptor.Nil();
					token3 = (IToken)Match(input, 15, Follow._STRING_in_value892);
					commonTree4 = (CommonTree)adaptor.Create(token3);
					adaptor.AddChild(commonTree, commonTree4);
					value_return.value = new ValueExpression(extractString(token3?.Text));
					break;
				case 4:
					commonTree = (CommonTree)adaptor.Nil();
					token4 = (IToken)Match(input, 4, Follow._DATETIME_in_value901);
					commonTree5 = (CommonTree)adaptor.Create(token4);
					adaptor.AddChild(commonTree, commonTree5);
					value_return.value = new ValueExpression(DateTime.Parse((token4?.Text).Substring(1, (token4?.Text).Length - 2)));
					break;
				case 5:
					commonTree = (CommonTree)adaptor.Nil();
					token5 = (IToken)Match(input, 16, Follow._TRUE_in_value908);
					commonTree6 = (CommonTree)adaptor.Create(token5);
					adaptor.AddChild(commonTree, commonTree6);
					value_return.value = new ValueExpression(true);
					break;
				case 6:
					commonTree = (CommonTree)adaptor.Nil();
					token6 = (IToken)Match(input, 8, Follow._FALSE_in_value916);
					commonTree7 = (CommonTree)adaptor.Create(token6);
					adaptor.AddChild(commonTree, commonTree7);
					value_return.value = new ValueExpression(false);
					break;
				}
				value_return.Stop = input.LT(-1);
				value_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(value_return.Tree, value_return.Start, value_return.Stop);
				return value_return;
			}
			catch (RecognitionException ex3)
			{
				ReportError(ex3);
				Recover(input, ex3);
				value_return.Tree = (CommonTree)adaptor.ErrorNode(input, value_return.Start, input.LT(-1), ex3);
				return value_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("identifier")]
	private identifier_return identifier()
	{
		identifier_return identifier_return = new identifier_return();
		identifier_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		try
		{
			try
			{
				int num = 2;
				try
				{
					switch (input.LA(1))
					{
					case 11:
						num = 1;
						break;
					case 14:
						num = 2;
						break;
					default:
					{
						NoViableAltException ex = new NoViableAltException(string.Empty, 21, 0, input);
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
					commonTree = (CommonTree)adaptor.Nil();
					token = (IToken)Match(input, 11, Follow._ID_in_identifier934);
					commonTree2 = (CommonTree)adaptor.Create(token);
					adaptor.AddChild(commonTree, commonTree2);
					identifier_return.value = new Identifier(token?.Text);
					break;
				case 2:
					commonTree = (CommonTree)adaptor.Nil();
					token2 = (IToken)Match(input, 14, Follow._NAME_in_identifier942);
					commonTree3 = (CommonTree)adaptor.Create(token2);
					adaptor.AddChild(commonTree, commonTree3);
					identifier_return.value = new Identifier((token2?.Text).Substring(1, (token2?.Text).Length - 2));
					break;
				}
				identifier_return.Stop = input.LT(-1);
				identifier_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(identifier_return.Tree, identifier_return.Start, identifier_return.Stop);
				return identifier_return;
			}
			catch (RecognitionException ex2)
			{
				ReportError(ex2);
				Recover(input, ex2);
				identifier_return.Tree = (CommonTree)adaptor.ErrorNode(input, identifier_return.Start, input.LT(-1), ex2);
				return identifier_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("expressionList")]
	private expressionList_return expressionList()
	{
		expressionList_return expressionList_return = new expressionList_return();
		expressionList_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		logicalExpression_return logicalExpression_return = null;
		logicalExpression_return logicalExpression_return2 = null;
		CommonTree commonTree2 = null;
		List<LogicalExpression> list = new List<LogicalExpression>();
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				PushFollow(Follow._logicalExpression_in_expressionList966);
				logicalExpression_return = logicalExpression();
				PopFollow();
				adaptor.AddChild(commonTree, logicalExpression_return.Tree);
				list.Add(logicalExpression_return?.value);
				try
				{
					while (true)
					{
						int num = 2;
						try
						{
							int num2 = input.LA(1);
							if (num2 == 28)
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
						token = (IToken)Match(input, 28, Follow._28_in_expressionList973);
						commonTree2 = (CommonTree)adaptor.Create(token);
						adaptor.AddChild(commonTree, commonTree2);
						PushFollow(Follow._logicalExpression_in_expressionList977);
						logicalExpression_return2 = logicalExpression();
						PopFollow();
						adaptor.AddChild(commonTree, logicalExpression_return2.Tree);
						list.Add(logicalExpression_return2?.value);
					}
				}
				finally
				{
				}
				expressionList_return.value = list;
				expressionList_return.Stop = input.LT(-1);
				expressionList_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(expressionList_return.Tree, expressionList_return.Start, expressionList_return.Stop);
				return expressionList_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				expressionList_return.Tree = (CommonTree)adaptor.ErrorNode(input, expressionList_return.Start, input.LT(-1), ex);
				return expressionList_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}

	[GrammarRule("arguments")]
	private arguments_return arguments()
	{
		arguments_return arguments_return = new arguments_return();
		arguments_return.Start = input.LT(1);
		CommonTree commonTree = null;
		IToken token = null;
		IToken token2 = null;
		expressionList_return expressionList_return = null;
		CommonTree commonTree2 = null;
		CommonTree commonTree3 = null;
		arguments_return.value = new List<LogicalExpression>();
		try
		{
			try
			{
				commonTree = (CommonTree)adaptor.Nil();
				token = (IToken)Match(input, 24, Follow._24_in_arguments1006);
				commonTree2 = (CommonTree)adaptor.Create(token);
				adaptor.AddChild(commonTree, commonTree2);
				int num = 2;
				try
				{
					try
					{
						int num2 = input.LA(1);
						switch (num2)
						{
						default:
							if ((num2 >= 11 && num2 <= 12) || (num2 >= 14 && num2 <= 16) || num2 == 19 || num2 == 24 || num2 == 29 || num2 == 44 || num2 == 48)
							{
								break;
							}
							goto end_IL_0080;
						case 4:
						case 8:
						case 9:
							break;
						}
						num = 1;
						end_IL_0080:;
					}
					finally
					{
					}
					if (num == 1)
					{
						PushFollow(Follow._expressionList_in_arguments1010);
						expressionList_return = expressionList();
						PopFollow();
						adaptor.AddChild(commonTree, expressionList_return.Tree);
						arguments_return.value = expressionList_return?.value;
					}
				}
				finally
				{
				}
				token2 = (IToken)Match(input, 25, Follow._25_in_arguments1017);
				commonTree3 = (CommonTree)adaptor.Create(token2);
				adaptor.AddChild(commonTree, commonTree3);
				arguments_return.Stop = input.LT(-1);
				arguments_return.Tree = (CommonTree)adaptor.RulePostProcessing(commonTree);
				adaptor.SetTokenBoundaries(arguments_return.Tree, arguments_return.Start, arguments_return.Stop);
				return arguments_return;
			}
			catch (RecognitionException ex)
			{
				ReportError(ex);
				Recover(input, ex);
				arguments_return.Tree = (CommonTree)adaptor.ErrorNode(input, arguments_return.Start, input.LT(-1), ex);
				return arguments_return;
			}
			finally
			{
			}
		}
		finally
		{
		}
	}
}
