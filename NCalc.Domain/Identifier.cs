namespace NCalc.Domain
{
	public class Identifier : LogicalExpression
	{
		public string Name
		{
			get;
			set;
		}

		public Identifier(string name)
		{
			Name = name;
		}

		public override void Accept(LogicalExpressionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
