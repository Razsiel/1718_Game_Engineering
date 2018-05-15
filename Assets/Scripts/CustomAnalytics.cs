using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Analytics;

public class CustomAnalytics : MonoBehaviour
{
    private Stopwatch _stopwatch;
    private int _sequenceRanAmount;
    private TimeSpan _monologueTimeSpan;
    private GameInfo _gameInfo;

    public void Awake()
    {
        _sequenceRanAmount = 0;

        EventManager.OnLevelLoaded += OnLevelStart;
        EventManager.OnReadyButtonClicked += IncrementSequenceRanAmount;
        EventManager.OnAllLevelGoalsReached += SendDataOnLevelComplete;
        EventManager.OnMonologueEnded += LogMonologueTimespan;
    }

    private void OnLevelStart(GameInfo gameInfo)
    {
        _gameInfo = gameInfo;
        _stopwatch = Stopwatch.StartNew();

        AnalyticsEvent.Custom("LevelStarted", new Dictionary<string, object>() {{"LevelName", gameInfo.Level.Name}});
    }

    private void IncrementSequenceRanAmount()
    {
        _sequenceRanAmount++;
    }

    private void LogMonologueTimespan()
    {
        _monologueTimeSpan = _stopwatch.Elapsed;
    }

    private void SendDataOnLevelComplete()
    {
        print("1");
        _stopwatch.Stop();
        TimeSpan totalPlayTime = _stopwatch.Elapsed;

        // Get Sequence, make serializable, add to data
        // Get Score (stars), add to data

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("PlayTime", totalPlayTime);
        data.Add("SequenceRanAmount", _sequenceRanAmount);
        data.Add("MonologueTimespan", _monologueTimeSpan);
        data.Add("LevelName", _gameInfo.Level.Name);

        AnalyticsEvent.Custom("LevelCompleted", data);
    }

}
