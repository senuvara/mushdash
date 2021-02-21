using System;

namespace NCalc
{
	public class Numbers
	{
		private static object ConvertIfString(object s)
		{
			if (s is string || s is char)
			{
				return decimal.Parse(s.ToString());
			}
			return s;
		}

		public static object Add(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			TypeCode typeCode = Type.GetTypeCode(a.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(b.GetType());
			switch (typeCode)
			{
			case TypeCode.Boolean:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'");
				case TypeCode.Byte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.Byte:
					return (byte)a + (byte)b;
				case TypeCode.SByte:
					return (byte)a + (sbyte)b;
				case TypeCode.Int16:
					return (byte)a + (short)b;
				case TypeCode.UInt16:
					return (byte)a + (ushort)b;
				case TypeCode.Int32:
					return (byte)a + (int)b;
				case TypeCode.UInt32:
					return (byte)a + (uint)b;
				case TypeCode.Int64:
					return (int)(byte)a + (long)b;
				case TypeCode.UInt64:
					return (byte)a + (ulong)b;
				case TypeCode.Single:
					return (float)(int)(byte)a + (float)b;
				case TypeCode.Double:
					return (double)(int)(byte)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(byte)a + (decimal)b;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.Byte:
					return (sbyte)a + (byte)b;
				case TypeCode.SByte:
					return (sbyte)a + (sbyte)b;
				case TypeCode.Int16:
					return (sbyte)a + (short)b;
				case TypeCode.UInt16:
					return (sbyte)a + (ushort)b;
				case TypeCode.Int32:
					return (sbyte)a + (int)b;
				case TypeCode.UInt32:
					return (sbyte)a + (uint)b;
				case TypeCode.Int64:
					return (sbyte)a + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)(sbyte)a + (float)b;
				case TypeCode.Double:
					return (double)(sbyte)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(sbyte)a + (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.Byte:
					return (short)a + (byte)b;
				case TypeCode.SByte:
					return (short)a + (sbyte)b;
				case TypeCode.Int16:
					return (short)a + (short)b;
				case TypeCode.UInt16:
					return (short)a + (ushort)b;
				case TypeCode.Int32:
					return (short)a + (int)b;
				case TypeCode.UInt32:
					return (short)a + (uint)b;
				case TypeCode.Int64:
					return (short)a + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)(short)a + (float)b;
				case TypeCode.Double:
					return (double)(short)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(short)a + (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.Byte:
					return (ushort)a + (byte)b;
				case TypeCode.SByte:
					return (ushort)a + (sbyte)b;
				case TypeCode.Int16:
					return (ushort)a + (short)b;
				case TypeCode.UInt16:
					return (ushort)a + (ushort)b;
				case TypeCode.Int32:
					return (ushort)a + (int)b;
				case TypeCode.UInt32:
					return (ushort)a + (uint)b;
				case TypeCode.Int64:
					return (int)(ushort)a + (long)b;
				case TypeCode.UInt64:
					return (ushort)a + (ulong)b;
				case TypeCode.Single:
					return (float)(int)(ushort)a + (float)b;
				case TypeCode.Double:
					return (double)(int)(ushort)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(ushort)a + (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.Byte:
					return (int)a + (byte)b;
				case TypeCode.SByte:
					return (int)a + (sbyte)b;
				case TypeCode.Int16:
					return (int)a + (short)b;
				case TypeCode.UInt16:
					return (int)a + (ushort)b;
				case TypeCode.Int32:
					return (int)a + (int)b;
				case TypeCode.UInt32:
					return (int)a + (uint)b;
				case TypeCode.Int64:
					return (int)a + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)(int)a + (float)b;
				case TypeCode.Double:
					return (double)(int)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(int)a + (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
				case TypeCode.Byte:
					return (uint)a + (byte)b;
				case TypeCode.SByte:
					return (uint)a + (sbyte)b;
				case TypeCode.Int16:
					return (uint)a + (short)b;
				case TypeCode.UInt16:
					return (uint)a + (ushort)b;
				case TypeCode.Int32:
					return (uint)a + (int)b;
				case TypeCode.UInt32:
					return (uint)a + (uint)b;
				case TypeCode.Int64:
					return (uint)a + (long)b;
				case TypeCode.UInt64:
					return (uint)a + (ulong)b;
				case TypeCode.Single:
					return (float)(uint)a + (float)b;
				case TypeCode.Double:
					return (double)(uint)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(uint)a + (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.Byte:
					return (long)a + (int)(byte)b;
				case TypeCode.SByte:
					return (long)a + (sbyte)b;
				case TypeCode.Int16:
					return (long)a + (short)b;
				case TypeCode.UInt16:
					return (long)a + (int)(ushort)b;
				case TypeCode.Int32:
					return (long)a + (int)b;
				case TypeCode.UInt32:
					return (long)a + (uint)b;
				case TypeCode.Int64:
					return (long)a + (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)(long)a + (float)b;
				case TypeCode.Double:
					return (double)(long)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(long)a + (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.Byte:
					return (ulong)a + (byte)b;
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a + (ushort)b;
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a + (uint)b;
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'ulong'");
				case TypeCode.UInt64:
					return (ulong)a + (ulong)b;
				case TypeCode.Single:
					return (float)(ulong)a + (float)b;
				case TypeCode.Double:
					return (double)(ulong)a + (double)b;
				case TypeCode.Decimal:
					return (decimal)(ulong)a + (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.Byte:
					return (float)a + (float)(int)(byte)b;
				case TypeCode.SByte:
					return (float)a + (float)(sbyte)b;
				case TypeCode.Int16:
					return (float)a + (float)(short)b;
				case TypeCode.UInt16:
					return (float)a + (float)(int)(ushort)b;
				case TypeCode.Int32:
					return (float)a + (float)(int)b;
				case TypeCode.UInt32:
					return (float)a + (float)(uint)b;
				case TypeCode.Int64:
					return (float)a + (float)(long)b;
				case TypeCode.UInt64:
					return (float)a + (float)(ulong)b;
				case TypeCode.Single:
					return (float)a + (float)b;
				case TypeCode.Double:
					return (double)(float)a + (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) + (decimal)b;
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.Byte:
					return (double)a + (double)(int)(byte)b;
				case TypeCode.SByte:
					return (double)a + (double)(sbyte)b;
				case TypeCode.Int16:
					return (double)a + (double)(short)b;
				case TypeCode.UInt16:
					return (double)a + (double)(int)(ushort)b;
				case TypeCode.Int32:
					return (double)a + (double)(int)b;
				case TypeCode.UInt32:
					return (double)a + (double)(uint)b;
				case TypeCode.Int64:
					return (double)a + (double)(long)b;
				case TypeCode.UInt64:
					return (double)a + (double)(ulong)b;
				case TypeCode.Single:
					return (double)a + (double)(float)b;
				case TypeCode.Double:
					return (double)a + (double)b;
				case TypeCode.Decimal:
					return Convert.ToDecimal(a) + (decimal)b;
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.Byte:
					return (decimal)a + (decimal)(byte)b;
				case TypeCode.SByte:
					return (decimal)a + (decimal)(sbyte)b;
				case TypeCode.Int16:
					return (decimal)a + (decimal)(short)b;
				case TypeCode.UInt16:
					return (decimal)a + (decimal)(ushort)b;
				case TypeCode.Int32:
					return (decimal)a + (decimal)(int)b;
				case TypeCode.UInt32:
					return (decimal)a + (decimal)(uint)b;
				case TypeCode.Int64:
					return (decimal)a + (decimal)(long)b;
				case TypeCode.UInt64:
					return (decimal)a + (decimal)(ulong)b;
				case TypeCode.Single:
					return (decimal)a + Convert.ToDecimal(b);
				case TypeCode.Double:
					return (decimal)a + Convert.ToDecimal(b);
				case TypeCode.Decimal:
					return (decimal)a + (decimal)b;
				}
				break;
			}
			return null;
		}

		public static object Soustract(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			TypeCode typeCode = Type.GetTypeCode(a.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(b.GetType());
			switch (typeCode)
			{
			case TypeCode.Boolean:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
				case TypeCode.Byte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
				}
				break;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (byte)a - (sbyte)b;
				case TypeCode.Int16:
					return (byte)a - (short)b;
				case TypeCode.UInt16:
					return (byte)a - (ushort)b;
				case TypeCode.Int32:
					return (byte)a - (int)b;
				case TypeCode.UInt32:
					return (byte)a - (uint)b;
				case TypeCode.Int64:
					return (int)(byte)a - (long)b;
				case TypeCode.UInt64:
					return (byte)a - (ulong)b;
				case TypeCode.Single:
					return (float)(int)(byte)a - (float)b;
				case TypeCode.Double:
					return (double)(int)(byte)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(byte)a - (decimal)b;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (sbyte)a - (sbyte)b;
				case TypeCode.Int16:
					return (sbyte)a - (short)b;
				case TypeCode.UInt16:
					return (sbyte)a - (ushort)b;
				case TypeCode.Int32:
					return (sbyte)a - (int)b;
				case TypeCode.UInt32:
					return (sbyte)a - (uint)b;
				case TypeCode.Int64:
					return (sbyte)a - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)(sbyte)a - (float)b;
				case TypeCode.Double:
					return (double)(sbyte)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(sbyte)a - (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (short)a - (sbyte)b;
				case TypeCode.Int16:
					return (short)a - (short)b;
				case TypeCode.UInt16:
					return (short)a - (ushort)b;
				case TypeCode.Int32:
					return (short)a - (int)b;
				case TypeCode.UInt32:
					return (short)a - (uint)b;
				case TypeCode.Int64:
					return (short)a - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)(short)a - (float)b;
				case TypeCode.Double:
					return (double)(short)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(short)a - (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (ushort)a - (sbyte)b;
				case TypeCode.Int16:
					return (ushort)a - (short)b;
				case TypeCode.UInt16:
					return (ushort)a - (ushort)b;
				case TypeCode.Int32:
					return (ushort)a - (int)b;
				case TypeCode.UInt32:
					return (ushort)a - (uint)b;
				case TypeCode.Int64:
					return (int)(ushort)a - (long)b;
				case TypeCode.UInt64:
					return (ushort)a - (ulong)b;
				case TypeCode.Single:
					return (float)(int)(ushort)a - (float)b;
				case TypeCode.Double:
					return (double)(int)(ushort)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(ushort)a - (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a - (sbyte)b;
				case TypeCode.Int16:
					return (int)a - (short)b;
				case TypeCode.UInt16:
					return (int)a - (ushort)b;
				case TypeCode.Int32:
					return (int)a - (int)b;
				case TypeCode.UInt32:
					return (int)a - (uint)b;
				case TypeCode.Int64:
					return (int)a - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)(int)a - (float)b;
				case TypeCode.Double:
					return (double)(int)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(int)a - (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (uint)a - (sbyte)b;
				case TypeCode.Int16:
					return (uint)a - (short)b;
				case TypeCode.UInt16:
					return (uint)a - (ushort)b;
				case TypeCode.Int32:
					return (uint)a - (int)b;
				case TypeCode.UInt32:
					return (uint)a - (uint)b;
				case TypeCode.Int64:
					return (uint)a - (long)b;
				case TypeCode.UInt64:
					return (uint)a - (ulong)b;
				case TypeCode.Single:
					return (float)(uint)a - (float)b;
				case TypeCode.Double:
					return (double)(uint)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(uint)a - (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a - (sbyte)b;
				case TypeCode.Int16:
					return (long)a - (short)b;
				case TypeCode.UInt16:
					return (long)a - (int)(ushort)b;
				case TypeCode.Int32:
					return (long)a - (int)b;
				case TypeCode.UInt32:
					return (long)a - (uint)b;
				case TypeCode.Int64:
					return (long)a - (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)(long)a - (float)b;
				case TypeCode.Double:
					return (double)(long)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(long)a - (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a - (ushort)b;
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a - (uint)b;
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a - (ulong)b;
				case TypeCode.Single:
					return (float)(ulong)a - (float)b;
				case TypeCode.Double:
					return (double)(ulong)a - (double)b;
				case TypeCode.Decimal:
					return (decimal)(ulong)a - (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a - (float)(sbyte)b;
				case TypeCode.Int16:
					return (float)a - (float)(short)b;
				case TypeCode.UInt16:
					return (float)a - (float)(int)(ushort)b;
				case TypeCode.Int32:
					return (float)a - (float)(int)b;
				case TypeCode.UInt32:
					return (float)a - (float)(uint)b;
				case TypeCode.Int64:
					return (float)a - (float)(long)b;
				case TypeCode.UInt64:
					return (float)a - (float)(ulong)b;
				case TypeCode.Single:
					return (float)a - (float)b;
				case TypeCode.Double:
					return (double)(float)a - (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'decimal'");
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a - (double)(sbyte)b;
				case TypeCode.Int16:
					return (double)a - (double)(short)b;
				case TypeCode.UInt16:
					return (double)a - (double)(int)(ushort)b;
				case TypeCode.Int32:
					return (double)a - (double)(int)b;
				case TypeCode.UInt32:
					return (double)a - (double)(uint)b;
				case TypeCode.Int64:
					return (double)a - (double)(long)b;
				case TypeCode.UInt64:
					return (double)a - (double)(ulong)b;
				case TypeCode.Single:
					return (double)a - (double)(float)b;
				case TypeCode.Double:
					return (double)a - (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'decimal'");
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a - (decimal)(sbyte)b;
				case TypeCode.Int16:
					return (decimal)a - (decimal)(short)b;
				case TypeCode.UInt16:
					return (decimal)a - (decimal)(ushort)b;
				case TypeCode.Int32:
					return (decimal)a - (decimal)(int)b;
				case TypeCode.UInt32:
					return (decimal)a - (decimal)(uint)b;
				case TypeCode.Int64:
					return (decimal)a - (decimal)(long)b;
				case TypeCode.UInt64:
					return (decimal)a - (decimal)(ulong)b;
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'float'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'double'");
				case TypeCode.Decimal:
					return (decimal)a - (decimal)b;
				}
				break;
			}
			return null;
		}

		public static object Multiply(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			TypeCode typeCode = Type.GetTypeCode(a.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(b.GetType());
			switch (typeCode)
			{
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (byte)a * (sbyte)b;
				case TypeCode.Int16:
					return (byte)a * (short)b;
				case TypeCode.UInt16:
					return (byte)a * (ushort)b;
				case TypeCode.Int32:
					return (byte)a * (int)b;
				case TypeCode.UInt32:
					return (byte)a * (uint)b;
				case TypeCode.Int64:
					return (int)(byte)a * (long)b;
				case TypeCode.UInt64:
					return (byte)a * (ulong)b;
				case TypeCode.Single:
					return (float)(int)(byte)a * (float)b;
				case TypeCode.Double:
					return (double)(int)(byte)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(byte)a * (decimal)b;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (sbyte)a * (sbyte)b;
				case TypeCode.Int16:
					return (sbyte)a * (short)b;
				case TypeCode.UInt16:
					return (sbyte)a * (ushort)b;
				case TypeCode.Int32:
					return (sbyte)a * (int)b;
				case TypeCode.UInt32:
					return (sbyte)a * (uint)b;
				case TypeCode.Int64:
					return (sbyte)a * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)(sbyte)a * (float)b;
				case TypeCode.Double:
					return (double)(sbyte)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(sbyte)a * (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (short)a * (sbyte)b;
				case TypeCode.Int16:
					return (short)a * (short)b;
				case TypeCode.UInt16:
					return (short)a * (ushort)b;
				case TypeCode.Int32:
					return (short)a * (int)b;
				case TypeCode.UInt32:
					return (short)a * (uint)b;
				case TypeCode.Int64:
					return (short)a * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)(short)a * (float)b;
				case TypeCode.Double:
					return (double)(short)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(short)a * (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (ushort)a * (sbyte)b;
				case TypeCode.Int16:
					return (ushort)a * (short)b;
				case TypeCode.UInt16:
					return (ushort)a * (ushort)b;
				case TypeCode.Int32:
					return (ushort)a * (int)b;
				case TypeCode.UInt32:
					return (ushort)a * (uint)b;
				case TypeCode.Int64:
					return (int)(ushort)a * (long)b;
				case TypeCode.UInt64:
					return (ushort)a * (ulong)b;
				case TypeCode.Single:
					return (float)(int)(ushort)a * (float)b;
				case TypeCode.Double:
					return (double)(int)(ushort)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(ushort)a * (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a * (sbyte)b;
				case TypeCode.Int16:
					return (int)a * (short)b;
				case TypeCode.UInt16:
					return (int)a * (ushort)b;
				case TypeCode.Int32:
					return (int)a * (int)b;
				case TypeCode.UInt32:
					return (int)a * (uint)b;
				case TypeCode.Int64:
					return (int)a * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)(int)a * (float)b;
				case TypeCode.Double:
					return (double)(int)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(int)a * (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (uint)a * (sbyte)b;
				case TypeCode.Int16:
					return (uint)a * (short)b;
				case TypeCode.UInt16:
					return (uint)a * (ushort)b;
				case TypeCode.Int32:
					return (uint)a * (int)b;
				case TypeCode.UInt32:
					return (uint)a * (uint)b;
				case TypeCode.Int64:
					return (uint)a * (long)b;
				case TypeCode.UInt64:
					return (uint)a * (ulong)b;
				case TypeCode.Single:
					return (float)(uint)a * (float)b;
				case TypeCode.Double:
					return (double)(uint)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(uint)a * (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a * (sbyte)b;
				case TypeCode.Int16:
					return (long)a * (short)b;
				case TypeCode.UInt16:
					return (long)a * (int)(ushort)b;
				case TypeCode.Int32:
					return (long)a * (int)b;
				case TypeCode.UInt32:
					return (long)a * (uint)b;
				case TypeCode.Int64:
					return (long)a * (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)(long)a * (float)b;
				case TypeCode.Double:
					return (double)(long)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(long)a * (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a * (ushort)b;
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a * (uint)b;
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a * (ulong)b;
				case TypeCode.Single:
					return (float)(ulong)a * (float)b;
				case TypeCode.Double:
					return (double)(ulong)a * (double)b;
				case TypeCode.Decimal:
					return (decimal)(ulong)a * (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a * (float)(sbyte)b;
				case TypeCode.Int16:
					return (float)a * (float)(short)b;
				case TypeCode.UInt16:
					return (float)a * (float)(int)(ushort)b;
				case TypeCode.Int32:
					return (float)a * (float)(int)b;
				case TypeCode.UInt32:
					return (float)a * (float)(uint)b;
				case TypeCode.Int64:
					return (float)a * (float)(long)b;
				case TypeCode.UInt64:
					return (float)a * (float)(ulong)b;
				case TypeCode.Single:
					return (float)a * (float)b;
				case TypeCode.Double:
					return (double)(float)a * (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'decimal'");
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a * (double)(sbyte)b;
				case TypeCode.Int16:
					return (double)a * (double)(short)b;
				case TypeCode.UInt16:
					return (double)a * (double)(int)(ushort)b;
				case TypeCode.Int32:
					return (double)a * (double)(int)b;
				case TypeCode.UInt32:
					return (double)a * (double)(uint)b;
				case TypeCode.Int64:
					return (double)a * (double)(long)b;
				case TypeCode.UInt64:
					return (double)a * (double)(ulong)b;
				case TypeCode.Single:
					return (double)a * (double)(float)b;
				case TypeCode.Double:
					return (double)a * (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'decimal'");
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a * (decimal)(sbyte)b;
				case TypeCode.Int16:
					return (decimal)a * (decimal)(short)b;
				case TypeCode.UInt16:
					return (decimal)a * (decimal)(ushort)b;
				case TypeCode.Int32:
					return (decimal)a * (decimal)(int)b;
				case TypeCode.UInt32:
					return (decimal)a * (decimal)(uint)b;
				case TypeCode.Int64:
					return (decimal)a * (decimal)(long)b;
				case TypeCode.UInt64:
					return (decimal)a * (decimal)(ulong)b;
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'float'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'double'");
				case TypeCode.Decimal:
					return (decimal)a * (decimal)b;
				}
				break;
			}
			return null;
		}

		public static object Divide(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			TypeCode typeCode = Type.GetTypeCode(a.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(b.GetType());
			switch (typeCode)
			{
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)(byte)a / (int)(sbyte)b;
				case TypeCode.Int16:
					return (int)(byte)a / (int)(short)b;
				case TypeCode.UInt16:
					return (int)(byte)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (int)(byte)a / (int)b;
				case TypeCode.UInt32:
					return (byte)a / (uint)b;
				case TypeCode.Int64:
					return (int)(byte)a / (long)b;
				case TypeCode.UInt64:
					return (byte)a / (ulong)b;
				case TypeCode.Single:
					return (float)(int)(byte)a / (float)b;
				case TypeCode.Double:
					return (double)(int)(byte)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(byte)a / (decimal)b;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (sbyte)a / (sbyte)b;
				case TypeCode.Int16:
					return (sbyte)a / (short)b;
				case TypeCode.UInt16:
					return (int)(sbyte)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (sbyte)a / (int)b;
				case TypeCode.UInt32:
					return (long)(sbyte)a / (long)(uint)b;
				case TypeCode.Int64:
					return (sbyte)a / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)(sbyte)a / (float)b;
				case TypeCode.Double:
					return (double)(sbyte)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(sbyte)a / (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (short)a / (sbyte)b;
				case TypeCode.Int16:
					return (short)a / (short)b;
				case TypeCode.UInt16:
					return (int)(short)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (short)a / (int)b;
				case TypeCode.UInt32:
					return (long)(short)a / (long)(uint)b;
				case TypeCode.Int64:
					return (short)a / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)(short)a / (float)b;
				case TypeCode.Double:
					return (double)(short)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(short)a / (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)(ushort)a / (int)(sbyte)b;
				case TypeCode.Int16:
					return (int)(ushort)a / (int)(short)b;
				case TypeCode.UInt16:
					return (int)(ushort)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (int)(ushort)a / (int)b;
				case TypeCode.UInt32:
					return (ushort)a / (uint)b;
				case TypeCode.Int64:
					return (int)(ushort)a / (long)b;
				case TypeCode.UInt64:
					return (ushort)a / (ulong)b;
				case TypeCode.Single:
					return (float)(int)(ushort)a / (float)b;
				case TypeCode.Double:
					return (double)(int)(ushort)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(ushort)a / (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a / (sbyte)b;
				case TypeCode.Int16:
					return (int)a / (short)b;
				case TypeCode.UInt16:
					return (int)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (int)a / (int)b;
				case TypeCode.UInt32:
					return (long)(int)a / (long)(uint)b;
				case TypeCode.Int64:
					return (int)a / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)(int)a / (float)b;
				case TypeCode.Double:
					return (double)(int)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(int)a / (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)(uint)a / (long)(sbyte)b;
				case TypeCode.Int16:
					return (long)(uint)a / (long)(short)b;
				case TypeCode.UInt16:
					return (uint)a / (ushort)b;
				case TypeCode.Int32:
					return (long)(uint)a / (long)(int)b;
				case TypeCode.UInt32:
					return (uint)a / (uint)b;
				case TypeCode.Int64:
					return (long)(uint)a / (long)b;
				case TypeCode.UInt64:
					return (uint)a / (ulong)b;
				case TypeCode.Single:
					return (float)(uint)a / (float)b;
				case TypeCode.Double:
					return (double)(uint)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(uint)a / (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a / (sbyte)b;
				case TypeCode.Int16:
					return (long)a / (short)b;
				case TypeCode.UInt16:
					return (long)a / (int)(ushort)b;
				case TypeCode.Int32:
					return (long)a / (int)b;
				case TypeCode.UInt32:
					return (long)a / (long)(uint)b;
				case TypeCode.Int64:
					return (long)a / (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)(long)a / (float)b;
				case TypeCode.Double:
					return (double)(long)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(long)a / (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a / (ushort)b;
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a / (uint)b;
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a / (ulong)b;
				case TypeCode.Single:
					return (float)(ulong)a / (float)b;
				case TypeCode.Double:
					return (double)(ulong)a / (double)b;
				case TypeCode.Decimal:
					return (decimal)(ulong)a / (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a / (float)(sbyte)b;
				case TypeCode.Int16:
					return (float)a / (float)(short)b;
				case TypeCode.UInt16:
					return (float)a / (float)(int)(ushort)b;
				case TypeCode.Int32:
					return (float)a / (float)(int)b;
				case TypeCode.UInt32:
					return (float)a / (float)(uint)b;
				case TypeCode.Int64:
					return (float)a / (float)(long)b;
				case TypeCode.UInt64:
					return (float)a / (float)(ulong)b;
				case TypeCode.Single:
					return (float)a / (float)b;
				case TypeCode.Double:
					return (double)(float)a / (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'decimal'");
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a / (double)(sbyte)b;
				case TypeCode.Int16:
					return (double)a / (double)(short)b;
				case TypeCode.UInt16:
					return (double)a / (double)(int)(ushort)b;
				case TypeCode.Int32:
					return (double)a / (double)(int)b;
				case TypeCode.UInt32:
					return (double)a / (double)(uint)b;
				case TypeCode.Int64:
					return (double)a / (double)(long)b;
				case TypeCode.UInt64:
					return (double)a / (double)(ulong)b;
				case TypeCode.Single:
					return (double)a / (double)(float)b;
				case TypeCode.Double:
					return (double)a / (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'decimal'");
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a / (decimal)(sbyte)b;
				case TypeCode.Int16:
					return (decimal)a / (decimal)(short)b;
				case TypeCode.UInt16:
					return (decimal)a / (decimal)(ushort)b;
				case TypeCode.Int32:
					return (decimal)a / (decimal)(int)b;
				case TypeCode.UInt32:
					return (decimal)a / (decimal)(uint)b;
				case TypeCode.Int64:
					return (decimal)a / (decimal)(long)b;
				case TypeCode.UInt64:
					return (decimal)a / (decimal)(ulong)b;
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'float'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'double'");
				case TypeCode.Decimal:
					return (decimal)a / (decimal)b;
				}
				break;
			}
			return null;
		}

		public static object Modulo(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			TypeCode typeCode = Type.GetTypeCode(a.GetType());
			TypeCode typeCode2 = Type.GetTypeCode(b.GetType());
			switch (typeCode)
			{
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'byte' and 'bool'");
				case TypeCode.SByte:
					return (int)(byte)a % (int)(sbyte)b;
				case TypeCode.Int16:
					return (int)(byte)a % (int)(short)b;
				case TypeCode.UInt16:
					return (int)(byte)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (int)(byte)a % (int)b;
				case TypeCode.UInt32:
					return (byte)a % (uint)b;
				case TypeCode.Int64:
					return (int)(byte)a % (long)b;
				case TypeCode.UInt64:
					return (byte)a % (ulong)b;
				case TypeCode.Single:
					return (float)(int)(byte)a % (float)b;
				case TypeCode.Double:
					return (double)(int)(byte)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(byte)a % (decimal)b;
				}
				break;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'bool'");
				case TypeCode.SByte:
					return (sbyte)a % (sbyte)b;
				case TypeCode.Int16:
					return (sbyte)a % (short)b;
				case TypeCode.UInt16:
					return (int)(sbyte)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (sbyte)a % (int)b;
				case TypeCode.UInt32:
					return (long)(sbyte)a % (long)(uint)b;
				case TypeCode.Int64:
					return (sbyte)a % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'");
				case TypeCode.Single:
					return (float)(sbyte)a % (float)b;
				case TypeCode.Double:
					return (double)(sbyte)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(sbyte)a % (decimal)b;
				}
				break;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'bool'");
				case TypeCode.SByte:
					return (short)a % (sbyte)b;
				case TypeCode.Int16:
					return (short)a % (short)b;
				case TypeCode.UInt16:
					return (int)(short)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (short)a % (int)b;
				case TypeCode.UInt32:
					return (long)(short)a % (long)(uint)b;
				case TypeCode.Int64:
					return (short)a % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'ulong'");
				case TypeCode.Single:
					return (float)(short)a % (float)b;
				case TypeCode.Double:
					return (double)(short)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(short)a % (decimal)b;
				}
				break;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ushort' and 'bool'");
				case TypeCode.SByte:
					return (int)(ushort)a % (int)(sbyte)b;
				case TypeCode.Int16:
					return (int)(ushort)a % (int)(short)b;
				case TypeCode.UInt16:
					return (int)(ushort)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (int)(ushort)a % (int)b;
				case TypeCode.UInt32:
					return (ushort)a % (uint)b;
				case TypeCode.Int64:
					return (int)(ushort)a % (long)b;
				case TypeCode.UInt64:
					return (ushort)a % (ulong)b;
				case TypeCode.Single:
					return (float)(int)(ushort)a % (float)b;
				case TypeCode.Double:
					return (double)(int)(ushort)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(ushort)a % (decimal)b;
				}
				break;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'bool'");
				case TypeCode.SByte:
					return (int)a % (sbyte)b;
				case TypeCode.Int16:
					return (int)a % (short)b;
				case TypeCode.UInt16:
					return (int)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (int)a % (int)b;
				case TypeCode.UInt32:
					return (long)(int)a % (long)(uint)b;
				case TypeCode.Int64:
					return (int)a % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'ulong'");
				case TypeCode.Single:
					return (float)(int)a % (float)b;
				case TypeCode.Double:
					return (double)(int)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(int)a % (decimal)b;
				}
				break;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'uint' and 'bool'");
				case TypeCode.SByte:
					return (long)(uint)a % (long)(sbyte)b;
				case TypeCode.Int16:
					return (long)(uint)a % (long)(short)b;
				case TypeCode.UInt16:
					return (uint)a % (ushort)b;
				case TypeCode.Int32:
					return (long)(uint)a % (long)(int)b;
				case TypeCode.UInt32:
					return (uint)a % (uint)b;
				case TypeCode.Int64:
					return (long)(uint)a % (long)b;
				case TypeCode.UInt64:
					return (uint)a % (ulong)b;
				case TypeCode.Single:
					return (float)(uint)a % (float)b;
				case TypeCode.Double:
					return (double)(uint)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(uint)a % (decimal)b;
				}
				break;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'bool'");
				case TypeCode.SByte:
					return (long)a % (sbyte)b;
				case TypeCode.Int16:
					return (long)a % (short)b;
				case TypeCode.UInt16:
					return (long)a % (int)(ushort)b;
				case TypeCode.Int32:
					return (long)a % (int)b;
				case TypeCode.UInt32:
					return (long)a % (long)(uint)b;
				case TypeCode.Int64:
					return (long)a % (long)b;
				case TypeCode.UInt64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'ulong'");
				case TypeCode.Single:
					return (float)(long)a % (float)b;
				case TypeCode.Double:
					return (double)(long)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(long)a % (decimal)b;
				}
				break;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'bool'");
				case TypeCode.SByte:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'");
				case TypeCode.Int16:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'short'");
				case TypeCode.UInt16:
					return (ulong)a % (ushort)b;
				case TypeCode.Int32:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'int'");
				case TypeCode.UInt32:
					return (ulong)a % (uint)b;
				case TypeCode.Int64:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'long'");
				case TypeCode.UInt64:
					return (ulong)a % (ulong)b;
				case TypeCode.Single:
					return (float)(ulong)a % (float)b;
				case TypeCode.Double:
					return (double)(ulong)a % (double)b;
				case TypeCode.Decimal:
					return (decimal)(ulong)a % (decimal)b;
				}
				break;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'bool'");
				case TypeCode.SByte:
					return (float)a % (float)(sbyte)b;
				case TypeCode.Int16:
					return (float)a % (float)(short)b;
				case TypeCode.UInt16:
					return (float)a % (float)(int)(ushort)b;
				case TypeCode.Int32:
					return (float)a % (float)(int)b;
				case TypeCode.UInt32:
					return (float)a % (float)(uint)b;
				case TypeCode.Int64:
					return (float)a % (float)(long)b;
				case TypeCode.UInt64:
					return (float)a % (float)(ulong)b;
				case TypeCode.Single:
					return (float)a % (float)b;
				case TypeCode.Double:
					return (double)(float)a % (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'decimal'");
				}
				break;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'bool'");
				case TypeCode.SByte:
					return (double)a % (double)(sbyte)b;
				case TypeCode.Int16:
					return (double)a % (double)(short)b;
				case TypeCode.UInt16:
					return (double)a % (double)(int)(ushort)b;
				case TypeCode.Int32:
					return (double)a % (double)(int)b;
				case TypeCode.UInt32:
					return (double)a % (double)(uint)b;
				case TypeCode.Int64:
					return (double)a % (double)(long)b;
				case TypeCode.UInt64:
					return (double)a % (double)(ulong)b;
				case TypeCode.Single:
					return (double)a % (double)(float)b;
				case TypeCode.Double:
					return (double)a % (double)b;
				case TypeCode.Decimal:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'decimal'");
				}
				break;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Boolean:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'bool'");
				case TypeCode.SByte:
					return (decimal)a % (decimal)(sbyte)b;
				case TypeCode.Int16:
					return (decimal)a % (decimal)(short)b;
				case TypeCode.UInt16:
					return (decimal)a % (decimal)(ushort)b;
				case TypeCode.Int32:
					return (decimal)a % (decimal)(int)b;
				case TypeCode.UInt32:
					return (decimal)a % (decimal)(uint)b;
				case TypeCode.Int64:
					return (decimal)a % (decimal)(long)b;
				case TypeCode.UInt64:
					return (decimal)a % (decimal)(ulong)b;
				case TypeCode.Single:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'float'");
				case TypeCode.Double:
					throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'decimal'");
				case TypeCode.Decimal:
					return (decimal)a % (decimal)b;
				}
				break;
			}
			return null;
		}

		public static object Max(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			if (a == null && b == null)
			{
				return null;
			}
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			switch (Type.GetTypeCode(a.GetType()))
			{
			case TypeCode.Byte:
				return Math.Max((byte)a, Convert.ToByte(b));
			case TypeCode.SByte:
				return Math.Max((sbyte)a, Convert.ToSByte(b));
			case TypeCode.Int16:
				return Math.Max((short)a, Convert.ToInt16(b));
			case TypeCode.UInt16:
				return Math.Max((ushort)a, Convert.ToUInt16(b));
			case TypeCode.Int32:
				return Math.Max((int)a, Convert.ToInt32(b));
			case TypeCode.UInt32:
				return Math.Max((uint)a, Convert.ToUInt32(b));
			case TypeCode.Int64:
				return Math.Max((long)a, Convert.ToInt64(b));
			case TypeCode.UInt64:
				return Math.Max((ulong)a, Convert.ToUInt64(b));
			case TypeCode.Single:
				return Math.Max((float)a, Convert.ToSingle(b));
			case TypeCode.Double:
				return Math.Max((double)a, Convert.ToDouble(b));
			case TypeCode.Decimal:
				return Math.Max((decimal)a, Convert.ToDecimal(b));
			default:
				return null;
			}
		}

		public static object Min(object a, object b)
		{
			a = ConvertIfString(a);
			b = ConvertIfString(b);
			if (a == null && b == null)
			{
				return null;
			}
			if (a == null)
			{
				return b;
			}
			if (b == null)
			{
				return a;
			}
			switch (Type.GetTypeCode(a.GetType()))
			{
			case TypeCode.Byte:
				return Math.Min((byte)a, Convert.ToByte(b));
			case TypeCode.SByte:
				return Math.Min((sbyte)a, Convert.ToSByte(b));
			case TypeCode.Int16:
				return Math.Min((short)a, Convert.ToInt16(b));
			case TypeCode.UInt16:
				return Math.Min((ushort)a, Convert.ToUInt16(b));
			case TypeCode.Int32:
				return Math.Min((int)a, Convert.ToInt32(b));
			case TypeCode.UInt32:
				return Math.Min((uint)a, Convert.ToUInt32(b));
			case TypeCode.Int64:
				return Math.Min((long)a, Convert.ToInt64(b));
			case TypeCode.UInt64:
				return Math.Min((ulong)a, Convert.ToUInt64(b));
			case TypeCode.Single:
				return Math.Min((float)a, Convert.ToSingle(b));
			case TypeCode.Double:
				return Math.Min((double)a, Convert.ToDouble(b));
			case TypeCode.Decimal:
				return Math.Min((decimal)a, Convert.ToDecimal(b));
			default:
				return null;
			}
		}
	}
}
