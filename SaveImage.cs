using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveImage : MonoBehaviour
{
	public void OnCilck(Image images)
	{
		StartCoroutine(SaveImages(images.sprite.texture));
	}

	private IEnumerator SaveImages(Texture2D texture)
	{
		string path2 = Application.persistentDataPath;
		path2 = "C:\\Users\\" + Environment.UserName + "\\Pictures";
		if (!Directory.Exists(path2))
		{
			Directory.CreateDirectory(path2);
		}
		string savePath = path2 + "/" + texture.name + ".png";
		File.WriteAllBytes(savePath, texture.EncodeToPNG());
		savePngAndUpdate(savePath);
		yield return new WaitForEndOfFrame();
	}

	private void savePngAndUpdate(string fileName)
	{
		string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "saveImageSucceed");
	}

	private AndroidJavaObject GetAndroidJavaObject()
	{
		return new AndroidJavaObject("com.prpr.plugin.SaveImageActivity");
	}
}
