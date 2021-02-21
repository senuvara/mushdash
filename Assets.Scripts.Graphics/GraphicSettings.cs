using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
	public class GraphicSettings
	{
		private static int m_IdPeroTime;

		private static float m_StartTime;

		public static int originScreenWidth;

		public static int originScreenHeight;

		public static int curScreenWidth;

		public static int curScreenHeight;

		private static int m_PassFrame;

		private static float m_PassTime;

		private static int m_FrameCount;

		private const uint SWP_SHOWWINDOW = 64u;

		private const int GWL_STYLE = -16;

		private const int WS_BORDER = 1;

		private const int WS_POPUP = 8388608;

		private const int WS_CAPTION = 12582912;

		private static float[] factor = new float[3]
		{
			0.5f,
			0.75f,
			1f
		};

		public static bool isFrameOverOneHundred => m_FrameCount >= 100;

		public static bool isOverOneHundred => Application.targetFrameRate >= 100 || Application.targetFrameRate == -1;

		private static void GetCurrentFrame(float t)
		{
			if (Singleton<StageBattleComponent>.instance.isInGame)
			{
				m_PassFrame++;
				m_PassTime += Time.unscaledDeltaTime;
				m_FrameCount = Mathf.RoundToInt((float)m_PassFrame / m_PassTime);
				m_PassFrame = 0;
				m_PassTime = 0f;
			}
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll")]
		private static extern int EnumWindows(CallBackPtr callPtr, int lPar);

		[DllImport("user32.dll")]
		private static extern int GetClassName(int hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32")]
		public static extern int GetWindowText(int hwnd, StringBuilder lptrString, int nMaxCount);

		private static IntPtr GetForegroundWindow()
		{
			return new IntPtr(FindWindow("Muse Dash", "UnityWndClass"));
		}

		public static void Init()
		{
			curScreenWidth = (originScreenWidth = Screen.width);
			curScreenHeight = (originScreenHeight = Screen.height);
			m_IdPeroTime = Shader.PropertyToID("_Pero_Time");
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(default(Guid).ToString(), Update, UnityGameManager.LoopType.Update);
			ShaderVariantCollection shaderVariantCollection = Singleton<AssetBundleManager>.instance.LoadFromName<ShaderVariantCollection>("_WarmUpShaders");
			shaderVariantCollection.WarmUp();
			string result = Singleton<DataManager>.instance["GameConfig"]["ScreenResolutions"].GetResult<string>();
			int num = int.Parse(result.BeginBefore('x'));
			int num2 = int.Parse(result.LastAfter('x'));
			bool result2 = Singleton<DataManager>.instance["GameConfig"]["FullScreen"].GetResult<bool>();
			if (Singleton<DataManager>.instance["GameConfig"]["HasBorder"].GetResult<bool>())
			{
				SetResolution(num, num2, result2);
			}
			else
			{
				SetNoBorder(num, num2);
			}
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("GetCurrentFrame", GetCurrentFrame, UnityGameManager.LoopType.Update);
		}

		public static int FindWindow(string Title, string ClassName)
		{
			int TargetHwnd = 0;
			CallBackPtr callPtr = delegate(int hwnd, int lParam)
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				StringBuilder stringBuilder2 = new StringBuilder(512);
				GetClassName(hwnd, stringBuilder, stringBuilder.Capacity);
				GetWindowText(hwnd, stringBuilder2, stringBuilder2.Capacity);
				if (stringBuilder.ToString().Equals(ClassName, StringComparison.InvariantCultureIgnoreCase) && stringBuilder2.ToString().Equals(Title, StringComparison.InvariantCultureIgnoreCase))
				{
					TargetHwnd = hwnd;
				}
				return true;
			};
			EnumWindows(callPtr, 0);
			return TargetHwnd;
		}

		private static void SetQualityLevel(int level)
		{
			int width = Screen.width;
			int height = Screen.height;
			SetResolution(width, height, true);
		}

		public static void SetResolution(int w, int h, bool fullScreen)
		{
			curScreenWidth = w;
			curScreenHeight = h;
			Screen.SetResolution(w, h, fullScreen, 60);
			Debug.LogFormat("[Graphic] Screen Resolution Adjust before : [{0},{1}] , after : [{2},{3}]", originScreenWidth, originScreenHeight, w, h);
		}

		public static void SetNoBorder(int width, int height)
		{
			int num = Screen.currentResolution.width - width;
			int num2 = Screen.currentResolution.height - height;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			SetWindowLong(GetForegroundWindow(), -16, 8388609);
			SetWindowPos(GetForegroundWindow(), 0, num / 2, num2 / 2, width, height, 64u);
		}

		public static void SetHasBorder(int width, int height)
		{
			int num = Screen.currentResolution.width - width;
			int num2 = Screen.currentResolution.height - height;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			SetWindowLong(GetForegroundWindow(), -16, 12582913);
			SetWindowPos(GetForegroundWindow(), 0, num / 2, num2 / 2, width, height, 64u);
		}

		public static int GetRecommandEffectCode()
		{
			return 1;
		}

		public static int GetRecommandQualityCode()
		{
			Debug.LogFormat("[GraphicSetting] Graphic Memory : {0}.", SystemInfo.graphicsMemorySize);
			return 2;
		}

		public static void SetFps(int fps, bool vsnyc)
		{
			QualitySettings.vSyncCount = (vsnyc ? 1 : 0);
			Application.targetFrameRate = fps;
		}

		private static void Update(float arg0)
		{
			Shader.SetGlobalFloat(m_IdPeroTime, m_StartTime += ((!Singleton<StageBattleComponent>.instance.isPause) ? Time.deltaTime : 0f));
		}

		public static void Reset()
		{
		}
	}
}
