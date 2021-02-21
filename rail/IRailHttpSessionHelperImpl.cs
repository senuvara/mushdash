using System;

namespace rail
{
	public class IRailHttpSessionHelperImpl : RailObject, IRailHttpSessionHelper
	{
		internal IRailHttpSessionHelperImpl(IntPtr cPtr)
		{
			swigCPtr_ = cPtr;
		}

		~IRailHttpSessionHelperImpl()
		{
		}

		public virtual IRailHttpSession CreateHttpSession()
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailHttpSessionHelper_CreateHttpSession(swigCPtr_);
			return (!(intPtr == IntPtr.Zero)) ? new IRailHttpSessionImpl(intPtr) : null;
		}

		public virtual IRailHttpResponse CreateHttpResponse(string http_response_data)
		{
			IntPtr intPtr = RAIL_API_PINVOKE.IRailHttpSessionHelper_CreateHttpResponse(swigCPtr_, http_response_data);
			return (!(intPtr == IntPtr.Zero)) ? new IRailHttpResponseImpl(intPtr) : null;
		}
	}
}
