using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSingletonBehaviour<T> : DBehaviour where T : DSingletonBehaviour<T>
{
    public static T Instance { get; protected set; }

    public DSingletonBehaviour() {
        Instance = this as T;
    }
}
