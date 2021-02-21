using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PaperPlaneTools
{
	public class AlertUnityUIAdapter : MonoBehaviour, IAlertPlatformAdapter
	{
		[Tooltip("Text component to set alert title. If null title isn't presented.")]
		public Text titleText;

		[Tooltip("Text component to set alert message. If null message isn't presented.")]
		public Text messageText;

		[Tooltip("Button component to set alert positive button text and callback. If null no button is presented.")]
		public Button positiveButton;

		[Tooltip("Button component to set alert neutral button text and callback. If null no button is presented.")]
		public Button neutralButton;

		[Tooltip("Button component to set alert negative button text and callback. If null no button is presented.")]
		public Button negativeButton;

		[Tooltip("Backgroud panel which dismisses the dialog when clicked. If null, dialog doesn't dismiss when click/tap background.")]
		public GameObject dismissPanel;

		private Action onDismiss;

		void IAlertPlatformAdapter.SetOnDismiss(Action action)
		{
			onDismiss = action;
		}

		void IAlertPlatformAdapter.Dismiss()
		{
			base.gameObject.SetActive(false);
			if (onDismiss != null)
			{
				onDismiss();
			}
		}

		void IAlertPlatformAdapter.Show(Alert alert)
		{
			base.transform.SetAsLastSibling();
			titleText.gameObject.SetActive(alert.Title != null);
			if (titleText != null)
			{
				titleText.text = ((alert.Title == null) ? string.Empty : alert.Title);
			}
			messageText.gameObject.SetActive(alert.Message != null);
			if (messageText != null)
			{
				messageText.text = ((alert.Message == null) ? string.Empty : alert.Message);
			}
			SetButton(positiveButton, alert.PositiveButton);
			SetButton(neutralButton, alert.NeutralButton);
			SetButton(negativeButton, alert.NegativeButton);
			if (dismissPanel != null)
			{
				EventTrigger eventTrigger = dismissPanel.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = (dismissPanel.AddComponent(typeof(EventTrigger)) as EventTrigger);
				}
				if (eventTrigger != null)
				{
					List<EventTrigger.Entry> list2 = eventTrigger.triggers = new List<EventTrigger.Entry>();
					list2.RemoveAll((EventTrigger.Entry foo) => true);
					EventTrigger.Entry entry = new EventTrigger.Entry();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener(delegate
					{
						((IAlertPlatformAdapter)this).Dismiss();
					});
					list2.Add(entry);
				}
			}
			base.gameObject.SetActive(true);
		}

		void IAlertPlatformAdapter.HandleEvent(string name, string value)
		{
		}

		private void SetButton(Button uiButton, AlertButton alertButton)
		{
			if (!(uiButton != null))
			{
				return;
			}
			uiButton.gameObject.SetActive(alertButton != null);
			Component[] componentsInChildren = uiButton.GetComponentsInChildren(typeof(Text), true);
			if (componentsInChildren != null && componentsInChildren.Length > 0)
			{
				Text text = componentsInChildren[0] as Text;
				text.text = ((alertButton == null || alertButton.Title == null) ? string.Empty : alertButton.Title);
			}
			uiButton.onClick.RemoveAllListeners();
			uiButton.onClick.AddListener(delegate
			{
				((IAlertPlatformAdapter)this).Dismiss();
			});
			if (alertButton != null && alertButton.Handler != null)
			{
				uiButton.onClick.AddListener(delegate
				{
					alertButton.Handler();
				});
			}
		}
	}
}
