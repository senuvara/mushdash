using rail;
using UnityEngine;

[DisallowMultipleComponent]
public class RailManager : MonoBehaviour
{
	private static RailManager instance_;

	private static bool ever_initialize_;

	private bool initialized_;

	private static RailManager Instance
	{
		get
		{
			if (instance_ == null)
			{
				return new GameObject("RailManager").AddComponent<RailManager>();
			}
			return instance_;
		}
	}

	public static bool Initialized => Instance.initialized_;

	private void Awake()
	{
		if (instance_ != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance_ = this;
		if (ever_initialize_)
		{
			Debug.LogError("Tried to Initialize the RailSDK twice in one session!");
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		RailGameID game_id = new RailGameID(2000005uL);
		if (rail_api.RailNeedRestartAppForCheckingEnvironment(game_id, 1, new string[1]
		{
			string.Empty
		}))
		{
			Debug.LogError("RailNeedRestartAppForCheckingEnvironment return true!");
			Application.Quit();
			return;
		}
		initialized_ = rail_api.RailInitialize();
		if (!initialized_)
		{
			Debug.LogError("RailInitialize failed!");
			Application.Quit();
		}
		else
		{
			RailCallBackHelper.Instance.RegisterCallback(RAILEventID.kRailEventSystemStateChanged, OnRailEvent);
			ever_initialize_ = true;
		}
	}

	public void OnRailEvent(RAILEventID id, EventBase data)
	{
		if (data.result == RailResult.kSuccess && id == RAILEventID.kRailEventSystemStateChanged)
		{
			RailSystemStateChanged railSystemStateChanged = (RailSystemStateChanged)data;
			if (railSystemStateChanged.state == RailSystemState.kSystemStatePlatformOffline || railSystemStateChanged.state == RailSystemState.kSystemStatePlatformExit)
			{
				Application.Quit();
			}
		}
	}

	private void OnEnable()
	{
		if (instance_ == null)
		{
			instance_ = this;
		}
	}

	private void OnDestroy()
	{
		if (!(instance_ != this))
		{
			instance_ = null;
			if (initialized_)
			{
				RailCallBackHelper.Instance.UnregisterAllCallback();
				rail_api.RailFinalize();
			}
		}
	}

	private void Update()
	{
		if (initialized_)
		{
			rail_api.RailFireEvents();
		}
	}
}
