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
    private RectTransform _mainPanel;
    private Player _localPlayer;

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
        _mainPanel = transform.parent.GetComponent<RectTransform>();
        _localPlayer = gameInfo.LocalPlayer.Player;
        EventManager.OnSecondarySequenceChanged += OnSecondarySequenceChanged;
        EventManager.OnSequenceChanged += OnSequenceChanged;
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

        GameObject sequenceBar = isMainCommandsList ? _mainSequenceBar : _secondarySequenceBar;

        sequenceBar.GetComponent<ScrollRect>().content = commandsListPanel.GetComponent<RectTransform>();

        AddReorderableListToComponent(sequenceBar, commandsListFlowLayoutGroup, _mainPanel, isMainCommandsList, isMainCommandsList);

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
        //this will go wrong 
        if (arg0.ToIndex != arg0.FromIndex)
        {
            List<int> toIndexes = new List<int>();
            List<int> fromIndexes = new List<int>();
            //If the list that we're dropping to has a slotscript, its not the sequence bar.
            //Therefore we have to get the list of indexes to determine where to add the command to the player sequence.
            if (arg0.ToList.GetComponent<SlotScript>() != null)
            {
                //SlotScript commandSlotScript = arg0.ToList.GetComponent<SlotScript>();

                ////Indexes of the location the element is to be dropped to
                //toIndexes.AddRange(commandSlotScript.indexes);
                //toIndexes.Add(arg0.ToIndex);
            }//The list we're dropping to is the sequence bar
            else
            {
                toIndexes.Add(arg0.ToIndex);
            }

            //If true, we're taking from inside a loop
            if (arg0.FromList.GetComponent<SlotScript>() != null)
            {
                //SlotScript commandSlotScript = arg0.FromList.GetComponent<SlotScript>();

                ////Indexes of the location the element is to be dropped to
                //fromIndexes.AddRange(commandSlotScript.indexes);
                //fromIndexes.Add(arg0.ToIndex);

            }//The list we're dropping from is the sequence bar
            else
            {
                fromIndexes.Add(arg0.FromIndex);
            }

            //Swap the commands
            _localPlayer.Sequence.SwapAtIndexes(fromIndexes, toIndexes);

        }
    }

    internal void AddDroppedElementToMainSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        BaseCommand command = arg0.SourceObject.GetComponent<CommandPanelCommand>().command;
        if (command is LoopCommand)
        {
            command = ScriptableObject.CreateInstance<LoopCommand>();
            command.Icon = _gameInfo.AllCommands.LoopCommand.Icon;
            command.Name = _gameInfo.AllCommands.LoopCommand.Name;
            command.Priority = _gameInfo.AllCommands.LoopCommand.Priority;
        }


        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indexes to determine where to add the command to the player sequence.
        if (arg0.ToList.GetComponent<SlotScript>() != null)
        {
            SlotScript commandSlotScript = arg0.ToList.GetComponent<SlotScript>();
            List<int> indexes = new List<int>();
            indexes.AddRange(commandSlotScript.indexes);
            indexes.Add(arg0.ToIndex);

            _localPlayer.Sequence.Add(command, indexes);
        }//If the index we want to be dropping to is empty in the sequence bar, drop it there
        else if (_localPlayer.Sequence.isEmpty(arg0.ToIndex))
        {
            _localPlayer.Sequence.Add(command);
        }//If the slot is not empty in the sequence bar, insert the command at the index
        else
        {
            _localPlayer.Sequence.Insert(arg0.ToIndex, command);
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
            SetReadyButtonState(!_gameInfo.IsMultiplayer ? ReadyButtonState.Stop : ReadyButtonState.Ready);
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

    public void OnSequenceChanged(List<BaseCommand> commands)
    {
        print("-------------");
        foreach (var command in commands)
        {
            print(command.Name);
        }
        print("-------------");
        ClearSequenceBar(true);

        UpdateSequenceBar(commands, _commandsListPanel.transform, true);
    }

    private void OnSecondarySequenceChanged(List<BaseCommand> commands)
    {
        ClearSequenceBar(false);

        UpdateSequenceBar(commands, _secondaryCommandsListPanel.transform, false);
    }

    private void ClearSequenceBar(bool isMainSequenceBar)
    {
        if (isMainSequenceBar)
        {
            for (int i = 0; i < _commandsListPanel.transform.childCount; i++)
            {
                if (_commandsListPanel.transform.GetChild(i).GetComponent<Image>() != null)
                {
                    Destroy(_commandsListPanel.transform.GetChild(i).gameObject);
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

    private void UpdateSequenceBar(List<BaseCommand> commands, Transform parent, bool isMainSequenceBar)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            GameObject slot = CreateSequenceBarSlot(isMainSequenceBar, i, commands[i].Icon, commands[i] is LoopCommand);
            slot.transform.SetParent(parent, false);

            //Edit the SlotScript so that the right index is added to the slot
            //Check if im in a deeper level than the sequence bar
            var slotSlotScript = slot.GetComponent<SlotScript>();

            if (slot.transform.parent != null && slot.transform.parent.parent.GetComponent<SlotScript>() != null)
            {
                slotSlotScript.indexes.AddRange(slot.transform.parent.parent.GetComponent<SlotScript>().indexes);
                slotSlotScript.indexes.Add(i);
            }
            //Im directly in the sequence bar, add the index
            else
            {
                slotSlotScript.indexes.Add(i);
            }


            //We're dropping the slot inside of a loop or if else listpanel
            if (parent.GetComponent<SlotListPanel>() != null && parent.childCount > 1)
            {
                //Adjust the size of the listpanel and the parent of the listpanel accordingly
                parent.parent.GetComponent<LayoutElement>().preferredWidth += 100;
                parent.GetComponent<LayoutElement>().preferredWidth += 100;
            }

            if (commands[i] is LoopCommand && ((LoopCommand) commands[i]).Sequence != null)
            {
                //Update sequence bar with the new slots inside of the loop
                UpdateSequenceBar(((LoopCommand) commands[i]).Sequence.Commands, slot.transform.GetChild(0), true);
            }
        }
    }

    private GameObject CreateSequenceBarSlot(bool isMainSequenceBar, int index, Sprite image, bool isLoopCommandSlot)
    {
        int size = isMainSequenceBar ? 95 : 55;

        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();

        slotLayoutElement.preferredHeight = size;
        slotLayoutElement.preferredWidth = size;

        //if Loop command, add reorderable list setup
        if (isLoopCommandSlot)
        {
            GameObject listInSlot = new GameObject("list in slot");
            var listInSlotFlow = listInSlot.AddComponent<FlowLayoutGroup>();
            var listInSlotContent = listInSlot.AddComponent<ContentSizeFitter>();
            var listInSlotLayout = listInSlot.AddComponent<LayoutElement>();
            listInSlot.AddComponent<SlotListPanel>();

            listInSlotContent.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            listInSlotContent.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            listInSlotLayout.preferredHeight = 125;
            listInSlotLayout.preferredWidth = 125;

            listInSlot.transform.SetParent(slot.transform);
            var slotReorderableList = slot.AddComponent<ReorderableList>();
            slotReorderableList.ContentLayout = listInSlotFlow;
            slotReorderableList.DraggableArea = _mainPanel;
            slotReorderableList.isContainerCommandList = true;
            slotReorderableList.indexInParent = index;

            listInSlotFlow.childAlignment = TextAnchor.MiddleLeft;
            listInSlotFlow.padding.left = 15;
            listInSlotFlow.spacing = new Vector2(5f,0);

            slotLayoutElement.preferredHeight = size;
            slotLayoutElement.preferredWidth = 125;
        }

        slotImage.sprite = image;
        if (isMainSequenceBar)
        {
            var slotButton = slot.AddComponent<Button>();
            var slotListItem = slot.AddComponent<ReorderableListElement>();

            slotButton.onClick.AddListener(() => SlotClicked(slotListItem, slotSlotScript.indexes));

            slotListItem.IsGrabbable = true;
            slotListItem.IsTransferable = true;
        }
        
        slotContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        slotContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        return slot;
    }


    private void SlotClicked(ReorderableListElement slotListElement, List<int> i)
    { 
        //Only execute if not being dragged
        if (!slotListElement._isDragging)
        {
            _localPlayer.Sequence.RemoveAt(i);
        }
    }

    void HideBottomPanel()
    {

    }

    void ShowBottomPanel()
    {
    }
}
