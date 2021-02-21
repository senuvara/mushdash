using System;
using System.Runtime.InteropServices;

namespace Discord
{
	public class Discord : IDisposable
	{
		internal struct FFIEvents
		{
		}

		internal struct FFIMethods
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void DestroyHandler(IntPtr MethodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate Result RunCallbacksMethod(IntPtr methodsPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookCallback(IntPtr ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void SetLogHookMethod(IntPtr methodsPtr, LogLevel minLevel, IntPtr callbackData, SetLogHookCallback callback);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetApplicationManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetUserManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetImageManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetActivityManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetRelationshipManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetLobbyManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetNetworkManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetOverlayManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStorageManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetStoreManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetVoiceManagerMethod(IntPtr discordPtr);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr GetAchievementManagerMethod(IntPtr discordPtr);

			internal DestroyHandler Destroy;

			internal RunCallbacksMethod RunCallbacks;

			internal SetLogHookMethod SetLogHook;

			internal GetApplicationManagerMethod GetApplicationManager;

			internal GetUserManagerMethod GetUserManager;

			internal GetImageManagerMethod GetImageManager;

			internal GetActivityManagerMethod GetActivityManager;

			internal GetRelationshipManagerMethod GetRelationshipManager;

			internal GetLobbyManagerMethod GetLobbyManager;

			internal GetNetworkManagerMethod GetNetworkManager;

			internal GetOverlayManagerMethod GetOverlayManager;

			internal GetStorageManagerMethod GetStorageManager;

			internal GetStoreManagerMethod GetStoreManager;

			internal GetVoiceManagerMethod GetVoiceManager;

			internal GetAchievementManagerMethod GetAchievementManager;
		}

		internal struct FFICreateParams
		{
			internal long ClientId;

			internal ulong Flags;

			internal IntPtr Events;

			internal IntPtr EventData;

			internal IntPtr ApplicationEvents;

			internal uint ApplicationVersion;

			internal IntPtr UserEvents;

			internal uint UserVersion;

			internal IntPtr ImageEvents;

			internal uint ImageVersion;

			internal IntPtr ActivityEvents;

			internal uint ActivityVersion;

			internal IntPtr RelationshipEvents;

			internal uint RelationshipVersion;

			internal IntPtr LobbyEvents;

			internal uint LobbyVersion;

			internal IntPtr NetworkEvents;

			internal uint NetworkVersion;

			internal IntPtr OverlayEvents;

			internal uint OverlayVersion;

			internal IntPtr StorageEvents;

			internal uint StorageVersion;

			internal IntPtr StoreEvents;

			internal uint StoreVersion;

			internal IntPtr VoiceEvents;

			internal uint VoiceVersion;

			internal IntPtr AchievementEvents;

			internal uint AchievementVersion;
		}

		public delegate void SetLogHookHandler(LogLevel level, string message);

		private GCHandle SelfHandle;

		private IntPtr EventsPtr;

		private FFIEvents Events;

		private IntPtr ApplicationEventsPtr;

		private ApplicationManager.FFIEvents ApplicationEvents;

		internal ApplicationManager ApplicationManagerInstance;

		private IntPtr UserEventsPtr;

		private UserManager.FFIEvents UserEvents;

		internal UserManager UserManagerInstance;

		private IntPtr ImageEventsPtr;

		private ImageManager.FFIEvents ImageEvents;

		internal ImageManager ImageManagerInstance;

		private IntPtr ActivityEventsPtr;

		private ActivityManager.FFIEvents ActivityEvents;

		internal ActivityManager ActivityManagerInstance;

		private IntPtr RelationshipEventsPtr;

		private RelationshipManager.FFIEvents RelationshipEvents;

		internal RelationshipManager RelationshipManagerInstance;

		private IntPtr LobbyEventsPtr;

		private LobbyManager.FFIEvents LobbyEvents;

		internal LobbyManager LobbyManagerInstance;

		private IntPtr NetworkEventsPtr;

		private NetworkManager.FFIEvents NetworkEvents;

		internal NetworkManager NetworkManagerInstance;

		private IntPtr OverlayEventsPtr;

		private OverlayManager.FFIEvents OverlayEvents;

		internal OverlayManager OverlayManagerInstance;

		private IntPtr StorageEventsPtr;

		private StorageManager.FFIEvents StorageEvents;

		internal StorageManager StorageManagerInstance;

		private IntPtr StoreEventsPtr;

		private StoreManager.FFIEvents StoreEvents;

		internal StoreManager StoreManagerInstance;

		private IntPtr VoiceEventsPtr;

		private VoiceManager.FFIEvents VoiceEvents;

		internal VoiceManager VoiceManagerInstance;

		private IntPtr AchievementEventsPtr;

		private AchievementManager.FFIEvents AchievementEvents;

		internal AchievementManager AchievementManagerInstance;

		private IntPtr MethodsPtr;

		private object MethodsStructure;

		private GCHandle? setLogHook;

		public Result IsInit;

		private FFIMethods Methods
		{
			get
			{
				if (MethodsStructure == null)
				{
					MethodsStructure = Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods));
				}
				return (FFIMethods)MethodsStructure;
			}
		}

