using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCreator : MonoBehaviour {
	[SerializeField] private GameObject groundPrefab;
	public Queue<GameObject> grounds;
	public GameObject tailGround;

	private float winWidth;

	// Use this for initialization
	void Start () {
		winWidth = Camera.main.aspect * Camera.main.orthographicSize;
		grounds = new Queue<GameObject> ();

		_CreatePrepareArea ();

		for (int i = 0; i < winWidth + 3; i++) {
			var ground = Instantiate (groundPrefab) as GameObject;
			ground.transform.position = new Vector3 (i, 0, 0);
			grounds.Enqueue (ground);
			tailGround = ground;
		}
	}

	void _CreatePrepareArea(){
		for (float i = -winWidth - 3; i < 0; i++) {
			var ground = Instantiate (groundPrefab) as GameObject;
			ground.transform.position = new Vector3 (i, 0, 0);
			grounds.Enqueue (ground);
			tailGround = ground;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

}
