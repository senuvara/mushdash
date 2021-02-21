using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using System.Collections.Generic;

public class AlbumTagName : Singleton<AlbumTagName>
{
	private Dictionary<string, string> m_AlbumCollectionsNames = new Dictionary<string, string>
	{
		{
			"English",
			"Favorite Music"
		},
		{
			"ChineseS",
			"我喜欢的音乐"
		},
		{
			"ChineseT",
			"我喜歡的音樂"
		},
		{
			"Japanese",
			"お気に入り"
		},
		{
			"Korean",
			"내가 좋아하는 음악"
		}
	};

	private Dictionary<string, string> m_AlbumAllNames = new Dictionary<string, string>
	{
		{
			"English",
			"All"
		},
		{
			"ChineseS",
			"全部"
		},
		{
			"ChineseT",
			"全部"
		},
		{
			"Japanese",
			"全部"
		},
		{
			"Korean",
			"전부"
		}
	};

	private Dictionary<string, string> m_AlbumDefaultNames = new Dictionary<string, string>
	{
		{
			"English",
			"Default Music"
		},
		{
			"ChineseS",
			"基础包"
		},
		{
			"ChineseT",
			"基礎包"
		},
		{
			"Japanese",
			"デフォルト曲"
		},
		{
			"Korean",
			"기본 패키지"
		}
	};

	private Dictionary<string, string> m_AlbumGiveUpNames = new Dictionary<string, string>
	{
		{
			"English",
			"Give Up TREATMENT"
		},
		{
			"ChineseS",
			"放弃治疗"
		},
		{
			"ChineseT",
			"放棄治療"
		},
		{
			"Japanese",
			"音ゲー依存症"
		},
		{
			"Korean",
			"치유는 포기했어"
		}
	};

	private Dictionary<string, string> m_AlbumHappyNames = new Dictionary<string, string>
	{
		{
			"English",
			"Happy Otaku Pack"
		},
		{
			"ChineseS",
			"肥宅快乐包"
		},
		{
			"ChineseT",
			"肥宅快樂包"
		},
		{
			"Japanese",
			"MUSIC快楽天"
		},
		{
			"Korean",
			"오타쿠의 쾌락 모음"
		}
	};

	private Dictionary<string, string> m_AlbumCuteNames = new Dictionary<string, string>
	{
		{
			"English",
			"Cute Is Everyting"
		},
		{
			"ChineseS",
			"可爱即正义"
		},
		{
			"ChineseT",
			"可愛即正義"
		},
		{
			"Japanese",
			"かわいいは正義"
		},
		{
			"Korean",
			"귀여움은 정의다"
		}
	};

	private Dictionary<string, string> m_AlbumRadioNames = new Dictionary<string, string>
	{
		{
			"English",
			"MUSE RADIO"
		},
		{
			"ChineseS",
			"暮 色 電 台"
		},
		{
			"ChineseT",
			"暮 色 電 台"
		},
		{
			"Japanese",
			"ミューズラジオ"
		},
		{
			"Korean",
			"뮤즈 라디오"
		}
	};

	private Dictionary<string, string> m_AlbumCollabNames = new Dictionary<string, string>
	{
		{
			"English",
			"Collab"
		},
		{
			"ChineseS",
			"联动"
		},
		{
			"ChineseT",
			"聯動"
		},
		{
			"Japanese",
			"コラボ"
		},
		{
			"Korean",
			"콜라보레이션"
		}
	};

	private Dictionary<string, string> m_AlbumHideNames = new Dictionary<string, string>
	{
		{
			"English",
			"Hidden"
		},
		{
			"ChineseS",
			"隐藏"
		},
		{
			"ChineseT",
			"隱藏"
		},
		{
			"Japanese",
			"非表示"
		},
		{
			"Korean",
			"숨김"
		}
	};

	private Dictionary<string, string> m_AlbumCyTusNames = new Dictionary<string, string>
	{
		{
			"English",
			"cyTus"
		},
		{
			"ChineseS",
			"cyTus"
		},
		{
			"ChineseT",
			"cyTus"
		},
		{
			"Japanese",
			"cyTus"
		},
		{
			"Korean",
			"cyTus"
		}
	};

	private Dictionary<string, string> m_AlbumRedNames = new Dictionary<string, string>
	{
		{
			"English",
			"HARDCORE TANO*C"
		},
		{
			"ChineseS",
			"HARDCORE TANO*C"
		},
		{
			"ChineseT",
			"HARDCORE TANO*C"
		},
		{
			"Japanese",
			"HARDCORE TANO*C"
		},
		{
			"Korean",
			"HARDCORE TANO*C"
		}
	};

	private Dictionary<string, string> m_RandomNames = new Dictionary<string, string>
	{
		{
			"English",
			"RANDOM MUSIC"
		},
		{
			"ChineseS",
			"随机歌曲"
		},
		{
			"ChineseT",
			"隨機歌曲"
		},
		{
			"Japanese",
			"ランダム"
		},
		{
			"Korean",
			"랜덤 곡"
		}
	};

	public string GetAlbumTagLocaliztion(string albumUid)
	{
		string activeOption = SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Language");
		if (albumUid != null)
		{
			if (_003C_003Ef__switch_0024map4 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(11);
				dictionary.Add("collections", 0);
				dictionary.Add("tag-1", 1);
				dictionary.Add("tag-2", 2);
				dictionary.Add("tag-3", 3);
				dictionary.Add("tag-4", 4);
				dictionary.Add("tag-5", 5);
				dictionary.Add("tag-6", 6);
				dictionary.Add("tag-7", 7);
				dictionary.Add("hide", 8);
				dictionary.Add("tag-new0", 9);
				dictionary.Add("tag-new1", 10);
				_003C_003Ef__switch_0024map4 = dictionary;
			}
			int value;
			if (_003C_003Ef__switch_0024map4.TryGetValue(albumUid, out value))
			{
				switch (value)
				{
				case 0:
					return m_AlbumCollectionsNames[activeOption];
				case 1:
					return m_AlbumAllNames[activeOption];
				case 2:
					return m_AlbumDefaultNames[activeOption];
				case 3:
					return m_AlbumGiveUpNames[activeOption];
				case 4:
					return m_AlbumHappyNames[activeOption];
				case 5:
					return m_AlbumCuteNames[activeOption];
				case 6:
					return m_AlbumRadioNames[activeOption];
				case 7:
					return m_AlbumCollabNames[activeOption];
				case 8:
					return m_AlbumHideNames[activeOption];
				case 9:
					return m_AlbumCyTusNames[activeOption];
				case 10:
					return m_AlbumRedNames[activeOption];
				}
			}
		}
		return string.Empty;
	}

	public string GetAlbumDifficultyLocaliztion()
	{
		string activeOption = SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Language");
		return m_AlbumAllNames[activeOption];
	}

	public string GetRandomNameLocaliztion()
	{
		string activeOption = SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Language");
		return m_RandomNames[activeOption];
	}
}
