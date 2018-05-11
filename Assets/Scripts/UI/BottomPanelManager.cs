using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Debug = System.Diagnostics.Debug;

public class BottomPanelManager : MonoBehaviour
{
    public GameObject MainSequenceBar;
    public GameObject SecondarySequenceBar;
    private GameObject _commandsListPanel;
    private GameObject _secondaryCommandsListPanel;
    private GameInfo _gameInfo;
    private GameObject _mainSequenceBar;
    private GameObject _secondarySequenceBar;
    private GameObject _readyButton;
    private Sprite _readyButtonPlay;
    private Sprite _readyButtonReady;
    private Sprite _readyButtonStop;
    private ReadyButtonState _readyButtonState;
    private RectTransform _mainPanel;
    private Player _localPlayer;

    private bool _secondaryBarIsHidden;

    public enum ReadyButtonState
    {
        Play, Ready, Stop
    }

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    //EventManager.OnUserInputEnable += ShowBottomPanel;
	    //EventManager.OnUserInputDisable += HideBottomPanel;

	}

    void Initialize(GameInfo gameInfo)
    {
        _gameInfo = gameInfo;
        _mainPanel = transform.parent.GetComponent<RectTransform>();
        _localPlayer = gameInfo.LocalPlayer.Player;
        InitializeSequenceBars();
        InitializeReadyButton();
        InitializePlayerIcons();
    }

    private void InitializePlayerIcons()
    {
        InitializeIcons(true);
        InitializeIcons(false);
    }

    private void InitializeIcons(bool isMainPlayerIcon)
    {
        GameObject secondaryPlayerIcon = new GameObject();
        Transform parent = isMainPlayerIcon ? transform.GetChild(2).transform : transform.GetChild(0).transform; 
        secondaryPlayerIcon.transform.SetParent(parent, false);
        secondaryPlayerIcon.transform.SetAsFirstSibling();
        var image = secondaryPlayerIcon.AddComponent<Image>();
        var layoutElement = secondaryPlayerIcon.AddComponent<LayoutElement>();
        var contentSizeFitter = secondaryPlayerIcon.AddComponent<ContentSizeFitter>();

        if (!isMainPlayerIcon)
        {
            secondaryPlayerIcon.AddComponent<Button>().onClick.AddListener(SecondaryPlayerIconClicked);
        }

        layoutElement.preferredWidth = layoutElement.preferredHeight = isMainPlayerIcon ? 125 : 75;
        contentSizeFitter.horizontalFit = contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void InitializeSequenceBars()
    {
        
        InitializeMainSequenceBar();
        InitializeSecondarySequenceBar();

    }

    private void InitializeMainSequenceBar()
    {
        _mainSequenceBar = Instantiate(MainSequenceBar);
        _mainSequenceBar.transform.SetParent(transform.GetChild(2), false);
        _mainSequenceBar.transform.SetSiblingIndex(1);

        _mainSequenceBar.AddComponent<SequenceBarBehaviour>().Initialize(true, _mainPanel, _gameInfo);
    }

    private void InitializeSecondarySequenceBar()
    {
        _secondarySequenceBar = Instantiate(SecondarySequenceBar);

        _secondarySequenceBar.transform.SetParent(transform.GetChild(0), false);
        _secondarySequenceBar.transform.SetSiblingIndex(1);

        _secondarySequenceBar.AddComponent<SequenceBarBehaviour>().Initialize(false, _mainPanel, _gameInfo);
    }

    private void InitializeReadyButton()
    {
        _readyButton = new GameObject("ReadyButton");
        var readyButtonButton = _readyButton.AddComponent<Button>();
        var readyButtonImage = _readyButton.AddComponent<Image>();
        var readyButtonLayoutElement = _readyButton.AddComponent<LayoutElement>();
        var readyButtonContentSizeFitter = _readyButton.AddComponent<ContentSizeFitter>();

        _readyButton.transform.SetParent(transform.GetChild(2), false);

        readyButtonLayoutElement.preferredWidth = 125;
        readyButtonLayoutElement.preferredHeight = 125;

        readyButtonContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        readyButtonContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        _readyButtonPlay = Resources.Load("Images/Img_Play_Temp", typeof(Sprite)) as Sprite;
        _readyButtonReady = Resources.Load("Images/Img_Ready_Temp", typeof(Sprite)) as Sprite;
        _readyButtonStop = Resources.Load("Images/Img_Stop_Temp", typeof(Sprite)) as Sprite;

        readyButtonImage.sprite = _readyButtonPlay;
        _readyButtonState = ReadyButtonState.Play;

        readyButtonButton.onClick.AddListener(() => ReadyButtonClicked());
    }


    private void SetReadyButtonState(ReadyButtonState newState)
    {
        if (newState == ReadyButtonState.Play)
        {
            _readyButton.GetComponent<Image>().sprite = _readyButtonPlay;
        }
        else if (newState == ReadyButtonState.Ready)
        {
            _readyButton.GetComponent<Image>().sprite = _readyButtonReady;
            _localPlayer.IsReady = true;
        }
        else
        {
            _readyButton.GetComponent<Image>().sprite = _readyButtonStop;
            _localPlayer.IsReady = false;
        }

        _readyButtonState = newState;
    }

    private void ReadyButtonClicked()
    {

        if (_readyButtonState == ReadyButtonState.Play)
        {
            SetReadyButtonState(_gameInfo.IsMultiplayer ? ReadyButtonState.Ready : ReadyButtonState.Stop);
            EventManager.ReadyButtonClicked();
            EventManager.PlayerReady(true);

        }else if (_readyButtonState == ReadyButtonState.Ready)
        {
            SetReadyButtonState(ReadyButtonState.Play);
            EventManager.PlayerReady(false);

        }else
        {
            SetReadyButtonState(ReadyButtonState.Play);
            EventManager.StopButtonClicked();
        }

    }

    private void SecondaryPlayerIconClicked()
    {
        if(_secondaryBarIsHidden)
            ShowSecondarySequenceBar();
        else
            HideSecondarySequenceBar();

        _secondaryBarIsHidden = !_secondaryBarIsHidden;
    }

    void HideSecondarySequenceBar()
    {
        _secondarySequenceBar.SetActive(false);
    }

    void ShowSecondarySequenceBar()
    {
        _secondarySequenceBar.SetActive(true);
    }
}
