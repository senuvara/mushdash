using ProtoBuf;

namespace Assets.Scripts.Common
{
	[ProtoContract]
	internal class Beat
	{
		[ProtoMember(1)]
		public string note_type
		{
			get;
			set;
		}

		[ProtoMember(2)]
		public int offset
		{
			get;
			set;
		}

		[ProtoMember(3)]
		public int score
		{
			get;
			set;
		}

		[ProtoMember(4)]
		public Side side
		{
			get;
			set;
		}
	}
}
