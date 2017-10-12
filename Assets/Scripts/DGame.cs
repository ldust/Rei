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
            switch (_currentViewDirection) {
                case ViewDirection.NegativeX:
                    ViewVector = new Vector3(-1, 0, 0);
                    break;
                case ViewDirection.NegativeY:
                    ViewVector = new Vector3(0, -1, 0);
                    break;
                case ViewDirection.NegativeZ:
                    ViewVector = new Vector3(0, 0, -1);
                    break;
                case ViewDirection.PositiveX:
                    ViewVector = new Vector3(1, 0, 0);
                    break;
                case ViewDirection.PositiveY:
                    ViewVector = new Vector3(0, 1, 0);
                    break;
                case ViewDirection.PositiveZ:
                    ViewVector = new Vector3(0, 0, 1);
                    break;
            }
            DGlobalEvents.OnViewDirectionChange.Invoke();
        }
    }

    public Vector3 ViewVector { get; private set; }

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
