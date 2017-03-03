using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public enum Mode
	{
		Classic,
		AutoMove
	}

	public GameObject micControl;
	public Mode mode = Mode.Classic;
	public float speed = 0.4f;

	bool playing = false;
//	MicControl mic;
	PitchControl pitch;
	// Use this for initialization

	void Start () {
//		mic = micControl.GetComponent <MicControl> ();
		pitch = micControl.GetComponent <PitchControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles = new Vector3 (0, 0, 0);
		if (mode == Mode.Classic) {
			float force = pitch.avgPitch;
			if (force > 80) {
				transform.Translate (speed * Time.deltaTime, 0, 0);
			}
		} else {
			float force = pitch.avgPitch;
			if (force > 10 && !playing) {
				playing = true;
			}
			if (playing) {
				transform.Translate (speed * Time.deltaTime, 0, 0);
			}
		}
	}

	void FixedUpdate(){
		if (transform.position.y > 1.5f || transform.position.y < 0) {
			return;
		}
		if (mode == Mode.Classic) {
			var body = GetComponent <Rigidbody2D>();
			float force = pitch.avgPitch;
			if (force > 320) {
				body.AddForce (new Vector2 (force / 40, force / 4));
			}
		} else {
			var body = GetComponent <Rigidbody2D>();
			float force = pitch.avgPitch;
			body.AddForce (new Vector2(0, force / 8));
		}
	}
}
