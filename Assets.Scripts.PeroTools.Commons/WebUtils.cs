using Assets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class WebUtils
	{
		public static UnityWebRequest SendToUrl(string url, string method = "GET", Dictionary<string, object> datas = null, Action<DownloadHandler> callback = null, Action<string> faillCallback = null, Dictionary<string, string> headers = null, int failTime = 0, bool autoSend = true)
		{
			string text = string.Empty;
			Coroutine abortCoroutine = null;
			UnityWebRequest webRequest = new UnityWebRequest(url, method)
			{
				downloadHandler = new DownloadHandlerBuffer(),
				timeout = failTime
			};
			if (datas != null)
			{
				text = JsonUtils.Serialize(datas);
				if (method == "GET")
				{
					string @string = Encoding.UTF8.GetString(UnityWebRequest.SerializeSimpleForm(datas.ToDictionary((KeyValuePair<string, object> d) => d.Key, (KeyValuePair<string, object> d) => d.Value.ToString())));
					webRequest.url = $"{webRequest.url}?{@string}";
				}
				else
				{
					webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(text));
				}
			}
			webRequest.SetRequestHeader("Content-Type", "application/json");
			string text2 = $"==============Send to url: {webRequest.url} on method: {webRequest.method}==============, with data: \n{text}";
			if (headers != null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					webRequest.SetRequestHeader(header.Key, header.Value);
				}
				text2 = $"{text2}\n{$"==============With header: \n{JsonUtils.Serialize(headers)}=============="}";
			}
			Debug.Log(text2);
			if (autoSend)
			{
				webRequest.SendWebRequest();
			}
			if (failTime > 0)
			{
				abortCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay((Action)delegate
				{
					if (faillCallback != null)
					{
						faillCallback(webRequest.error);
					}
				}, (float)failTime);
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (abortCoroutine != null)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(abortCoroutine);
				}
				if (string.IsNullOrEmpty(webRequest.error))
				{
					Debug.Log($"==============Succuessfully recieve from url: {webRequest.url} on method: {webRequest.method}==============, with data: \n{Encoding.UTF8.GetString(webRequest.downloadHandler.data)} with response code : {webRequest.responseCode}");
					if (callback != null)
					{
						callback(webRequest.downloadHandler);
					}
				}
				else
				{
					Debug.Log($"==============Error recieve from url: {webRequest.url} on method: {webRequest.method}==============, with data: \n{webRequest.error} with response code : {webRequest.responseCode}");
					if (faillCallback != null)
					{
						faillCallback(webRequest.error);
					}
				}
			}, () => webRequest.isDone);
			return webRequest;
		}
	}
}
