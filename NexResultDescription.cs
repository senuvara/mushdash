public class NexResultDescription
{
	public enum ErrorType
	{
		None,
		Logout,
		Busy,
		NoNet,
		NoNsa
	}

	public static bool isUnknowError;

	public static bool isSuccess(int des)
	{
		return des == 0;
	}

	public static ErrorType GetErrorType(int des)
	{
		return ErrorType.None;
	}
}
