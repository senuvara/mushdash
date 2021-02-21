using System;

namespace rail
{
	public class IRailLeaderboardHelperImpl : RailObject, IRailLeaderboardHelper
	{
		internal IRailLeaderboardHelperImpl(IntPtr cPtr)
		{
			swigCPtr_ = cPtr;
		}

		~IRailLeaderboardHelperImpl()
		{
		}

		public virtual IRailLeaderboard OpenLeaderboard(string leaderboard_name)
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailLeaderboardHelper_OpenLeaderboard(swigCPtr_, leaderboard_name);
			return (!(intPtr == IntPtr.Zero)) ? new IRailLeaderboardImpl(intPtr) : null;
		}

		public virtual IRailLeaderboard AsyncCreateLeaderboard(string leaderboard_name, LeaderboardSortType sort_type, LeaderboardDisplayType display_type, string user_data, out RailResult result)
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailLeaderboardHelper_AsyncCreateLeaderboard(swigCPtr_, leaderboard_name, (int)sort_type, (int)display_type, user_data, out result);
			return (!(intPtr == IntPtr.Zero)) ? new IRailLeaderboardImpl(intPtr) : null;
		}
	}
}
