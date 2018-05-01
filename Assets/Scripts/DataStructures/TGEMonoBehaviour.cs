using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TGEMonoBehaviour : MonoBehaviour {
    public UnityAction OnAwake;
    public UnityAction OnStart;

    public GameInfo GameInfo { get; protected set; }

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

    /// <summary>
    /// Make the attached <see cref="GameObject"/> active
    /// </summary>
    public virtual void Activate()
    {
        this.gameObject.SetActive(true);      
    }

    /// <summary>
    /// Make the attached <see cref="GameObject"/> inactive
    /// </summary>
    public virtual void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Make the attached <see cref="GameObject"/> activatestate toggled
    /// </summary>
    public virtual void ToggleActivate()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}