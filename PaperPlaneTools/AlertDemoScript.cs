using UnityEngine;

namespace PaperPlaneTools
{
	public class AlertDemoScript : MonoBehaviour
	{
		public GameObject alertNativeWindow;

		public void OnButtonShowNativeWindow()
		{
			new Alert("Hello", "Hello, world").SetPositiveButton("OK", delegate
			{
				Debug.Log("Ok handler");
			}).Show();
		}

		public void OnButtonShowUIWindow()
		{
			if (alertNativeWindow == null)
			{
				Debug.Log("Alert Native Window property is the inspector");
			}
			else
			{
				new Alert("Hello", "Hello, world").SetPositiveButton("OK").SetNeutralButton("Cancel").SetAdapter(alertNativeWindow.GetComponent<IAlertPlatformAdapter>())
					.Show();
			}
		}

		public void OnButtonQueueTest()
		{
			new Alert("Hello", "#1 in queue").SetPositiveButton("OK").Show();
			new Alert("Hello", "#2 in queue").SetPositiveButton("OK").Show();
		}
	}
}
