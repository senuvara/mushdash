using System;

namespace rail
{
	public class IRailAssetsHelperImpl : RailObject, IRailAssetsHelper
	{
		internal IRailAssetsHelperImpl(IntPtr cPtr)
		{
			swigCPtr_ = cPtr;
		}

		~IRailAssetsHelperImpl()
		{
		}

		public virtual IRailAssets OpenAssets()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailAssetsHelper_OpenAssets(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailAssetsImpl(intPtr) : null;
		}

		public virtual IRailAssets OpenGameServerAssets()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailAssetsHelper_OpenGameServerAssets(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailAssetsImpl(intPtr) : null;
		}
	}
}
