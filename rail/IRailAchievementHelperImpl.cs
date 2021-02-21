using System;

namespace rail
{
	public class IRailAchievementHelperImpl : RailObject, IRailAchievementHelper
	{
		internal IRailAchievementHelperImpl(IntPtr cPtr)
		{
			swigCPtr_ = cPtr;
		}

		~IRailAchievementHelperImpl()
		{
		}

		public virtual IRailPlayerAchievement CreatePlayerAchievement(RailID player)
		{
			IntPtr intPtr = (!(player == null)) ? RAIL_API_PINVOKE.new_RailID__SWIG_0() : IntPtr.Zero;
			if (player != null)
			{
				RailConverter.Csharp2Cpp(player, intPtr);
			}
			try
			{
				IntPtr intPtr2 = RAIL_API_PINVOKE.IRailAchievementHelper_CreatePlayerAchievement(swigCPtr_, intPtr);
				return (!(intPtr2 == IntPtr.Zero)) ? new IRailPlayerAchievementImpl(intPtr2) : null;
			}
			finally
			{
				RAIL_API_PINVOKE.delete_RailID(intPtr);
			}
		}

		public virtual IRailGlobalAchievement GetGlobalAchievement()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailAchievementHelper_GetGlobalAchievement(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailGlobalAchievementImpl(intPtr) : null;
		}
	}
}
