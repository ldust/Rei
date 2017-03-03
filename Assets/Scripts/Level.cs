using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PutNextGround (Vector3 basePos, GameObject ground) {
		float rand = Random.Range (1, 100);
		Vector3 pos;
		if (rand > 70) {
			pos = new Vector3 (basePos.x + Random.Range (1.5f, 2.8f), 0, 0);
		} else {
			pos = new Vector3 (basePos.x + 1f, 0, 0);
		}
		ground.transform.position = pos;
	}
}
