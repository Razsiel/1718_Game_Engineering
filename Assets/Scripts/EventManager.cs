using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static UnityAction LevelSelected;
    public static UnityAction LoadingCompleted;
    public static UnityAction AllLevelGoalsReached;
           
    public static UnityAction InitializeUi;
    public static UnityAction InitializePhoton;
           
    public static UnityAction EnableUserInput;
    public static UnityAction DisableUserInput;
    public static UnityAction ReadyButtonClicked;
    public static UnityAction PhotonSynchronized;
    public static UnityAction MonologueEnded;

    public static void OnInitializeUi()
    {
        InitializeUi?.Invoke();
    }

    public static void OnInitializePhoton()
    {
        InitializePhoton?.Invoke();
    }

    public static void OnEnableUserInput()
    {
        EnableUserInput?.Invoke();
    }

    public static void OnDisableUserInput()
    {
        DisableUserInput?.Invoke();
    }

    public static void OnClickReadyButton()
    {
        ReadyButtonClicked?.Invoke();
    }

    private static void OnLevelSelected()
    {
        LevelSelected?.Invoke();
    }

    private static void OnLoadingCompleted()
    {
        LoadingCompleted?.Invoke();
    }

    private static void OnAllLevelGoalsReached()
    {
        AllLevelGoalsReached?.Invoke();
    }

    private static void OnPhotonSynchronized()
    {
        PhotonSynchronized?.Invoke();
    }

    private static void OnMonologueEnded()
    {
        MonologueEnded?.Invoke();
    }
}
