using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts;
using Assets.Scripts.Lib.Helpers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Sequence = Assets.Scripts.Sequence;

public class BottomPanelManager : MonoBehaviour
{
    public GameObject MainSequenceBar;
    public GameObject SecondarySequenceBar;
    public GameObject SequenceBarStars;

    private GameObject _commandsListPanel;
    private GameObject _secondaryCommandsListPanel;
    private GameInfo _gameInfo;
    private GameObject _mainSequenceBar;
    private GameObject _secondarySequenceBar;
    private GameObject _readyButton;
    private GameObject _readyButtonIcon;
    private GameObject _sequenceBarStars;

    [SerializeField] private Sprite _buttonBackground;
    [SerializeField] private Sprite _readyButtonPlay;
    [SerializeField] private Sprite _readyButtonReady;
    [SerializeField] private Sprite _readyButtonStop;
    [SerializeField] private Sprite _mainPlayerIcon;
    [SerializeField] private Sprite _secondaryPlayerIcon;

    [SerializeField] private Color _playStateColor;
    [SerializeField] private Color _readyStateColor;
    [SerializeField] private Color _stopStateColor;
    
    private ReadyButtonState _readyButtonState;
    private RectTransform _mainPanel;
    private Player _localPlayer;
    private bool _isHost;

    private bool _secondaryBarIsHidden;

