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
        EventManager.OnUserInputEnable += ToggleInputBlock;
        EventManager.OnUserInputDisable += ToggleInputBlock;
    }

    private void OnDestroy()
    {
        EventManager.OnUserInputEnable -= ToggleInputBlock;
        EventManager.OnUserInputDisable -= ToggleInputBlock;
    }

    private void ToggleInputBlock()
    {
        _commandPanelBlocker.raycastTarget = !_commandPanelBlocker.raycastTarget;
        _sequenceBarBlocker.raycastTarget = !_sequenceBarBlocker.raycastTarget;
    }
}
