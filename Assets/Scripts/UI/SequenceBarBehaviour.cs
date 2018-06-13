using System;
using Assets.Data.Command;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SequenceBarBehaviour : MonoBehaviour {
    private bool _isMainSequenceBar;
    private RectTransform _mainPanel;
    private GameObject _commandsListPanel;
    private GameInfo _gameInfo;
    private Player _localPlayer;
    private GameObject _decentScorePanel;
    private GameObject _highestScorePanel;
    private uint _decentScore;
    private uint _highestScore;
    private int _sequenceInputWidth;
    private float _commandSize;
    private float _loopWidth;
    private ScrollRect _scrollRect;
    private SequenceBarStarPanel _panelScript;
    private float _commandsListLocalPositionX;
    private float _commandsSpacing;

    private Sequence _sequence;

    void OnDestroy() {
        EventManager.OnSequenceChanged -= OnSequenceChanged;
        EventManager.OnElementDroppedToMainSequenceBar -= AddDroppedElementToMainSequence;
        EventManager.OnSecondarySequenceChanged -= OnSequenceChanged;
    }

    public void Initialize(bool isMainSequenceBar, RectTransform mainPanel, GameInfo gameInfo, bool isHost) {
        _isMainSequenceBar = isMainSequenceBar;
        _mainPanel = mainPanel;
        _gameInfo = gameInfo;
        _localPlayer = gameInfo.LocalPlayer.Player;
        _decentScore = _gameInfo.Level.LevelScore.DecentScore;
        _highestScore = _gameInfo.Level.LevelScore.HighestScore;
        _sequenceInputWidth = _isMainSequenceBar ? 2000 : 1100;
        _commandSize = _isMainSequenceBar ? 95 : 55;
        _loopWidth = isMainSequenceBar ? 155 : 105;
        _commandsSpacing = isMainSequenceBar ? 5 : 3;
        _scrollRect = GetComponent<ScrollRect>();

        //The master player has an orange sequence bar, the client has blue
        gameObject.GetComponent<Image>().color = isHost ? new Color32(255, 184, 66, 255) : new Color32(0x44, 0xDE, 0xFF, 0xFF);

        if (_isMainSequenceBar) {
            EventManager.OnSequenceChanged += OnSequenceChanged;
            EventManager.OnElementDroppedToMainSequenceBar += AddDroppedElementToMainSequence;
        }
        else {
            EventManager.OnSecondarySequenceChanged += OnSequenceChanged;
        }

        InitializeCommandsList(_isMainSequenceBar);
        _commandsListLocalPositionX = _commandsListPanel.transform.localPosition.x;
    }

    public void InitializeScoreStars(GameObject starsPanel) {
        _panelScript = starsPanel.GetComponent<SequenceBarStarPanel>();
        _panelScript.Initialize(_highestScore, _decentScore, _commandSize);
        _scrollRect.onValueChanged.AddListener(OnSequenceBarScroll);
    }

    private void OnSequenceBarScroll(Vector2 arg0) {
        float difference = _commandsListPanel.transform.localPosition.x - _commandsListLocalPositionX;
        _panelScript.OnSequenceScroll(difference);
    }

    private void InitializeCommandsList(bool isMainCommandsList) {
        GameObject commandsListPanel = new GameObject("Commands list");
        GameObject sequenceBar = gameObject;
        commandsListPanel.transform.SetParent(sequenceBar.transform, false);

        var commandsListContentSizeFitter = commandsListPanel.AddComponent<ContentSizeFitter>();
        var commandsListLayoutElement = commandsListPanel.AddComponent<LayoutElement>();
        var commandsListFlowLayoutGroup = commandsListPanel.AddComponent<FlowLayoutGroup>();

        commandsListLayoutElement.preferredWidth = _sequenceInputWidth;
        commandsListLayoutElement.preferredHeight = isMainCommandsList ? 125 : 75;
        commandsListLayoutElement.flexibleWidth = 0;
        commandsListLayoutElement.minHeight = 0;

        commandsListContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        commandsListContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        commandsListFlowLayoutGroup.childAlignment = isMainCommandsList ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft;
        commandsListFlowLayoutGroup.spacing = new Vector2(_commandsSpacing, 0f);
        commandsListFlowLayoutGroup.horizontal = true;
        commandsListFlowLayoutGroup.padding.top = isMainCommandsList ? 0 : 10;
        commandsListFlowLayoutGroup.padding.left = isMainCommandsList ? 10 : 0;

        sequenceBar.GetComponent<ScrollRect>().content = commandsListPanel.GetComponent<RectTransform>();

        AddReorderableListToComponent(sequenceBar, commandsListFlowLayoutGroup, _mainPanel);

        _commandsListPanel = commandsListPanel;

        if (isMainCommandsList) {
            _commandsListPanel.transform.localPosition = new Vector3(230, 0, 0);
        }
        else {
            commandsListPanel.transform.localPosition = new Vector3(100, 0, 0);
        }
    }

    public void AddReorderableListToComponent(GameObject component, LayoutGroup contentLayoutGroup, RectTransform draggableArea)
    {
        var reorderableList = component.AddComponent<ReorderableList>();
        reorderableList.ContentLayout = contentLayoutGroup;
        reorderableList.DraggableArea = draggableArea;
        reorderableList.IsDraggable = _isMainSequenceBar;
        reorderableList.IsDropable = _isMainSequenceBar;

        if(_isMainSequenceBar)
            reorderableList.OnElementAdded.AddListener(RearrangeElementsInSequence);
    }

    public void AddDroppedElementToMainSequence(ReorderableList.ReorderableListEventStruct element) {
        BaseCommand command = element.SourceObject.GetComponent<CommandPanelCommand>().command;
        if (command is LoopCommand) {
            command = Instantiate(_gameInfo.AllCommands.LoopCommand) as LoopCommand;
            command = command.Init();
        }

        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if (element.ToList.GetComponent<SlotScript>() != null) {
            SlotScript commandSlotScript = element.ToList.GetComponent<SlotScript>();
            List<int> indices = new List<int>();
            indices.AddRange(commandSlotScript.indices);
            indices.Add(element.ToIndex);

            _localPlayer.Sequence.Add(command, indices);
        } //If the index we want to be dropping to is empty in the sequence bar, drop it there
        else if (_localPlayer.Sequence.isEmpty(element.ToIndex)) {
            _localPlayer.Sequence.Add(command);
        } //If the slot is not empty in the sequence bar, insert the command at the index
        else {
            _localPlayer.Sequence.Insert(element.ToIndex, command);
        }
    }

    private void RearrangeElementsInSequence(ReorderableList.ReorderableListEventStruct element) {
        //This function is called by reorderable list multiple times, 
        //if its trying to alter the same index and the same list, ignore.

        print(
            $"From index: {element.FromIndex}, To index: {element.ToIndex}, From list: {element.FromList}, To list: {element.ToList}");

        if (element.ToIndex == element.FromIndex && element.ToList == element.FromList) {
            return;
        }
        List<int> toIndices = new List<int>();
        List<int> fromIndices = new List<int>();

        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if (element.ToList.GetComponent<SlotScript>() != null) {
            SlotScript commandSlotScript = element.ToList.GetComponent<SlotScript>();

            //indices of the location the element is to be dropped to
            toIndices.AddRange(commandSlotScript.indices);
            toIndices.Add(element.ToIndex);
        } //The list we're dropping to is the sequence bar
        else {
            toIndices.Add(element.ToIndex);
        }

        //If true, we're taking from inside a loop
        if (element.FromList.GetComponent<SlotScript>() != null) {
            SlotScript commandSlotScript = element.FromList.GetComponent<SlotScript>();

            //indices of the location the element is to be dropped to
            fromIndices.AddRange(commandSlotScript.indices);
            fromIndices.Add(element.FromIndex);
        } //The list we're dropping from is the sequence bar
        else {
            fromIndices.Add(element.FromIndex);
        }

        //Swap the commands
        _localPlayer.Sequence.RemoveFromIndicesAndAddToIndices(fromIndices, toIndices, element.ToList != element.FromList);
    }

    private void ClearSequenceBar(bool isMainSequenceBar) {
        for (int i = 0; i < _commandsListPanel.transform.childCount; i++) {
            if (_commandsListPanel.transform.GetChild(i).GetComponent<Image>() != null) {
                Destroy(_commandsListPanel.transform.GetChild(i).gameObject);
            }
        }
    }

    private void UpdateSequenceBar(List<BaseCommand> commands, Transform parent) {
        for (int i = 0; i < commands.Count; i++) {

            GameObject slot = CreateSequenceBarSlot(_isMainSequenceBar, i, commands[i], commands[i] is LoopCommand);
            slot.transform.SetParent(parent, false);

            //Edit the SlotScript so that the right index is added to the slot
            var slotSlotScript = slot.GetComponent<SlotScript>();

            //Check if im in a deeper level than the sequence bar
            if (parent.parent.GetComponent<SlotScript>() != null) {
                slotSlotScript.indices.AddRange(parent.parent.GetComponent<SlotScript>().indices);
                slotSlotScript.indices.Add(i);
            }
            //Im directly in the sequence bar, add the index
            else {
                slotSlotScript.indices.Add(i);
            }

            if (commands[i] is LoopCommand && ((LoopCommand) commands[i]).Sequence != null) {
                //Update sequence bar with the new slots inside of the loop
                UpdateSequenceBar(((LoopCommand) commands[i]).Sequence.Commands, slot.transform.GetChild(1));

                //Set the loop and its contents widths to fit the children
                SetLoopToWidthOfChildren(slot.transform.GetChild(1).gameObject);
                slot.GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth - 30;
            }
        }
        UpdateSequencebarInputWidth(commands, _commandsListPanel.GetComponent<LayoutElement>());
        if (_isMainSequenceBar)
            UpdateStarPositions(commands);
    }

    private void UpdateStarPositions(List<BaseCommand> commands) {
        List<BaseCommand> expandedCommands = commands;
        int loopCount = 0;
        int standardCommandCount = 0;
        float loopWidthInHighestScoreSegment = 0;
        float loopWidthInDecentScoreSegment = 0;

        foreach (var command in expandedCommands) {
            if (command is LoopCommand) {
                float additionalWidth = 0;
                if (((LoopCommand) command).Sequence.Count > 0)
                    additionalWidth = _loopWidth - _commandSize;
                else
                    additionalWidth = _loopWidth + _commandsSpacing;

                if (standardCommandCount < _highestScore) {
                    loopWidthInHighestScoreSegment += additionalWidth;
                }
                else if (standardCommandCount >= _highestScore && standardCommandCount < _decentScore) {
                    loopWidthInDecentScoreSegment += additionalWidth;
                }
            }
            else {
                standardCommandCount++;
            }
        }

        _panelScript.UpdateStarPanelWidths(loopWidthInHighestScoreSegment, loopWidthInDecentScoreSegment);
    }

    private void UpdateSequencebarInputWidth(List<BaseCommand> commands, LayoutElement commandsListLayoutElement) {
        List<BaseCommand> playerCommands = commands; //_localPlayer.Sequence.Expanded(true).ToList();
        float width = 200;
        foreach (var a in playerCommands)
            if (a is LoopCommand)
                width += _loopWidth + _commandsSpacing;
            else
                width += _commandSize + _commandsSpacing;

        if (width > _sequenceInputWidth) {
            commandsListLayoutElement.preferredWidth = width;
            if (_isMainSequenceBar)
                _panelScript.UpdateScrollContentWidth(width);
        }
        else {
            commandsListLayoutElement.preferredWidth = _sequenceInputWidth;
            if (_isMainSequenceBar)
                _panelScript.UpdateScrollContentWidth(_sequenceInputWidth);
        }
    }

    private void SetLoopToWidthOfChildren(GameObject loop) {
        float width = _isMainSequenceBar ? 55 : 45;

        foreach (Transform child in loop.transform) {
            width += child.GetComponent<LayoutElement>().preferredWidth + _commandsSpacing;
        }

        if (loop.transform.childCount == 0) {
            width = _loopWidth;
        }

        var itemLayout = loop.GetComponent<LayoutElement>();
        itemLayout.preferredWidth = width;
    }

    private void AmountOfLoopsEdited(string newAmountOfLoops, List<int> indices) {
        _localPlayer.Sequence.LoopEdited(newAmountOfLoops, indices);
    }


    private GameObject CreateSequenceBarSlot(bool isMainSequenceBar, int index, BaseCommand command, bool isLoopCommandSlot) {
        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();

        slotLayoutElement.preferredHeight = _commandSize;
        slotLayoutElement.preferredWidth = _commandSize;
        slotImage.sprite = command.Icon;

        //if Loop command, add reorderable list setup
        if (isLoopCommandSlot) {
            //Set the slot image to be invisible, it has to have one for it to be droppable but we dont want to see it
            slotImage.color = new Color(0f, 0f, 0f, 0f);
            GameObject listInSlot = new GameObject("list in slot");
            var listInSlotFlow = listInSlot.AddComponent<FlowLayoutGroup>();
            var listInSlotContent = listInSlot.AddComponent<ContentSizeFitter>();
            var listInSlotLayout = listInSlot.AddComponent<LayoutElement>();
            listInSlot.AddComponent<SlotListPanel>();

            listInSlotContent.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            listInSlotContent.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            listInSlotLayout.preferredHeight = isMainSequenceBar ? 125 : 55;
            listInSlotLayout.preferredWidth = _loopWidth;
            listInSlotLayout.minHeight = 0;

            listInSlot.transform.SetParent(slot.transform, false);

            AddReorderableListToComponent(slot, listInSlotFlow, _mainPanel);
            var slotReorderableList = slot.GetComponent<ReorderableList>();
            slotReorderableList.isContainerCommandList = true;
            slotReorderableList.indexInParent = index;

            listInSlotFlow.childAlignment = isMainSequenceBar ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft;
            listInSlotFlow.padding.left = isMainSequenceBar ? 15 : 10;
            listInSlotFlow.spacing = new Vector2(5f, 0);

            slotLayoutElement.preferredHeight = _commandSize;
            slotLayoutElement.preferredWidth = _loopWidth;

            GameObject loopImageAndInput = new GameObject("loop image & input");
            var loopAndImageFlowLayout = loopImageAndInput.AddComponent<FlowLayoutGroup>();
            var loopAndImageLayoutElement = loopImageAndInput.AddComponent<LayoutElement>();
            var loopAndImageContentSizeFitter = loopImageAndInput.AddComponent<ContentSizeFitter>();

            loopAndImageContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopAndImageContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            loopAndImageLayoutElement.minHeight = 0;
            loopAndImageLayoutElement.preferredWidth = _loopWidth;
            loopAndImageLayoutElement.preferredHeight = _commandSize;

            GameObject loopImage = new GameObject("image");
            loopImage.AddComponent<Image>().sprite = command.Icon;
            loopImage.GetComponent<Image>().type = Image.Type.Sliced;
            var loopImageContentSizeFitter = loopImage.AddComponent<ContentSizeFitter>();

            loopImageContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopImageContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var loopImageLayoutElement = loopImage.AddComponent<LayoutElement>();
            loopImageLayoutElement.preferredWidth = isMainSequenceBar ? 125 : 75;
            loopImageLayoutElement.preferredHeight = _commandSize;

            GameObject loopInput = new GameObject("input");
            var loopInputLayoutElement = loopInput.AddComponent<LayoutElement>();
            var loopInputContentSizeFitter = loopInput.AddComponent<ContentSizeFitter>();
            var loopInputImage = loopInput.AddComponent<Image>();

            loopInputContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopInputContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            loopInputLayoutElement.preferredHeight = _commandSize;
            loopInputLayoutElement.preferredWidth = 30;

            var loopInputField = loopInput.AddComponent<InputField>();
            loopImage.transform.SetParent(loopImageAndInput.transform, false);
            loopInput.transform.SetParent(loopImageAndInput.transform, false);
            loopInputField.readOnly = !_isMainSequenceBar;

            loopImageAndInput.transform.SetParent(slot.transform, false);
            loopImageAndInput.transform.SetSiblingIndex(0);

            //Create the text field
            GameObject inputText = new GameObject("text");
            var inputTextText = inputText.AddComponent<Text>();
            inputTextText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            inputTextText.alignment = TextAnchor.MiddleCenter;
            inputTextText.supportRichText = false;
            inputTextText.color = Color.black;
            inputTextText.fontSize = 50;
            inputTextText.horizontalOverflow = HorizontalWrapMode.Overflow;
            inputTextText.verticalOverflow = VerticalWrapMode.Overflow;
            inputText.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, _commandSize);

            loopInputField.textComponent = inputTextText;
            loopInputField.characterLimit = 1;
            loopInputField.text = ((LoopCommand) command).LoopCount.ToString();
            loopInputField.targetGraphic = loopInputImage;
            loopInputField.contentType = InputField.ContentType.IntegerNumber;
            loopInputField.onEndEdit.AddListener((string newAmountOfLoops) =>
                                                     AmountOfLoopsEdited(newAmountOfLoops, slotSlotScript.indices));
            loopInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, _commandSize);

            inputText.transform.SetParent(loopInputField.transform, false);
            loopInput.transform.SetParent(loopInputField.transform, false);
            slot.AddComponent<ReorderableListElement>().IsGrabbable = false;
        }

        if (isMainSequenceBar) {
            var slotButton = slot.AddComponent<Button>();
            var slotListItem = slot.AddComponent<ReorderableListElement>();

            slotButton.onClick.AddListener(() => SlotClicked(slotListItem, slotSlotScript.indices));

            slotListItem.IsGrabbable = true;
            slotListItem.IsTransferable = true;
        }

        slotContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        slotContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        return slot;
    }

    private void SlotClicked(ReorderableListElement slotListElement, List<int> i) {
        //Only execute if not being dragged
        if (!slotListElement._isDragging) {
            _localPlayer.Sequence.RemoveAt(i);
        }
    }

    public void OnSequenceChanged(List<BaseCommand> commands) {
        ClearSequenceBar(_isMainSequenceBar);

        UpdateSequenceBar(commands, _commandsListPanel.transform);
    }
}