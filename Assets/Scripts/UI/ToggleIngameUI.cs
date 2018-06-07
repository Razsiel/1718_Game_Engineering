using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIngameUI : MonoBehaviour
{

    [SerializeField] private Image _commandPanelBlocker;
    [SerializeField] private Image _sequenceBarBlocker;
    [SerializeField] private Image _playButtonBlocker;

    private void Awake()
    {
        EventManager.OnUserInputEnable += EnableInput;
        EventManager.OnUserInputDisable += BlockInput;
    }

    private void OnDestroy()
    {
        EventManager.OnUserInputEnable -= EnableInput;
        EventManager.OnUserInputDisable -= BlockInput;
    }

    private void BlockInput()
    {
        _commandPanelBlocker.raycastTarget = true;
        _sequenceBarBlocker.raycastTarget = true;
    }

    private void EnableInput()
    {
        _commandPanelBlocker.raycastTarget = false;
        _sequenceBarBlocker.raycastTarget = false;
    }
}
