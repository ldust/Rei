using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEventBase {
    public string name = "noname";
    protected Dictionary<Guid, List<Delegate>> eventList;
    protected DEventBase() {
        eventList = new Dictionary<Guid, List<Delegate>>();
    }

    protected virtual void AddEvent(DBehaviour owner, Delegate del) {
        var key = owner.GetType().GUID;
        if (!eventList.ContainsKey(key)) {
            eventList[key] = new List<Delegate>();
        }
        eventList[key].Add(del);
        owner.OnListenEvent(this);
    }

    public virtual void Remove(DBehaviour owner) {

    }

    public override string ToString() {
        return string.Format("[{0}]{1}", GetType(), name);
    }
}

public class DEvent<T> : DEventBase {
    private event Action<T> _event;
    Func<T> _valueFunc;

    public DEvent<T> Add(DBehaviour owner, Action<T> del) {
        AddEvent(owner, del);
        _event += del;
        return this;
    }

    public DEvent<T> Bind(Func<T> value) {
        _valueFunc = value;
        return this;
    }

    public override void Remove(DBehaviour owner) {
        var key = owner.GetType().GUID;
        if (!eventList.ContainsKey(key)) {
            return;
        }
        var list = eventList[key];
        for (int i = 0; i < list.Count; i++) {
            _event -= (Action<T>)list[i];
        }
    }

    public void Invoke(T v1) {
        if (_event != null) {
            _event(v1);
        }
    }

    public void Invoke() {
        if (_event != null) {
            if (_valueFunc != null) {
                _event(_valueFunc());
            } else {
                Debug.LogError("DEvent: value function is null");
            }
        }
    }
}

public class DEventR<TIn, TOut> : DEventBase {
    private event Func<TIn, TOut> _event;

    public void Add(DBehaviour owner, Func<TIn, TOut> del) {
        AddEvent(owner, del);
        _event += del;
    }

    public override void Remove(DBehaviour owner) {
        var key = owner.GetType().GUID;
        if (!eventList.ContainsKey(key)) {
            return;
        }
        var list = eventList[key];
        for (int i = 0; i < list.Count; i++) {
            _event -= (Func<TIn, TOut>)list[i];
        }
    }

    public TOut Invoke(TIn v) {
        if (_event != null) {
            return _event(v);
        }
        return default(TOut);
    }
}

public class DEvent<T1, T2> : DEventBase {
    private event Action<T1, T2> _event;

    public void Add(DBehaviour owner, Action<T1, T2> del) {
        AddEvent(owner, del);
        _event += del;
    }

    public override void Remove(DBehaviour owner) {
        var key = owner.GetType().GUID;
        if (!eventList.ContainsKey(key)) {
            return;
        }
        var list = eventList[key];
        for (int i = 0; i < list.Count; i++) {
            _event -= (Action<T1, T2>)list[i];
        }
    }

    public void Invoke(T1 v1, T2 v2) {
        if (_event != null) {
            _event(v1, v2);
        }
    }
}