		public Discord(long clientId, ulong flags)
		{
			FFICreateParams createParams = default(FFICreateParams);
			createParams.ClientId = clientId;
			createParams.Flags = flags;
			Events = default(FFIEvents);
			EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Events));
			createParams.Events = EventsPtr;
			SelfHandle = GCHandle.Alloc(this);
			createParams.EventData = GCHandle.ToIntPtr(SelfHandle);
			ApplicationEvents = default(ApplicationManager.FFIEvents);
			ApplicationEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ApplicationEvents));
			createParams.ApplicationEvents = ApplicationEventsPtr;
			createParams.ApplicationVersion = 1u;
			UserEvents = default(UserManager.FFIEvents);
			UserEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UserEvents));
			createParams.UserEvents = UserEventsPtr;
			createParams.UserVersion = 1u;
			ImageEvents = default(ImageManager.FFIEvents);
			ImageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ImageEvents));
			createParams.ImageEvents = ImageEventsPtr;
			createParams.ImageVersion = 1u;
			ActivityEvents = default(ActivityManager.FFIEvents);
			ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ActivityEvents));
			createParams.ActivityEvents = ActivityEventsPtr;
			createParams.ActivityVersion = 1u;
			RelationshipEvents = default(RelationshipManager.FFIEvents);
			RelationshipEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(RelationshipEvents));
			createParams.RelationshipEvents = RelationshipEventsPtr;
			createParams.RelationshipVersion = 1u;
			LobbyEvents = default(LobbyManager.FFIEvents);
			LobbyEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(LobbyEvents));
			createParams.LobbyEvents = LobbyEventsPtr;
			createParams.LobbyVersion = 1u;
			NetworkEvents = default(NetworkManager.FFIEvents);
			NetworkEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(NetworkEvents));
			createParams.NetworkEvents = NetworkEventsPtr;
			createParams.NetworkVersion = 1u;
			OverlayEvents = default(OverlayManager.FFIEvents);
			OverlayEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(OverlayEvents));
			createParams.OverlayEvents = OverlayEventsPtr;
			createParams.OverlayVersion = 1u;
			StorageEvents = default(StorageManager.FFIEvents);
			StorageEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(StorageEvents));
			createParams.StorageEvents = StorageEventsPtr;
			createParams.StorageVersion = 1u;
			StoreEvents = default(StoreManager.FFIEvents);
			StoreEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(StoreEvents));
			createParams.StoreEvents = StoreEventsPtr;
			createParams.StoreVersion = 1u;
			VoiceEvents = default(VoiceManager.FFIEvents);
			VoiceEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(VoiceEvents));
			createParams.VoiceEvents = VoiceEventsPtr;
			createParams.VoiceVersion = 1u;
			AchievementEvents = default(AchievementManager.FFIEvents);
			AchievementEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(AchievementEvents));
			createParams.AchievementEvents = AchievementEventsPtr;
			createParams.AchievementVersion = 1u;
			InitEvents(EventsPtr, ref Events);
			if ((IsInit = DiscordCreate(2u, ref createParams, out MethodsPtr)) != 0)
			{
				Dispose();
			}
		}

		[DllImport("discord_game_sdk", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
		private static extern Result DiscordCreate(uint version, ref FFICreateParams createParams, out IntPtr manager);

		private void InitEvents(IntPtr eventsPtr, ref FFIEvents events)
		{
			Marshal.StructureToPtr(events, eventsPtr, false);
		}

		public void Dispose()
		{
			if (MethodsPtr != IntPtr.Zero)
			{
				FFIMethods methods = Methods;
				methods.Destroy(MethodsPtr);
			}
			SelfHandle.Free();
			Marshal.FreeHGlobal(EventsPtr);
			Marshal.FreeHGlobal(ApplicationEventsPtr);
			Marshal.FreeHGlobal(UserEventsPtr);
			Marshal.FreeHGlobal(ImageEventsPtr);
			Marshal.FreeHGlobal(ActivityEventsPtr);
			Marshal.FreeHGlobal(RelationshipEventsPtr);
			Marshal.FreeHGlobal(LobbyEventsPtr);
			Marshal.FreeHGlobal(NetworkEventsPtr);
			Marshal.FreeHGlobal(OverlayEventsPtr);
			Marshal.FreeHGlobal(StorageEventsPtr);
			Marshal.FreeHGlobal(StoreEventsPtr);
			Marshal.FreeHGlobal(VoiceEventsPtr);
			Marshal.FreeHGlobal(AchievementEventsPtr);
			if (setLogHook.HasValue)
			{
				setLogHook.Value.Free();
			}
		}

		public bool RunCallbacks()
		{
			FFIMethods methods = Methods;
			if (methods.RunCallbacks(MethodsPtr) != 0)
			{
				return false;
			}
			return true;
		}

		[MonoPInvokeCallback]
		private static void SetLogHookCallbackImpl(IntPtr ptr, LogLevel level, string message)
		{
			SetLogHookHandler setLogHookHandler = (SetLogHookHandler)GCHandle.FromIntPtr(ptr).Target;
			setLogHookHandler(level, message);
		}

		public void SetLogHook(LogLevel minLevel, SetLogHookHandler callback)
		{
			if (setLogHook.HasValue)
			{
				setLogHook.Value.Free();
			}
			setLogHook = GCHandle.Alloc(callback);
			FFIMethods methods = Methods;
			methods.SetLogHook(MethodsPtr, minLevel, GCHandle.ToIntPtr(setLogHook.Value), SetLogHookCallbackImpl);
		}

		public ApplicationManager GetApplicationManager()
		{
			if (ApplicationManagerInstance == null)
			{
				FFIMethods methods = Methods;
				ApplicationManagerInstance = new ApplicationManager(methods.GetApplicationManager(MethodsPtr), ApplicationEventsPtr, ref ApplicationEvents);
			}
			return ApplicationManagerInstance;
		}

		public UserManager GetUserManager()
		{
			if (UserManagerInstance == null)
			{
				FFIMethods methods = Methods;
				UserManagerInstance = new UserManager(methods.GetUserManager(MethodsPtr), UserEventsPtr, ref UserEvents);
			}
			return UserManagerInstance;
		}

		public ImageManager GetImageManager()
		{
			if (ImageManagerInstance == null)
			{
				FFIMethods methods = Methods;
				ImageManagerInstance = new ImageManager(methods.GetImageManager(MethodsPtr), ImageEventsPtr, ref ImageEvents);
			}
			return ImageManagerInstance;
		}

		public ActivityManager GetActivityManager()
		{
			if (ActivityManagerInstance == null)
			{
				FFIMethods methods = Methods;
				ActivityManagerInstance = new ActivityManager(methods.GetActivityManager(MethodsPtr), ActivityEventsPtr, ref ActivityEvents);
			}
			return ActivityManagerInstance;
		}

		public RelationshipManager GetRelationshipManager()
		{
			if (RelationshipManagerInstance == null)
			{
				FFIMethods methods = Methods;
				RelationshipManagerInstance = new RelationshipManager(methods.GetRelationshipManager(MethodsPtr), RelationshipEventsPtr, ref RelationshipEvents);
			}
			return RelationshipManagerInstance;
		}

		public LobbyManager GetLobbyManager()
		{
			if (LobbyManagerInstance == null)
			{
				FFIMethods methods = Methods;
				LobbyManagerInstance = new LobbyManager(methods.GetLobbyManager(MethodsPtr), LobbyEventsPtr, ref LobbyEvents);
			}
			return LobbyManagerInstance;
		}

		public NetworkManager GetNetworkManager()
		{
			if (NetworkManagerInstance == null)
			{
				FFIMethods methods = Methods;
				NetworkManagerInstance = new NetworkManager(methods.GetNetworkManager(MethodsPtr), NetworkEventsPtr, ref NetworkEvents);
			}
			return NetworkManagerInstance;
		}

		public OverlayManager GetOverlayManager()
		{
			if (OverlayManagerInstance == null)
			{
				FFIMethods methods = Methods;
				OverlayManagerInstance = new OverlayManager(methods.GetOverlayManager(MethodsPtr), OverlayEventsPtr, ref OverlayEvents);
			}
			return OverlayManagerInstance;
		}

		public StorageManager GetStorageManager()
		{
			if (StorageManagerInstance == null)
			{
				FFIMethods methods = Methods;
				StorageManagerInstance = new StorageManager(methods.GetStorageManager(MethodsPtr), StorageEventsPtr, ref StorageEvents);
			}
			return StorageManagerInstance;
		}

		public StoreManager GetStoreManager()
		{
			if (StoreManagerInstance == null)
			{
				FFIMethods methods = Methods;
				StoreManagerInstance = new StoreManager(methods.GetStoreManager(MethodsPtr), StoreEventsPtr, ref StoreEvents);
			}
			return StoreManagerInstance;
		}

		public VoiceManager GetVoiceManager()
		{
			if (VoiceManagerInstance == null)
			{
				FFIMethods methods = Methods;
				VoiceManagerInstance = new VoiceManager(methods.GetVoiceManager(MethodsPtr), VoiceEventsPtr, ref VoiceEvents);
			}
			return VoiceManagerInstance;
		}

		public AchievementManager GetAchievementManager()
		{
			if (AchievementManagerInstance == null)
			{
				FFIMethods methods = Methods;
				AchievementManagerInstance = new AchievementManager(methods.GetAchievementManager(MethodsPtr), AchievementEventsPtr, ref AchievementEvents);
			}
			return AchievementManagerInstance;
		}
	}
}
