using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TGEMonoBehaviour : MonoBehaviour {

    public UnityAction OnAwake;
    public UnityAction OnStart;

    /// <summary>
    /// Awake function that implements event callbacks when called.
    /// </summary>
    public virtual void Awake() {
        OnAwake?.Invoke();
    }

    /// <summary>
    /// Start function that implements event callbacks when called.
    /// </summary>
    public virtual void Start() {
        OnStart?.Invoke();
    }

    protected T Spawn<T>(T componentClass, TGEMonoBehaviour parentComponent, UnityAction<T> initializer = null) where T : TGEMonoBehaviour
    {
        Assert.IsNotNull(parentComponent);

        return Spawn(componentClass, parentComponent.gameObject, initializer);
    }

    // Spawn class-restricted GameObject
    protected T Spawn<T>(T componentClass, GameObject parent, UnityAction<T> initializer = null) where T : TGEMonoBehaviour
    {
        Assert.IsNotNull(componentClass);
        Assert.IsNotNull(parent);

        T instance = null;

        return instance;
    }

}
