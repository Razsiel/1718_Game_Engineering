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

public class BottomPanelBehaviour : MonoBehaviour
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

    public enum ReadyButtonState
    {
        Play, Ready, Stop
    }

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    EventManager.OnUserInputEnable += ShowBottomPanel;
	    EventManager.OnUserInputDisable += HideBottomPanel;

	}

    void Initialize(GameInfo gameInfo)
    {
        _gameInfo = gameInfo;
        EventManager.OnSecondarySequenceChanged += OnSecondarySequenceChanged;
        InitializeSequenceBars();
        InitializeReadyButton();
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

        InitializeCommandsList(true);
    }

    private void InitializeSecondarySequenceBar()
    {
        _secondarySequenceBar = Instantiate(SecondarySequenceBar);

        _secondarySequenceBar.transform.SetParent(transform.GetChild(0), false);
        _secondarySequenceBar.transform.SetSiblingIndex(1);

        InitializeCommandsList(false);
    }

    private void InitializeCommandsList(bool isMainCommandsList)
    {
        GameObject commandsListPanel = new GameObject("Commands list");
        Transform parent = isMainCommandsList ? _mainSequenceBar.transform : _secondarySequenceBar.transform;
        commandsListPanel.transform.SetParent(parent, false);

        var commandsListContentSizeFitter = commandsListPanel.AddComponent<ContentSizeFitter>();
        var commandsListLayoutElement = commandsListPanel.AddComponent<LayoutElement>();
        var commandsListFlowLayoutGroup = commandsListPanel.AddComponent<FlowLayoutGroup>();

        commandsListLayoutElement.preferredWidth = isMainCommandsList ? 2000 : 1100;
        commandsListLayoutElement.preferredHeight = isMainCommandsList ? 125 : 75;

        commandsListContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        commandsListContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        commandsListFlowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        commandsListFlowLayoutGroup.spacing = isMainCommandsList ? new Vector2(5f, 0f) : new Vector2(3f, 0f);
        commandsListFlowLayoutGroup.horizontal = true;

        InitializeSequenceBarSlots(15, isMainCommandsList, commandsListPanel.transform);

        GameObject sequenceBar = isMainCommandsList ? _mainSequenceBar : _secondarySequenceBar;

        sequenceBar.GetComponent<ScrollRect>().content = commandsListPanel.GetComponent<RectTransform>();

        AddReorderableListToComponent(sequenceBar, commandsListFlowLayoutGroup, transform.parent.GetComponent<RectTransform>(), isMainCommandsList, isMainCommandsList);

        if (isMainCommandsList)
            _commandsListPanel = commandsListPanel;
        else
            _secondaryCommandsListPanel = commandsListPanel;
    }

    public void AddReorderableListToComponent(GameObject component, LayoutGroup contentLayoutGroup, RectTransform draggableArea, bool isDraggable, bool isRearrangeable)
    {
        var reorderableList = component.AddComponent<ReorderableList>();
        reorderableList.ContentLayout = contentLayoutGroup;
        reorderableList.DraggableArea = draggableArea;
        reorderableList.IsDraggable = isDraggable;

        if (isRearrangeable)
            reorderableList.OnElementAdded.AddListener(RearrangeElementsInSequence);
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

    private void RearrangeElementsInSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        if (arg0.ToIndex != arg0.FromIndex)
        {
            if (_gameInfo.LocalPlayer.Player.Sequence.isEmpty(arg0.ToIndex))
            {
                arg0.ToIndex = _gameInfo.LocalPlayer.Player.Sequence.Count - 1;
            }

            _gameInfo.LocalPlayer.Player.Sequence.SwapAtIndexes(arg0.ToIndex, arg0.FromIndex);
        }
    }

    internal void AddDroppedElementToMainSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        //Find what type of command was added
        BaseCommand command;
        CommandEnum commandType = arg0.SourceObject.GetComponent<CommandPanelCommand>().CommandType;
        switch (commandType)
        {
            case CommandEnum.InteractCommand:
                command = _gameInfo.AllCommands.InteractCommand;
                break;
            case CommandEnum.WaitCommand:
                command = _gameInfo.AllCommands.WaitCommand;
                break;
            case CommandEnum.MoveCommand:
                command = _gameInfo.AllCommands.MoveCommand;
                break;
            case CommandEnum.TurnLeftCommand:
                command = _gameInfo.AllCommands.TurnLeftCommand;
                break;
            case CommandEnum.TurnRightCommand:
                command = _gameInfo.AllCommands.TurnRightCommand;
                break;
            case CommandEnum.LoopCommand:
                command = _gameInfo.AllCommands.LoopCommand;
                break;
            default:
                command = _gameInfo.AllCommands.WaitCommand;
                break;
        }

        //Find what index it should be set to
        if (_gameInfo.LocalPlayer.Player.Sequence.isEmpty(arg0.ToIndex))
        {
            _gameInfo.LocalPlayer.Player.Sequence.Add(command);
        }
        else
        {
            _gameInfo.LocalPlayer.Player.Sequence.Insert(arg0.ToIndex, command);
        }

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
            _gameInfo.LocalPlayer.Player.IsReady = true;
        }
        else
        {
            _readyButton.GetComponent<Image>().sprite = _readyButtonStop;
            _gameInfo.LocalPlayer.Player.IsReady = false;
        }

        _readyButtonState = newState;
    }

    private void ReadyButtonClicked()
    {

        if (_readyButtonState == ReadyButtonState.Play)
        {
            SetReadyButtonState(!_gameInfo.IsMultiplayer ? ReadyButtonState.Stop : ReadyButtonState.Ready);
            //EventManager.ReadyButtonClicked();
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

    public void OnSequenceChanged(List<BaseCommand> commands)
    {
        print("-------------");
        foreach (var command in commands)
        {
            print(command.Name);
        }
        print("-------------");
        ClearSequenceBar(true);

        UpdateSequenceBar(commands, true);
    }

    private void OnSecondarySequenceChanged(List<BaseCommand> commands)
    {
        ClearSequenceBar(false);

        UpdateSequenceBar(commands, false);
    }

    private void ClearSequenceBar(bool isMainSequenceBar)
    {
        if (isMainSequenceBar)
        {
            for (int i = 0; i < _commandsListPanel.transform.childCount; i++)
            {
                if (_commandsListPanel.transform.GetChild(i).GetComponent<Image>() != null)
                {
                    _commandsListPanel.transform.GetChild(i).GetComponent<Image>().sprite = null;
                }
            }
        }
        else
        {
            for (int i = 0; i < _secondaryCommandsListPanel.transform.childCount; i++)
            {
                if (_secondaryCommandsListPanel.transform.GetChild(i).GetComponent<Image>() != null)
                {
                    _secondaryCommandsListPanel.transform.GetChild(i).GetComponent<Image>().sprite = null;
                }
            }
        }

    }

    private void UpdateSequenceBar(List<BaseCommand> commands, bool isMainSequenceBar)
    {
        Transform parent = isMainSequenceBar ? _commandsListPanel.transform : _secondaryCommandsListPanel.transform;

        for (int i = 0; i < commands.Count; i++)
        {
            if (parent.GetChild(i).GetComponent<Image>() != null)
            {
                Image slotImage = parent.GetChild(i).GetComponent<Image>();
                slotImage.sprite = commands[i].Icon;
                //if (commands[i].Name == _gameInfo.AllCommands.LoopCommand.Name)
                //{
                //    GameObject slotInSlot = CreateSequenceBarSlot(95, 0);
                //    slotInSlot.transform.SetParent(parent.GetChild(i).transform, false);
                //    var slotHorizontalLayoutGroup = slotInSlot.AddComponent<HorizontalLayoutGroup>();

                //    var slotReorderableList = parent.GetChild(i).gameObject.AddComponent<ReorderableList>();
                //    slotReorderableList.ContentLayout = slotHorizontalLayoutGroup;
                //    slotReorderableList.DraggableArea = transform.parent.GetComponent<RectTransform>();
                //    slotReorderableList.IsDraggable = true;
                //    slotReorderableList.OnElementAdded.AddListener(RearrangeElementsInSequence);

                //}
            }
        }
    }
    private void InitializeSequenceBarSlots(int amountOfSlots, bool isMainSequenceBar, Transform parent)
    {
        for (int i = 0; i < amountOfSlots; i++)
        {
            GameObject slot = CreateSequenceBarSlot(isMainSequenceBar, i);
            slot.transform.SetParent(parent, false);
        }
    }

    private GameObject CreateSequenceBarSlot(bool isMainSequenceBar, int index)
    {
        int size = isMainSequenceBar ? 95 : 55;

        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();

        if (isMainSequenceBar)
        {
            var slotButton = slot.AddComponent<Button>();
            var slotListItem = slot.AddComponent<ReorderableListElement>();

            slotButton.onClick.AddListener(() => SlotClicked(slotListItem, slotSlotScript.index));

            slotListItem.IsGrabbable = true;
            slotListItem.IsTransferable = true;
        }

        slotSlotScript.index = index;

        slotContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        slotContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        slotLayoutElement.preferredHeight = size;
        slotLayoutElement.preferredWidth = size;

        return slot;
    }


    private void SlotClicked(ReorderableListElement slotListElement, int i)
    { 
        //Only execute if not being dragged
        if (!slotListElement._isDragging)
        {
            _gameInfo.LocalPlayer.Player.Sequence.RemoveAt(i);
        }
    }

    void HideBottomPanel()
    {

    }

    void ShowBottomPanel()
    {
    }
}
