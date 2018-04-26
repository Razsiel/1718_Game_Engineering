using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TGEMonoBehaviour : MonoBehaviour {
    public UnityAction OnAwake;
    public UnityAction OnStart;

    public GameInfo GameInfo { get; private set; }

    /// <summary>
    /// Awake function that implements event callbacks when called.
    /// </summary>
    public virtual void Awake() {
        EventManager.OnGameStart += gameInfo => { this.GameInfo = gameInfo; };
        OnAwake?.Invoke();
    }

    /// <summary>
    /// Start function that implements event callbacks when called.
    /// </summary>
    public virtual void Start() {
        OnStart?.Invoke();
    }

    protected T Spawn<T>(string objectName, GameObject parent, UnityAction<T> initializer = null)
        where T : MonoBehaviour {
        Assert.IsNotNull(objectName);
        Assert.IsNotNull(parent);

        var spawnedObject = new GameObject(objectName, typeof(T));
        spawnedObject.transform.parent = parent.transform;
        var component = spawnedObject.GetComponent<T>();
        initializer?.Invoke(component);
        return component;
    }
}