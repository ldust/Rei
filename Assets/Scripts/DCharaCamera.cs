using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCharaCamera : DBehaviour {
    public float distance;
    public float moveDamping = 2.0f;
    public GameObject character;


    Camera _camera;
    Vector3 faceTo;
    Vector3 _target;

    void _UpdateCamera(ViewDirection dir){
        switch (dir) {
            case ViewDirection.NegativeX:
                faceTo = new Vector3(-1, 0, 0);
                break;
            case ViewDirection.NegativeY:
                faceTo = new Vector3(0, -1, 0);
                break;
            case ViewDirection.NegativeZ:
                faceTo = new Vector3(0, 0, -1);
                break;
            case ViewDirection.PositiveX:
                faceTo = new Vector3(1, 0, 0);
                break;
            case ViewDirection.PositiveY:
                faceTo = new Vector3(0, 1, 0);
                break;
            case ViewDirection.PositiveZ:
                faceTo = new Vector3(0, 0, 1);
                break;
        }
        _UpdateTarget();
    }

    void _UpdateTarget() {
        _target = character.transform.position + faceTo * distance;
    }

    void Start() {
        _camera = GetComponent<Camera>();
        DGlobalEvents.OnViewDirectionChange.Add(this, _UpdateCamera).Invoke();
        _UpdateTarget();
        transform.position = _target;
    }

	void Update () {
        transform.position = Vector3.Lerp(transform.position, _target, moveDamping * Time.deltaTime);
        transform.LookAt(character.transform);
    }
}
