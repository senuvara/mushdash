using ProtoBuf;
using System.Collections.Generic;

namespace Assets.Scripts.Common
{
	[ProtoContract]
	internal class UploadHighScore
	{
		[ProtoMember(1)]
		public string bms_version
		{
			get;
			set;
		}

		[ProtoMember(2)]
		public string music_uid
		{
			get;
			set;
		}

		[ProtoMember(3)]
		public int music_difficulty
		{
			get;
			set;
		}

		[ProtoMember(4)]
		public string character_uid
		{
			get;
			set;
		}

		[ProtoMember(5)]
		public string elfin_uid
		{
			get;
			set;
		}

		[ProtoMember(6)]
		public int combo
		{
			get;
			set;
		}

		[ProtoMember(7)]
		public int hp
		{
			get;
			set;
		}

		[ProtoMember(8)]
		public int score
		{
			get;
			set;
		}

		[ProtoMember(9)]
		public float acc
		{
			get;
			set;
		}

		[ProtoMember(10)]
		public int miss
		{
			get;
			set;
		}

		[ProtoMember(11)]
		public string judge
		{
			get;
			set;
		}

		[ProtoMember(12)]
		public List<Beat> beats
		{
			get;
			set;
		}

		[ProtoMember(13)]
		public string steam_id
		{
			get;
			set;
		}

		[ProtoMember(14)]
		public string devices_id
		{
			get;
			set;
		}
	}
}
