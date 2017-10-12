using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCharaMove : MonoBehaviour {
    public float speed;
    CharacterController _char;

	void Start () {
        _char = GetComponent<CharacterController>();

	}
	
	void Update () {
        float dh = Input.GetAxis("Horizontal") * speed;
        float dv = Input.GetAxis("Vertical") * speed;


	}
}
