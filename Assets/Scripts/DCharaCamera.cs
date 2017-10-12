using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCharaCamera : DBehaviour {
    public float distance;
    public float moveDamping;
    public GameObject character;

    Camera _camera;
    Vector3 faceTo;
    Vector3 _target;

    void _UpdateCamera(ViewDirection dir){
        faceTo = DGame.Instance.ViewVector;
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
