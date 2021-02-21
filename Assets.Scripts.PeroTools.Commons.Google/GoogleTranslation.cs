using Assets.Scripts.PeroTools.Managers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons.Google
{
	public static class GoogleTranslation
	{
		public static void Translate(string text, string languageCodeTo, Action<string> onTranslationReady, string languageCodeFrom = "auto")
		{
			WWW translationWWW = GetTranslationWWW(text, languageCodeFrom, languageCodeTo);
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(WaitForTranslation(translationWWW, onTranslationReady, text));
		}

		public static void Translate(string[] texts, string languageCodeTo, Action<string>[] onTranslationReadys, string languageCodeFrom = "auto")
		{
			for (int i = 0; i < texts.Length; i++)
			{
				string text = texts[i];
				Action<string> onTranslationReady = onTranslationReadys[i];
				WWW translationWWW = GetTranslationWWW(text, languageCodeFrom, languageCodeTo);
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(WaitForTranslation(translationWWW, onTranslationReady, text));
			}
		}

		private static IEnumerator WaitForTranslation(WWW www, Action<string> onTranslationReady, string originalText)
		{
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError(www.error);
				onTranslationReady(string.Empty);
			}
			else
			{
				string obj = ParseTranslationResult(www.text, originalText);
				onTranslationReady(obj);
			}
		}

		public static string ForceTranslate(string text, string languageCodeTo, string languageCodeFrom = "auto")
		{
			WWW translationWWW = GetTranslationWWW(text, languageCodeFrom, languageCodeTo);
			while (!translationWWW.isDone)
			{
			}
			if (!string.IsNullOrEmpty(translationWWW.error))
			{
				Debug.LogError("-- " + translationWWW.error);
				foreach (KeyValuePair<string, string> responseHeader in translationWWW.responseHeaders)
				{
					Debug.Log(responseHeader.Value + "=" + responseHeader.Key);
				}
				return string.Empty;
			}
			return ParseTranslationResult(translationWWW.text, text);
		}

		public static WWW GetTranslationWWW(string text, string languageCodeFrom, string languageCodeTo)
		{
			languageCodeFrom = GoogleLanguages.GetGoogleLanguageCode(languageCodeFrom);
			languageCodeTo = GoogleLanguages.GetGoogleLanguageCode(languageCodeTo);
			if (TitleCase(text) == text)
			{
				text = text.ToLower();
			}
			string text2 = "20170729000069210";
			string text3 = "jIS1m57P8iap3NSaNUMJ";
			int millisecond = DateTime.Now.Millisecond;
			string md5WithString = GetMd5WithString(text2 + text + millisecond + text3);
			string url = $"http://api.fanyi.baidu.com/api/trans/vip/translate?q={text}&from={languageCodeFrom}&to={languageCodeTo}&appid={text2}&salt={millisecond}&sign={md5WithString}";
			return new WWW(url);
		}

		public static string ParseTranslationResult(string html, string originalText)
		{
			try
			{
				JObject jObject = JsonUtils.ToObject(html);
				return jObject["trans_result"][0]["dst"].ToString();
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		private static string GetMd5WithString(string input)
		{
			if (input == null)
			{
				return null;
			}
			MD5 mD = MD5.Create();
			byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToLower().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		public static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}
	}
}
