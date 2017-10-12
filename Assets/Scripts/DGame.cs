using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewDirection {
    PositiveZ,
    NegativeZ,
    PositiveX,
    NegativeX,
    PositiveY,
    NegativeY,
}

public class DGame : DSingletonBehaviour<DGame> {
    ViewDirection _currentViewDirection;
    public ViewDirection CurrentViewDirection { 
        get { return _currentViewDirection; } 
        set {
            _currentViewDirection = value;
            DGlobalEvents.OnViewDirectionChange.Invoke();
        }
    }

	void Awake () {
        CurrentViewDirection = ViewDirection.NegativeX;
        DGlobalEvents.OnViewDirectionChange.Bind(() => CurrentViewDirection);
    }

    void Update () {
        
	}

    public void Test() {
        if (CurrentViewDirection < ViewDirection.NegativeY) {
            CurrentViewDirection = CurrentViewDirection + 1;
        } else {
            CurrentViewDirection = ViewDirection.PositiveZ;
        }

    }
}
