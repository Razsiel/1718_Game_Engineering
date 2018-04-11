using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void Call();

    public static event Call LevelSelected;
    public static event Call LoadingCompleted;
    public static event Call AllLevelGoalsReached;

    public static event Call InitializeUi;
    public static event Call InitializePhoton;

    public static event Call EnableUserInput;
    public static event Call DisableUserInput;
    public static event Call ReadyButtonClicked;
    public static event Call PhotonSynchronized;
    public static event Call MonologueEnded;

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
