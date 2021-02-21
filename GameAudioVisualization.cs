using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

public class GameAudioVisualization : MonoBehaviour
{
	public float power = 110f;

	public float upSpeed = 0.004f;

	public float downSpeed = 1.35f;

	public float baseN = 1.117f;

	public const int LENGTH = 32;

	public static float[] _samples = new float[1024];

	public static float[] _freqBand = new float[32];

	public static float[] _bandBuffer = new float[32];

	private float[] _bufferDecrease = new float[32];

	private float[] _freqBandHighrst = new float[32];

	public static float[] _audioBand = new float[32];

	public static float[] _audioBandBuffer = new float[32];

	public static bool isPlay = false;

	private void Start()
	{
		isPlay = false;
	}

	private void FixedUpdate()
	{
		if (isPlay)
		{
			GetSpectrumAudiosource();
			MakeFrequencyBands();
			BandBuffer();
			CreateAudioBands();
		}
	}

	private void CreateAudioBands()
	{
		for (int i = 0; i < 32; i++)
		{
			if (_freqBand[i] > _freqBandHighrst[i])
			{
				_freqBandHighrst[i] = _freqBand[i];
			}
			_audioBand[i] = _freqBand[i] / _freqBandHighrst[i];
			_audioBandBuffer[i] = _bandBuffer[i] / _freqBandHighrst[i];
		}
	}

	private void GetSpectrumAudiosource()
	{
		Singleton<AudioManager>.instance.bgm.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
	}

	private void BandBuffer()
	{
		for (int i = 0; i < 32; i++)
		{
			if (_freqBand[i] > _bandBuffer[i])
			{
				_bandBuffer[i] = _freqBand[i];
				_bufferDecrease[i] = upSpeed;
			}
			if (_freqBand[i] < _bandBuffer[i])
			{
				_bandBuffer[i] -= _bufferDecrease[i];
				_bufferDecrease[i] *= downSpeed;
			}
		}
	}

	private void MakeFrequencyBands()
	{
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			float num2 = 0f;
			int num3 = (int)Mathf.Pow(baseN, i) * 2;
			if (i == 7)
			{
				num3 += 2;
			}
			for (int j = 0; j < num3; j++)
			{
				num2 += _samples[num] * (float)(num + 1);
				num++;
			}
			num2 /= (float)num;
			_freqBand[i] = num2 * power;
		}
	}
}
