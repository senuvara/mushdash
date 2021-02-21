using System;

namespace rail
{
	public class IRailFactoryImpl : RailObject, IRailFactory
	{
		internal IRailFactoryImpl(IntPtr cPtr)
		{
			swigCPtr_ = cPtr;
		}

		~IRailFactoryImpl()
		{
		}

		public virtual IRailPlayer RailPlayer()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailPlayer(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailPlayerImpl(intPtr) : null;
		}

		public virtual IRailUsersHelper RailUsersHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailUsersHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailUsersHelperImpl(intPtr) : null;
		}

		public virtual IRailFriends RailFriends()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailFriends(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailFriendsImpl(intPtr) : null;
		}

		public virtual IRailFloatingWindow RailFloatingWindow()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailFloatingWindow(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailFloatingWindowImpl(intPtr) : null;
		}

		public virtual IRailBrowserHelper RailBrowserHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailBrowserHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailBrowserHelperImpl(intPtr) : null;
		}

		public virtual IRailInGamePurchase RailInGamePurchase()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailInGamePurchase(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailInGamePurchaseImpl(intPtr) : null;
		}

		public virtual IRailInGameCoin RailInGameCoin()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailInGameCoin(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailInGameCoinImpl(intPtr) : null;
		}

		public virtual IRailRoomHelper RailRoomHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailRoomHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailRoomHelperImpl(intPtr) : null;
		}

		public virtual IRailGameServerHelper RailGameServerHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailGameServerHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailGameServerHelperImpl(intPtr) : null;
		}

		public virtual IRailStorageHelper RailStorageHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailStorageHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailStorageHelperImpl(intPtr) : null;
		}

		public virtual IRailUserSpaceHelper RailUserSpaceHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailUserSpaceHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailUserSpaceHelperImpl(intPtr) : null;
		}

		public virtual IRailStatisticHelper RailStatisticHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailStatisticHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailStatisticHelperImpl(intPtr) : null;
		}

		public virtual IRailLeaderboardHelper RailLeaderboardHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailLeaderboardHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailLeaderboardHelperImpl(intPtr) : null;
		}

		public virtual IRailAchievementHelper RailAchievementHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailAchievementHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailAchievementHelperImpl(intPtr) : null;
		}

		public virtual IRailNetwork RailNetworkHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailNetworkHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailNetworkImpl(intPtr) : null;
		}

		public virtual IRailApps RailApps()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailApps(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailAppsImpl(intPtr) : null;
		}

		public virtual IRailGame RailGame()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailGame(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailGameImpl(intPtr) : null;
		}

		public virtual IRailUtils RailUtils()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailUtils(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailUtilsImpl(intPtr) : null;
		}

		public virtual IRailAssetsHelper RailAssetsHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailAssetsHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailAssetsHelperImpl(intPtr) : null;
		}

		public virtual IRailDlcHelper RailDlcHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailDlcHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailDlcHelperImpl(intPtr) : null;
		}

		public virtual IRailScreenshotHelper RailScreenshotHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailScreenshotHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailScreenshotHelperImpl(intPtr) : null;
		}

		public virtual IRailVoiceHelper RailVoiceHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailVoiceHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailVoiceHelperImpl(intPtr) : null;
		}

		public virtual IRailSystemHelper RailSystemHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailSystemHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailSystemHelperImpl(intPtr) : null;
		}

		public virtual IRailTextInputHelper RailTextInputHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailTextInputHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailTextInputHelperImpl(intPtr) : null;
		}

		public virtual IRailIMEHelper RailIMETextInputHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailIMETextInputHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailIMEHelperImpl(intPtr) : null;
		}

		public virtual IRailHttpSessionHelper RailHttpSessionHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailHttpSessionHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailHttpSessionHelperImpl(intPtr) : null;
		}

		public virtual IRailSmallObjectServiceHelper RailSmallObjectServiceHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailSmallObjectServiceHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailSmallObjectServiceHelperImpl(intPtr) : null;
		}

		public virtual IRailZoneServerHelper RailZoneServerHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailZoneServerHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailZoneServerHelperImpl(intPtr) : null;
		}

		public virtual IRailGroupChatHelper RailGroupChatHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailGroupChatHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailGroupChatHelperImpl(intPtr) : null;
		}

		public virtual IRailInGameStorePurchaseHelper RailInGameStorePurchaseHelper()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailFactory_RailInGameStorePurchaseHelper(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailInGameStorePurchaseHelperImpl(intPtr) : null;
		}
	}
}
