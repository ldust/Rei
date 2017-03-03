using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class MicControl : MonoBehaviour {
	public enum FreqList {
		_44100HzCD,
		_48000HzDVD
	};

	public bool useDefaultMic = false;
	public bool showDeviceName = false;
	public int inputDevice = 0;
	public bool advanced = false;
	public bool spectrumDropdown = false;
	public int amountSamples = 256;
	public float sensitivity = 1000;
	public Vector2 minMaxSensitivity = new Vector2 (0, 10);
	public int bufferTime = 1;
	public bool mute = true;
	public bool doNotDestroyOnLoad = false;
	public FreqList freq = FreqList._44100HzCD;
	public int maxFreq = 44100;
	public int minFreq = 0;

	public AudioSource audioSource;
	public float[] spectrumData;
	public float[] samples;

	public float loudness;
	public float loudnessNormalized;
	public float rawInput;
	public float pitch;

	bool recording = false;
	bool initialised = false;
	const float Threshold = 0.02f;
	const float RefValue = 0.1f;

	float outputSampleRate;

	string selectedDevice;
	AudioSource sourceContainer;

	// Use this for initialization
	void Start () {
		if (doNotDestroyOnLoad) {
			DontDestroyOnLoad (transform.gameObject);
		}
		spectrumData = new float[amountSamples];
		samples = new float[amountSamples];
		outputSampleRate = AudioSettings.outputSampleRate;
		Debug.Log (AudioSettings.outputSampleRate);
		_InitMic ();
		Debug.Log (AudioSettings.outputSampleRate);

	}

	void _InitMic() {
		if (!audioSource) {
			audioSource = GetComponent <AudioSource> ();
		}
		if (Microphone.devices.Length <= 0) {
			Debug.LogError ("No connected device detected! Connect at least one device.");
			return;
		}
		int i = Microphone.devices.Length;

		if (useDefaultMic) {
			selectedDevice = Microphone.devices [0];
		} else {
			selectedDevice = Microphone.devices [inputDevice];
		}
		if (freq == FreqList._44100HzCD) {
			maxFreq = 44100;
		} else if (freq == FreqList._48000HzDVD) {
			maxFreq = 48000;
		}

		//detect the selected microphone one time to geth the first buffer.
		audioSource.clip = Microphone.Start (selectedDevice, true, bufferTime, maxFreq);
		audioSource.loop = true;


		//don't do anything until the microphone started up
		while (!(Microphone.GetPosition (selectedDevice) > 0)) {}

		_StartMicrophone ();

		recording = true;
		initialised = true;
	}

	void _StartMicrophone(){
		_GetMicCaps ();
		//Starts recording
		audioSource.clip = Microphone.Start(selectedDevice, true, bufferTime, maxFreq);
		// Wait until the recording has started
		while (!(Microphone.GetPosition(selectedDevice) > 0)) {}
		// Play the audio source! 
		audioSource.Play();
	}
	void _StopMicrophone(){
		audioSource.Stop ();
		Microphone.End (selectedDevice);
		initialised = false;
		recording = false;
	}

	void _GetMicCaps () {
		//Gets the frequency of the device
		Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);
		//These 2 lines of code are mainly for windows computers
		if ((0 + maxFreq) == 0) {
			maxFreq = 44100;
		}
	}

	float _GetDb(){
		audioSource.GetOutputData(samples, 0); // fill array with samples

		for( int i = 1; i < samples.Length-1; i++ )
		{
			Debug.DrawLine( new Vector3( i - 1, samples[i] * 10000 + 10, 0 ), new Vector3( i, samples[i + 1] * 10000 + 10, 0 ), Color.red );
		}


		float sum = 0;
		for (int i = 0; i < amountSamples; i++) {
			sum += samples[i] * samples[i]; // sum squared samples
		}
		float RmsValue = Mathf.Sqrt(sum / amountSamples); // rms = square root of average
		float DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
		if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
		return DbValue;
	}

	float _GetPitch(){
		audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

		float[] spectrum = new float[256];

		float maxV = 0;
		var maxN = 0;
		// find max 
		for (int i = 0; i < amountSamples; i++) {
			if (!(spectrumData[i] > maxV) || !(spectrumData[i] > Threshold))
				continue;

			maxV = spectrumData[i];
			maxN = i; // maxN is the index of max
		}
		float freqN = maxN; // pass the index to a float variable
		if (maxN > 0 && maxN < amountSamples - 1)
		{ // interpolate index using neighbours
			var dL = spectrumData[maxN - 1] / spectrumData[maxN];
			var dR = spectrumData[maxN + 1] / spectrumData[maxN];
			freqN += 0.5f * (dR * dR - dL * dL);
		}
		return freqN * (outputSampleRate / 2) / amountSamples; // convert index to frequency
	}

	void Update () {
		if (!Application.isPlaying) {
			_StopMicrophone ();
			return;
		}

		if (!initialised) {
			_InitMic ();
		}

		if (Microphone.IsRecording (selectedDevice) && recording) {
			rawInput = _GetDb ();
			pitch = _GetPitch ();
			loudness = Mathf.Min (Mathf.Max (minMaxSensitivity.x, rawInput * sensitivity), minMaxSensitivity.y);
			loudnessNormalized = (loudness - minMaxSensitivity.x) / (minMaxSensitivity.y - minMaxSensitivity.x);
			audioSource.volume = 1;
		}
	}

}
