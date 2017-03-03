using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMove : MonoBehaviour {
	private float winWidth;
	private GroundCreator creator;
	private Level lvManager;

	// Use this for initialization
	void Start () {
		winWidth = Camera.main.aspect * Camera.main.orthographicSize;
		creator = GetComponent <GroundCreator> ();
		lvManager = GetComponent <Level> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 1; i++) {
			_CheckMove (creator.grounds);
		}
	}

	void _CheckMove (Queue<GameObject> grounds){
		Vector3 cameraPos = Camera.main.transform.position;
		GameObject firstGround = grounds.Peek ();
		Vector3 sub = cameraPos - firstGround.transform.position;
		if (sub.x > winWidth + 3) {
			var item = grounds.Dequeue ();
			grounds.Enqueue (item);
			if (lvManager) {
				lvManager.PutNextGround (creator.tailGround.transform.position, item);
			} else {
				item.transform.position = new Vector3 (creator.tailGround.transform.position.x + 1, 0, 0);
			}
			creator.tailGround = item;
		}
	}

}
