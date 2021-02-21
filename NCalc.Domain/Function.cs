namespace NCalc.Domain
{
	public class Function : LogicalExpression
	{
		public Identifier Identifier
		{
			get;
			set;
		}

		public LogicalExpression[] Expressions
		{
			get;
			set;
		}

		public Function(Identifier identifier, LogicalExpression[] expressions)
		{
			Identifier = identifier;
			Expressions = expressions;
		}

		public override void Accept(LogicalExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
