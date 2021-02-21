using System.Collections.Generic;

public class NsVibrationValue
{
	public List<AmplitudePoint> lowAmplitude;

	public List<AmplitudePoint> highAmplitude;

	public float totalTime;

	public NsVibrationValue(AmplitudePoint low, AmplitudePoint high, float time)
	{
		lowAmplitude = new List<AmplitudePoint>
		{
			low
		};
		highAmplitude = new List<AmplitudePoint>
		{
			high
		};
		totalTime = time;
	}

	public NsVibrationValue(List<AmplitudePoint> low, List<AmplitudePoint> high, float time)
	{
		lowAmplitude = low;
		highAmplitude = high;
		totalTime = time;
	}

	public bool isEmpty()
	{
		return lowAmplitude.Count == 0 && highAmplitude.Count == 0;
	}

	public override string ToString()
	{
		string text = string.Empty;
		for (int i = 0; i < lowAmplitude.Count; i++)
		{
			text += $"[{lowAmplitude[i]}]";
		}
		string text2 = string.Empty;
		for (int j = 0; j < highAmplitude.Count; j++)
		{
			text2 += $"[{highAmplitude[j]}]";
		}
		return $"Low {text} High {text2} Time [{totalTime:F}]";
	}
}
