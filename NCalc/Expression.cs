using Antlr.Runtime;
using NCalc.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NCalc
{
	public class Expression
	{
		protected string OriginalExpression;

		private static bool _cacheEnabled = true;

		private static Dictionary<string, WeakReference> _compiledExpressions = new Dictionary<string, WeakReference>();

		private static readonly ReaderWriterLock Rwl = new ReaderWriterLock();

		protected Dictionary<string, IEnumerator> ParameterEnumerators;

		protected Dictionary<string, object> ParametersBackup;

		private Dictionary<string, object> _parameters;

		public EvaluateOptions Options
		{
			get;
			set;
		}

		public static bool CacheEnabled
		{
			get
			{
				return _cacheEnabled;
			}
			set
			{
				_cacheEnabled = value;
				if (!CacheEnabled)
				{
					_compiledExpressions = new Dictionary<string, WeakReference>();
				}
			}
		}

		public string Error
		{
			get;
			private set;
		}

		public LogicalExpression ParsedExpression
		{
			get;
			private set;
		}

		public Dictionary<string, object> Parameters
		{
			get
			{
				return _parameters ?? (_parameters = new Dictionary<string, object>());
			}
			set
			{
				_parameters = value;
			}
		}

		public event EvaluateFunctionHandler EvaluateFunction;

		public event EvaluateParameterHandler EvaluateParameter;

		public Expression(string expression)
			: this(expression, EvaluateOptions.None)
		{
		}

		public Expression(string expression, EvaluateOptions options)
		{
			if (string.IsNullOrEmpty(expression))
			{
				throw new ArgumentException("Expression can't be empty", "expression");
			}
			OriginalExpression = expression;
			Options = options;
		}

		public Expression(LogicalExpression expression)
			: this(expression, EvaluateOptions.None)
		{
		}

		public Expression(LogicalExpression expression, EvaluateOptions options)
		{
			if (expression == null)
			{
				throw new ArgumentException("Expression can't be null", "expression");
			}
			ParsedExpression = expression;
			Options = options;
		}

		private static void CleanCache()
		{
			List<string> list = new List<string>();
			try
			{
				Rwl.AcquireWriterLock(-1);
				foreach (KeyValuePair<string, WeakReference> compiledExpression in _compiledExpressions)
				{
					if (!compiledExpression.Value.IsAlive)
					{
						list.Add(compiledExpression.Key);
					}
				}
				foreach (string item in list)
				{
					_compiledExpressions.Remove(item);
				}
			}
			finally
			{
				Rwl.ReleaseReaderLock();
			}
		}

		public static LogicalExpression Compile(string expression, bool nocache)
		{
			LogicalExpression logicalExpression = null;
			if (_cacheEnabled && !nocache)
			{
				try
				{
					Rwl.AcquireReaderLock(-1);
					if (_compiledExpressions.ContainsKey(expression))
					{
						WeakReference weakReference = _compiledExpressions[expression];
						logicalExpression = (weakReference.Target as LogicalExpression);
						if (weakReference.IsAlive && logicalExpression != null)
						{
							return logicalExpression;
						}
					}
				}
				finally
				{
					Rwl.ReleaseReaderLock();
				}
			}
			if (logicalExpression == null)
			{
				NCalcLexer tokenSource = new NCalcLexer(new ANTLRStringStream(expression));
				NCalcParser nCalcParser = new NCalcParser(new CommonTokenStream(tokenSource));
				logicalExpression = nCalcParser.ncalcExpression().value;
				if (nCalcParser.Errors != null && nCalcParser.Errors.Count > 0)
				{
					throw new EvaluationException(string.Join(Environment.NewLine, nCalcParser.Errors.ToArray()));
				}
				if (_cacheEnabled && !nocache)
				{
					try
					{
						Rwl.AcquireWriterLock(-1);
						_compiledExpressions[expression] = new WeakReference(logicalExpression);
					}
					finally
					{
						Rwl.ReleaseWriterLock();
					}
					CleanCache();
				}
			}
			return logicalExpression;
		}

		public bool HasErrors()
		{
			try
			{
				if (ParsedExpression == null)
				{
					ParsedExpression = Compile(OriginalExpression, (Options & EvaluateOptions.NoCache) == EvaluateOptions.NoCache);
				}
				return ParsedExpression != null && Error != null;
			}
			catch (Exception ex)
			{
				Error = ex.Message;
				return true;
			}
		}

		public object Evaluate()
		{
			if (HasErrors())
			{
				throw new EvaluationException(Error);
			}
			if (ParsedExpression == null)
			{
				ParsedExpression = Compile(OriginalExpression, (Options & EvaluateOptions.NoCache) == EvaluateOptions.NoCache);
			}
			EvaluationVisitor evaluationVisitor = new EvaluationVisitor(Options);
			evaluationVisitor.EvaluateFunction += this.EvaluateFunction;
			evaluationVisitor.EvaluateParameter += this.EvaluateParameter;
			evaluationVisitor.Parameters = Parameters;
			if ((Options & EvaluateOptions.IterateParameters) == EvaluateOptions.IterateParameters)
			{
				int num = -1;
				ParametersBackup = new Dictionary<string, object>();
				foreach (string key in Parameters.Keys)
				{
					ParametersBackup.Add(key, Parameters[key]);
				}
				ParameterEnumerators = new Dictionary<string, IEnumerator>();
				foreach (object value in Parameters.Values)
				{
					if (!(value is IEnumerable))
					{
						continue;
					}
					int num2 = 0;
					IEnumerator enumerator3 = ((IEnumerable)value).GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							object current3 = enumerator3.Current;
							num2++;
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator3 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (num == -1)
					{
						num = num2;
					}
					else if (num2 != num)
					{
						throw new EvaluationException("When IterateParameters option is used, IEnumerable parameters must have the same number of items");
					}
				}
				foreach (string key2 in Parameters.Keys)
				{
					IEnumerable enumerable = Parameters[key2] as IEnumerable;
					if (enumerable != null)
					{
						ParameterEnumerators.Add(key2, enumerable.GetEnumerator());
					}
				}
				List<object> list = new List<object>();
				for (int i = 0; i < num; i++)
				{
					foreach (string key3 in ParameterEnumerators.Keys)
					{
						IEnumerator enumerator6 = ParameterEnumerators[key3];
						enumerator6.MoveNext();
						Parameters[key3] = enumerator6.Current;
					}
					ParsedExpression.Accept(evaluationVisitor);
					list.Add(evaluationVisitor.Result);
				}
				return list;
			}
			ParsedExpression.Accept(evaluationVisitor);
			return evaluationVisitor.Result;
		}
	}
}
