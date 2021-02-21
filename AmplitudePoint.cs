using System;
using UnityEngine.UI;

public class AmplitudePoint
{
	public float time;

	public float vaule;

	public Slider slider;

	public AmplitudePoint(float t, float v)
	{
		time = t;
		vaule = v;
	}

	public void OnSelect(Action onSelect)
	{
		if (!(slider == null))
		{
			Button componentInChildren = slider.handleRect.GetComponentInChildren<Button>();
			if ((bool)componentInChildren)
			{
				componentInChildren.onClick.AddListener(onSelect.Invoke);
			}
		}
	}

	public override string ToString()
	{
		return $"Time {time:F} Vaule {vaule:F}";
	}
}
