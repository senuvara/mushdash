using System;

namespace rail
{
	public class RailComparableID : IEquatable<RailComparableID>, IComparable<RailComparableID>
	{
		public static readonly RailComparableID Nil = new RailComparableID(0uL);

		public ulong id_;

		public RailComparableID()
		{
		}

		public RailComparableID(ulong id)
		{
			id_ = id;
		}

		public bool IsValid()
		{
			return id_ != 0;
		}

		public override bool Equals(object other)
		{
			return other is RailComparableID && this == (RailComparableID)other;
		}

		public override int GetHashCode()
		{
			return id_.GetHashCode();
		}

		public static bool operator ==(RailComparableID x, RailComparableID y)
		{
			if (object.ReferenceEquals(x, null))
			{
				return object.ReferenceEquals(y, null);
			}
			return x.Equals(y);
		}

		public static bool operator !=(RailComparableID x, RailComparableID y)
		{
			return !(x == y);
		}

		public static explicit operator RailComparableID(ulong value)
		{
			return new RailComparableID(value);
		}

		public static explicit operator ulong(RailComparableID that)
		{
			return that.id_;
		}

		public bool Equals(RailComparableID other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, other))
			{
				return true;
			}
			if (GetType() != other.GetType())
			{
				return false;
			}
			return other.id_ == id_;
		}

		public int CompareTo(RailComparableID other)
		{
			return id_.CompareTo(other.id_);
		}

		public override string ToString()
		{
			return id_.ToString();
		}
	}
}
