using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class SteamGoogleLogin : SingletonMonoBehaviour<SteamGoogleLogin>
{
	private HttpListener listener;

	private Thread listenerThread;

	private UnityWebRequest m_WebRequest;

	public bool m_IsGoogleLogin;

	private static bool m_IsInited;

	[SerializeField]
	private bool m_CanRunCallbackWebRequest;

	private bool m_WhileRunCallbackWebRequest;

	private bool m_CanRunGetOpenIdRequest;

	private bool m_WhileRunGetOpenId;

	private bool m_CanRunTxwyRequest;

	private bool m_WhileCanRunTxwy;

	private bool m_WhileRunFail;

	private bool m_StartListener;

	private IAsyncResult m_Result;

	private string m_Prefix = "/oauth/google";

	private string base_login_url = "https://accounts.google.com/o/oauth2/auth?access_type=offline";

	private string redirect_url = "http://localhost:23333/oauth/google/callback";

	private string scope = "openid email";

	private string google_appID = "648101838039-t1750rkdag75bs1dghpn37dmtb17bgrk.apps.googleusercontent.com";

	private string google_appKey = "YdH1ql_M7ZMLlK_zwxz1WrV3";

	private string stateKey;

	private string access_token_url = "https://accounts.google.com/o/oauth2/token";

	private string m_NewCode;

	private string m_NewState;

	private string m_CallbackDataBody;

	private Dictionary<string, string> m_body = new Dictionary<string, string>();

	private bool m_IsCallbackCheckOver;

	private bool m_IsCallBackPostOver;

	private bool m_IsCallBackPostFail;

	private string m_CallbackContext;

	private string m_AccessToken;

	private string m_IdToken;

	private string m_OpenId;

	private string m_Email;

	private string m_Sid;

	private bool m_RunCallback;

	private bool m_RunGetOpenId;

	private bool m_RunTxwy;

	private string m_AuthEmail;

	private string m_AuthUserId;

	private string m_BaseOpenIdUrl;

	private bool m_IsGetOpenIdCheckOver;

	private bool m_IsGetOpenIdRunOver;

	private bool m_RunGetOpenIdFail;

	private string m_TxwyId = "161513";

	private string m_TxwyAppKey = "5a018829d087d55d5d28c5fe373cb0ea";

	private string m_TxwyParams;

	private bool m_IsGetTxwyCheckOver;

	private bool m_IsGetTxwyRunOver;

	private bool m_RunTxwyFail;

	private bool m_IsRunPlayBackGameCheckOver;

	public bool m_IsRunPlayBackGameOver;

	private bool m_IsFinishToShow;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnWebInit()
	{
		if (!m_IsInited)
		{
			m_IsInited = true;
			listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:23333/");
			listener.Prefixes.Add("http://127.0.0.1:23333/");
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
			listener.Start();
			listenerThread = new Thread(startListener);
			listenerThread.Start();
			m_StartListener = true;
			Debug.Log("Server Started");
		}
	}

	public void OpenWeb()
	{
		m_IsGoogleLogin = true;
		m_StartListener = true;
		m_CanRunTxwyRequest = false;
		m_IsCallbackCheckOver = false;
		m_IsCallBackPostOver = false;
		m_IsCallBackPostFail = false;
		m_RunGetOpenIdFail = false;
		m_RunTxwyFail = false;
		m_RunCallback = false;
		m_RunGetOpenId = false;
		m_RunTxwy = false;
		m_CanRunCallbackWebRequest = false;
		m_IsGetOpenIdCheckOver = false;
		m_IsGetOpenIdRunOver = false;
		m_CanRunGetOpenIdRequest = false;
		m_CanRunTxwyRequest = false;
		m_WhileRunCallbackWebRequest = false;
		m_WhileCanRunTxwy = false;
		m_WhileRunGetOpenId = false;
		m_IsGetTxwyCheckOver = false;
		m_IsGetTxwyRunOver = false;
		m_IsRunPlayBackGameCheckOver = false;
		m_IsRunPlayBackGameOver = false;
		StartCoroutine(WaitWebInit());
	}

	public void OnWebEnd()
	{
		m_IsCallbackCheckOver = true;
		m_IsGetOpenIdCheckOver = true;
		m_IsGetTxwyCheckOver = true;
		m_IsRunPlayBackGameCheckOver = true;
		m_WhileRunCallbackWebRequest = true;
		m_WhileRunGetOpenId = true;
		m_WhileCanRunTxwy = true;
	}

	private IEnumerator WaitWebInit()
	{
		yield return new WaitForSeconds(0.1f);
		Application.OpenURL("http://localhost:23333/oauth/google/login");
	}

	private void Update()
	{
		if (!m_WhileRunCallbackWebRequest && m_CanRunCallbackWebRequest)
		{
			m_WhileRunCallbackWebRequest = true;
			m_WebRequest = UnityWebRequest.Post(access_token_url, m_body);
			m_RunCallback = true;
			m_WebRequest.SendWebRequest().completed += OnWebRequestCompleted;
		}
		if (!m_WhileRunGetOpenId && m_CanRunGetOpenIdRequest)
		{
			m_WhileRunGetOpenId = true;
			m_WebRequest = UnityWebRequest.Get(m_BaseOpenIdUrl);
			m_RunGetOpenId = true;
			m_WebRequest.SendWebRequest().completed += OnWebRequestCompleted;
		}
		if (!m_WhileCanRunTxwy && m_CanRunTxwyRequest)
		{
			m_WhileCanRunTxwy = true;
			m_WebRequest = UnityWebRequest.Get(m_TxwyParams);
			m_RunTxwy = true;
			m_WebRequest.SendWebRequest().completed += OnWebRequestCompleted;
		}
	}

	private void startListener()
	{
		while (m_StartListener)
		{
			m_Result = listener.BeginGetContext(ListenerCallback, listener);
			m_Result.AsyncWaitHandle.WaitOne();
		}
	}

	private void OnDestroy()
	{
		m_StartListener = false;
	}

	private void ListenerCallback(IAsyncResult result)
	{
		HttpListenerContext httpListenerContext = listener.EndGetContext(result);
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/login")
		{
			stateKey = GetRandomString();
			string str = $"{base_login_url}&redirect_uri={redirect_url}&response_type=code&client_id={google_appID}&scope={scope}&state={stateKey}";
			string value = "<html><head><meta charset = \"UTF-8\"><title>Loading...</title></head><body onload = 'setTimeout(\"OnLogin()\",100)'><h1 align=\"center\">Loading...</h1 ><script>function OnLogin(){window.location.href='" + str + "';}</script></body></html>";
			StreamWriter streamWriter = new StreamWriter(httpListenerContext.Response.OutputStream);
			streamWriter.WriteLine(value);
			streamWriter.Flush();
			streamWriter.Close();
		}
		if (httpListenerContext.Request.QueryString.AllKeys.Length > 0)
		{
			string[] allKeys = httpListenerContext.Request.QueryString.AllKeys;
			foreach (string text in allKeys)
			{
				Debug.Log("Key: " + text + ", Value: " + httpListenerContext.Request.QueryString.GetValues(text)[0]);
			}
			if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/callback")
			{
				IsCallBack(httpListenerContext);
			}
		}
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/auth")
		{
			IsAuth(httpListenerContext);
		}
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/txwy")
		{
			IsTxwy(httpListenerContext);
		}
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/playbackgame")
		{
			IsPlayBackGame(httpListenerContext);
		}
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/finish")
		{
			IsFinish(httpListenerContext);
		}
		if (httpListenerContext.Request.Url.LocalPath == m_Prefix + "/fail")
		{
			IsFail(httpListenerContext);
		}
		if (httpListenerContext.Request.HttpMethod == "POST")
		{
			Thread.Sleep(1000);
			string message = new StreamReader(httpListenerContext.Request.InputStream, httpListenerContext.Request.ContentEncoding).ReadToEnd();
			Debug.Log(message);
		}
		httpListenerContext.Response.Close();
	}

	private void IsCallBack(HttpListenerContext context)
	{
		string[] allKeys = context.Request.QueryString.AllKeys;
		foreach (string text in allKeys)
		{
			Debug.Log("Key: " + text + ", Value: " + context.Request.QueryString.GetValues(text)[0]);
			if (text == "code")
			{
				m_NewCode = context.Request.QueryString.GetValues(text)[0];
			}
			if (text == "state")
			{
				m_NewState = context.Request.QueryString.GetValues(text)[0];
			}
		}
		if (!(m_NewState == stateKey))
		{
			return;
		}
		m_body.Clear();
		m_body.Add("code", m_NewCode);
		m_body.Add("client_id", google_appID);
		m_body.Add("client_secret", google_appKey);
		m_body.Add("redirect_uri", redirect_url);
		m_body.Add("grant_type", "authorization_code");
		m_CanRunCallbackWebRequest = true;
		while (!m_IsCallbackCheckOver)
		{
			if (m_IsCallBackPostOver)
			{
				if (!m_IsCallBackPostFail)
				{
					context.Response.Redirect("/oauth/google/auth");
				}
				else
				{
					context.Response.Redirect("/oauth/google/fail");
				}
				m_IsCallbackCheckOver = true;
			}
		}
	}

	private void OnWebRequestCompleted(AsyncOperation ao)
	{
		if (m_WebRequest.responseCode == 200)
		{
			if (m_RunCallback)
			{
				m_CallbackContext = m_WebRequest.downloadHandler.text;
				m_AccessToken = JsonUtils.Deserialize<JObject>(m_CallbackContext)["access_token"].ToString();
				m_IdToken = JsonUtils.Deserialize<JObject>(m_CallbackContext)["id_token"].ToString();
				m_IsCallBackPostOver = true;
				m_RunCallback = false;
			}
			if (m_RunGetOpenId)
			{
				m_CallbackContext = m_WebRequest.downloadHandler.text;
				m_OpenId = JsonUtils.Deserialize<JObject>(m_CallbackContext)["id"].ToString();
				m_Email = JsonUtils.Deserialize<JObject>(m_CallbackContext)["email"].ToString();
				m_IsGetOpenIdRunOver = true;
				m_RunGetOpenId = false;
			}
			if (m_RunTxwy)
			{
				m_CallbackContext = m_WebRequest.downloadHandler.text;
				m_Sid = JsonUtils.Deserialize<JObject>(m_CallbackContext)["sid"].ToString();
				if (m_Sid != string.Empty)
				{
					m_IsGetTxwyRunOver = true;
					Singleton<XDSDKManager>.instance.OnOSLoginSuccess(m_Sid);
				}
				else
				{
					m_RunTxwyFail = true;
					m_IsGetTxwyRunOver = true;
				}
				m_RunTxwy = false;
			}
		}
		else
		{
			if (m_RunCallback)
			{
				m_IsCallBackPostFail = true;
				m_IsCallBackPostOver = true;
				m_RunCallback = false;
			}
			else if (m_RunGetOpenId)
			{
				m_RunGetOpenIdFail = true;
				m_IsGetOpenIdRunOver = true;
				m_RunGetOpenId = false;
			}
			else if (m_RunTxwy)
			{
				m_RunTxwyFail = true;
				m_IsGetTxwyRunOver = true;
				m_RunTxwy = false;
			}
			Singleton<XDSDKManager>.instance.OnGoogleLoginFail();
		}
		Debug.Log("comp" + m_WebRequest.responseCode + "--" + m_WebRequest.downloadHandler.text);
	}

	private void IsAuth(HttpListenerContext context)
	{
		GetOpenId(context, m_AccessToken);
	}

	private void GetOpenId(HttpListenerContext context, string accessToken)
	{
		m_BaseOpenIdUrl = $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}";
		m_CanRunGetOpenIdRequest = true;
		while (!m_IsGetOpenIdCheckOver)
		{
			if (m_IsGetOpenIdRunOver)
			{
				if (!m_RunGetOpenIdFail)
				{
					context.Response.Redirect("/oauth/google/txwy");
				}
				else
				{
					context.Response.Redirect("/oauth/google/fail");
				}
				m_IsGetOpenIdCheckOver = true;
			}
		}
	}

	private void IsTxwy(HttpListenerContext context)
	{
		string text = "https://p.txwy.tw/rest/v1/mobile/xdlogin";
		string text2 = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000000).ToString();
		string strToEncrypt = m_TxwyId + m_OpenId + text2;
		string str = Md5Sum(strToEncrypt);
		string text3 = Md5Sum(str + m_TxwyAppKey);
		m_TxwyParams = text + "?appid=" + m_TxwyId + "&openid=" + m_OpenId + "&sign=" + text3 + "&time=" + text2 + "&type=google";
		m_CanRunTxwyRequest = true;
		while (!m_IsGetTxwyCheckOver)
		{
			if (m_IsGetTxwyRunOver)
			{
				if (!m_RunTxwyFail)
				{
					context.Response.Redirect("/oauth/google/playbackgame");
				}
				else
				{
					context.Response.Redirect("/oauth/google/fail");
				}
				m_IsGetTxwyCheckOver = true;
			}
		}
	}

	private void IsPlayBackGame(HttpListenerContext context)
	{
		while (!m_IsRunPlayBackGameCheckOver)
		{
			if (m_IsRunPlayBackGameOver)
			{
				m_IsRunPlayBackGameCheckOver = true;
				context.Response.Redirect("/oauth/google/finish");
			}
		}
	}

	public void CheckIsGoogleLogin()
	{
		if (m_IsGoogleLogin)
		{
			m_IsRunPlayBackGameOver = true;
		}
	}

	private void IsFinish(HttpListenerContext context)
	{
		m_IsGoogleLogin = false;
		string result = Singleton<DataManager>.instance.GetVariable("Account/Language").GetResult<string>();
		string text = string.Empty;
		string empty = string.Empty;
		string text2 = string.Empty;
		switch (result)
		{
		case "ChineseS":
			text = "登录成功！不信你回游戏看看？";
			empty = "（3秒后强制传送）";
			text2 = "登录成功!";
			break;
		case "ChineseT":
			text = "登錄成功！不信你回游戲看看？";
			empty = "（3秒後強制傳送）";
			text2 = "登錄成功！";
			break;
		case "English":
			text = "Login successful! Go back and check it out in-game?";
			empty = "(Teleporting in 3 seconds...)";
			text2 = "Login successful!";
			break;
		case "Japanese":
			text = "ログイン成功！ゲームに戻してみて？";
			empty = "（3秒後に強制戻る）";
			text2 = "ログイン成功！";
			break;
		case "Korean":
			text = "로그인 성공! 게임으로 들어가서 확인해볼래?";
			empty = "(3초 뒤 강제 전송)";
			text2 = "로그인 성공!";
			break;
		}
		string value = "<html><head><meta charset = \"UTF-8\"><title>" + text2 + "</title></head><body><h1 align=\"center\">" + text + "</h1 ></body></html>";
		StreamWriter streamWriter = new StreamWriter(context.Response.OutputStream);
		streamWriter.WriteLine(value);
		streamWriter.Flush();
		streamWriter.Close();
		OnWebEnd();
	}

	private void IsFail(HttpListenerContext context)
	{
		m_IsGoogleLogin = false;
		string result = Singleton<DataManager>.instance.GetVariable("Account/Language").GetResult<string>();
		string text = string.Empty;
		string text2 = string.Empty;
		switch (result)
		{
		case "ChineseS":
			text = "登录失败...要不要检查一下网络试试？";
			text2 = "登录失败...";
			break;
		case "ChineseT":
			text = "登錄失敗...要不要檢查一下網絡試試？";
			text2 = "登錄失敗...";
			break;
		case "English":
			text = "Login failed... Eh... Try checking your network connection?";
			text2 = "Login failed...";
			break;
		case "Japanese":
			text = "ログイン失敗…通信状況確認してみたら？";
			text2 = "ログイン失敗…";
			break;
		case "Korean":
			text = "로그인 실패...네트워크 한번만 확인해볼래?";
			text2 = "로그인 실패...";
			break;
		}
		string value = "<html><head><meta charset = \"UTF-8\"><title>" + text2 + "</title></head><body><h1 align=\"center\">" + text + "</h1 ></body></html>";
		StreamWriter streamWriter = new StreamWriter(context.Response.OutputStream);
		streamWriter.WriteLine(value);
		streamWriter.Flush();
		streamWriter.Close();
		OnWebEnd();
	}

	public string Md5Sum(string strToEncrypt)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(strToEncrypt);
		MD5 mD = MD5.Create();
		byte[] array = mD.ComputeHash(bytes);
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	private string GetRandomString()
	{
		byte[] array = new byte[16];
		System.Random random = new System.Random((int)(DateTime.Now.Ticks % 1000000));
		for (int i = 0; i < 16; i++)
		{
			int num;
			do
			{
				num = random.Next(48, 122);
				array[i] = Convert.ToByte(num);
			}
			while ((num >= 58 && num <= 64) || (num >= 91 && num <= 96));
		}
		return Encoding.ASCII.GetString(array);
	}
}
