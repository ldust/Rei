using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBehaviour : MonoBehaviour {
    public HashSet<DEventBase> _events = new HashSet<DEventBase>();

    public void OnListenEvent(DEventBase evt) {
        _events.Add(evt);
    }

    protected virtual void OnDestroy(){
        foreach (var evt in _events) {
            evt.Remove(this);
        }
        _events.Clear();
    }
}