    public enum ReadyButtonState
    {
        Play, Ready, Stop
    }

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    EventManager.OnAllPlayersReady += SetReadyButtonStateOnAllPlayersReady;
        EventManager.OnSimulationStop += OnSimulationStop;
	    //EventManager.OnUserInputEnable += ShowBottomPanel;
	    //EventManager.OnUserInputDisable += HideBottomPanel;
	}

    void OnDestroy() {
        EventManager.OnAllPlayersReady -= SetReadyButtonStateOnAllPlayersReady;
        EventManager.OnSimulationStop -= OnSimulationStop;
        EventManager.OnInitializeUi -= Initialize;
    }

    void Initialize(GameInfo gameInfo)
    {
        EventManager.OnInitializeUi -= Initialize;
        _gameInfo = gameInfo;
        _mainPanel = transform.parent.GetComponent<RectTransform>();
        _localPlayer = gameInfo.LocalPlayer.Player;

        if (_gameInfo.LocalPlayer.photonPlayer != null)
        {
            _isHost = _gameInfo.LocalPlayer.photonPlayer.IsMasterClient;
        }

        InitializeSequenceBarStars();
        InitializeSequenceBars();
        InitializeReadyButton();
        InitializePlayerIcons();
    }

    private void InitializePlayerIcons()
    {
        if (!_gameInfo.IsMultiplayer)
        {
            InitializeIcons(true, true);
        }
        else
        {
            InitializeIcons(true, _isHost);
            InitializeIcons(false, !_isHost);
        }

    }

    private void InitializeIcons(bool isMainPlayerIcon, bool isHost)
    {
        GameObject playerIcon = new GameObject("PlayerIcon");
        Transform parent = isMainPlayerIcon ? transform.GetChild(2).transform : transform.GetChild(0).transform;
        playerIcon.transform.SetParent(parent, false);
        playerIcon.transform.SetAsFirstSibling();
        var image = playerIcon.AddComponent<Image>();
        var aspectFitter = playerIcon.AddComponent<AspectRatioFitter>();
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        var layoutElement = playerIcon.AddComponent<LayoutElement>();
        image.sprite = isHost ? _mainPlayerIcon : _secondaryPlayerIcon;

        //var contentSizeFitter = secondaryPlayerIcon.AddComponent<ContentSizeFitter>();

        if (!isMainPlayerIcon)
        {
            playerIcon.AddComponent<Button>().onClick.AddListener(SecondaryPlayerIconClicked);
        }

        layoutElement.preferredWidth = layoutElement.preferredHeight = isMainPlayerIcon ? 125 : 75;
        layoutElement.flexibleWidth = 0;
        //contentSizeFitter.horizontalFit = contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void InitializeSequenceBars()
    {
        
        InitializeMainSequenceBar();
        if (_gameInfo.IsMultiplayer)
            InitializeSecondarySequenceBar();

    }

    private void InitializeMainSequenceBar()
    {
        _mainSequenceBar = Instantiate(MainSequenceBar);
        _mainSequenceBar.transform.SetParent(transform.GetChild(2), false);
        _mainSequenceBar.transform.SetSiblingIndex(1);

        if (!_gameInfo.IsMultiplayer)
        {
            _isHost = true;
        }
        _mainSequenceBar.AddComponent<SequenceBarBehaviour>().Initialize(true, _mainPanel, _gameInfo, _isHost);
        _mainSequenceBar.GetComponent<SequenceBarBehaviour>().InitializeScoreStars(_sequenceBarStars);
    }

    private void InitializeSequenceBarStars()
    {
        _sequenceBarStars = Instantiate(SequenceBarStars);
        _sequenceBarStars.transform.SetParent(transform, false);
        _sequenceBarStars.transform.SetSiblingIndex(1);
    }

    private void InitializeSecondarySequenceBar()
    {
        _secondarySequenceBar = Instantiate(SecondarySequenceBar);

        _secondarySequenceBar.transform.SetParent(transform.GetChild(0), false);
        _secondarySequenceBar.transform.SetSiblingIndex(1);

        _secondarySequenceBar.AddComponent<SequenceBarBehaviour>().Initialize(false, _mainPanel, _gameInfo, !_isHost);
    }

    private void InitializeReadyButton()
    {
        _readyButton = new GameObject("ReadyButton");
        var readyButtonButton = _readyButton.AddComponent<Button>();
        var readyButtonImage = _readyButton.AddComponent<Image>();
        var readyButtonLayoutElement = _readyButton.AddComponent<LayoutElement>();

        _readyButton.transform.SetParent(transform.GetChild(2), false);

        readyButtonLayoutElement.preferredWidth = 200;
        readyButtonLayoutElement.preferredHeight = 125;
        readyButtonLayoutElement.flexibleWidth = 0;

        readyButtonImage.sprite = _buttonBackground;
        _readyButtonIcon = new GameObject("Icon");
        var icon = _readyButtonIcon.AddComponent<Image>();
        icon.preserveAspect = true;
        _readyButtonIcon.transform.SetParent(readyButtonImage.transform, false);
        var iconTransform = (RectTransform) _readyButtonIcon.transform;
        iconTransform.localScale = Vector3.one * 0.5f;
        iconTransform.sizeDelta = Vector2.one;
        iconTransform.anchorMin = Vector2.zero;
        iconTransform.anchorMax = Vector2.one;

        SetReadyButtonState(ReadyButtonState.Play);

        readyButtonButton.onClick.RemoveAllListeners();
        readyButtonButton.onClick.AddListener(ReadyButtonClicked);
    }

    private void SetReadyButtonStateOnAllPlayersReady()
    {
        SetReadyButtonState(ReadyButtonState.Stop);
    }

    private void OnSimulationStop()
    {
        SetReadyButtonState(ReadyButtonState.Play);
    }

    private void SetReadyButtonState(ReadyButtonState newState)
    {
        if (newState == ReadyButtonState.Play)
        {
            _readyButton.GetComponent<Image>().color = _playStateColor;
            _readyButtonIcon.GetComponent<Image>().sprite = _readyButtonPlay;
        }
        else if (newState == ReadyButtonState.Ready)
        {
            _readyButton.GetComponent<Image>().color = _readyStateColor;
            _readyButtonIcon.GetComponent<Image>().sprite = _readyButtonReady;
            _localPlayer.IsReady = true;
        }
        else
        {
            _readyButton.GetComponent<Image>().color = _stopStateColor;
            _readyButtonIcon.GetComponent<Image>().sprite = _readyButtonStop;
            _localPlayer.IsReady = false;
        }

        _readyButtonState = newState;
    }

    private void ReadyButtonClicked() {
        print($"Pressed button: {_readyButtonState}");
        switch (_readyButtonState) {
            case ReadyButtonState.Play:
                SetReadyButtonState(_gameInfo.IsMultiplayer ? ReadyButtonState.Ready : ReadyButtonState.Stop);
                EventManager.ReadyButtonClicked();
                EventManager.PlayerReady(_gameInfo.LocalPlayer.Player, true);
                break;
            case ReadyButtonState.Ready:
                SetReadyButtonState(ReadyButtonState.Play);
                EventManager.PlayerReady(_gameInfo.LocalPlayer.Player, false);
                break;
            case ReadyButtonState.Stop:
            default:
                SetReadyButtonState(ReadyButtonState.Play);
                EventManager.StopButtonClicked();
                break;
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
