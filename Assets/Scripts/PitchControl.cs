using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pitch;

public class PitchControl : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip _micInput;
	public float[] _samples;
	public PitchTracker _pitchTracker;
	public List<int> _detectedPitches;
	public int avgPitch;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent <AudioSource> ();
		string _audioDevice = Microphone.devices [0];
		int minFreq, maxFreq;
		
		Microphone.GetDeviceCaps(_audioDevice, out minFreq, out maxFreq);
		if (minFreq > 0) _micInput = Microphone.Start(_audioDevice, true, 1, minFreq);
		else _micInput = Microphone.Start(_audioDevice, true, 1, 44000);

		// prepare for pitch tracking
		_samples = new float[_micInput.samples * _micInput.channels];
		_pitchTracker = new PitchTracker();
		_pitchTracker.SampleRate = _micInput.samples;
		_pitchTracker.PitchDetected += new PitchTracker.PitchDetectedHandler(PitchDetectedListener);
	}

	private void PitchDetectedListener(PitchTracker sender, PitchTracker.PitchRecord pitchRecord) {
		int pitch = (int)Mathf.Round(pitchRecord.Pitch);
		if (!_detectedPitches.Contains(pitch)) _detectedPitches.Add(pitch);
		avgPitch = (int)Mathf.Round((pitchRecord.Pitch + avgPitch) / 2f);
	}

	// Update is called once per frame
	void Update () {
		_detectedPitches.Clear(); // clear pitches from last update
		_micInput.GetData(_samples, 0);
		_pitchTracker.ProcessBuffer(_samples);
	}
}
