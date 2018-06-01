using Assets.Data.Command;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SequenceBarBehaviour : MonoBehaviour
{

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

    public void Initialize(bool isMainSequenceBar, RectTransform mainPanel, GameInfo gameInfo, bool isHost)
    {
        _isMainSequenceBar = isMainSequenceBar;
        _mainPanel = mainPanel;
        _gameInfo = gameInfo;
        _localPlayer = gameInfo.LocalPlayer.Player;
        _decentScore = _gameInfo.Level.LevelScore.DecentScore;
        _highestScore = _gameInfo.Level.LevelScore.HighestScore;
        _sequenceInputWidth = _isMainSequenceBar ? 2000 : 1100;
        _commandSize = _isMainSequenceBar ? 95 : 55;
        _loopWidth = isMainSequenceBar ? 155 : 105;
        _scrollRect = GetComponent<ScrollRect>();


        //The master player has an orange sequence bar, the client has blue
        if (isHost)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 184, 66, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(0x44, 0xDE, 0xFF, 0xFF);
        }

        if(_isMainSequenceBar)
        {
            EventManager.OnSequenceChanged += OnSequenceChanged;
            EventManager.OnElementDroppedToMainSequenceBar += AddDroppedElementToMainSequence;
        }
        else
        {
            EventManager.OnSecondarySequenceChanged += OnSequenceChanged;
        }

        InitializeCommandsList(_isMainSequenceBar);
        _commandsListLocalPositionX = _commandsListPanel.transform.localPosition.x;
    }

    public void InitializeScoreStars(GameObject starsPanel)
    {
        _panelScript = starsPanel.GetComponent<SequenceBarStarPanel>();
        _panelScript.Initialize(_highestScore, _decentScore, _commandSize);
        _scrollRect.onValueChanged.AddListener(OnSequenceBarScroll);
    }

    private void OnSequenceBarScroll(Vector2 arg0)
    {
       float difference = _commandsListPanel.transform.localPosition.x - _commandsListLocalPositionX;
       _panelScript.OnSequenceScroll(difference);
    }

    void OnDestroy()
    {
        EventManager.OnSequenceChanged -= OnSequenceChanged;
        EventManager.OnElementDroppedToMainSequenceBar -= AddDroppedElementToMainSequence;
        EventManager.OnSecondarySequenceChanged -= OnSequenceChanged;
    }

    private void InitializeCommandsList(bool isMainCommandsList)
    {
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
        commandsListFlowLayoutGroup.spacing = isMainCommandsList ? new Vector2(5f, 0f) : new Vector2(3f, 0f);
        commandsListFlowLayoutGroup.horizontal = true;
        commandsListFlowLayoutGroup.padding.top = isMainCommandsList ? 0 : 10;
        commandsListFlowLayoutGroup.padding.left = isMainCommandsList ? 10 : 0;
        


        sequenceBar.GetComponent<ScrollRect>().content = commandsListPanel.GetComponent<RectTransform>();

        AddReorderableListToComponent(sequenceBar, commandsListFlowLayoutGroup, _mainPanel, isMainCommandsList, isMainCommandsList);

        _commandsListPanel = commandsListPanel;

        if(isMainCommandsList)
        {
            _commandsListPanel.transform.localPosition = new Vector3(230, 0, 0);
        }
        else
        {
            commandsListPanel.transform.localPosition = new Vector3(100, 0, 0);
        }

    }

    public void AddReorderableListToComponent(GameObject component, LayoutGroup contentLayoutGroup, RectTransform draggableArea, bool isDraggable, bool isRearrangeable)
    {
        var reorderableList = component.AddComponent<ReorderableList>();
        reorderableList.ContentLayout = contentLayoutGroup;
        reorderableList.DraggableArea = draggableArea;
        reorderableList.IsDraggable = isDraggable;

        if(isRearrangeable)
            reorderableList.OnElementAdded.AddListener(RearrangeElementsInSequence);
    }

    public void AddDroppedElementToMainSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        print("Adding to main sequence bar");
        BaseCommand command = arg0.SourceObject.GetComponent<CommandPanelCommand>().command;
        if(command is LoopCommand)
        {
            command = Instantiate(_gameInfo.AllCommands.LoopCommand) as LoopCommand;
            command = command.Init();
        }


        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if(arg0.ToList.GetComponent<SlotScript>() != null)
        {
            SlotScript commandSlotScript = arg0.ToList.GetComponent<SlotScript>();
            List<int> indices = new List<int>();
            indices.AddRange(commandSlotScript.indices);
            indices.Add(arg0.ToIndex);

            _localPlayer.Sequence.Add(command, indices);
        }//If the index we want to be dropping to is empty in the sequence bar, drop it there
        else if(_localPlayer.Sequence.isEmpty(arg0.ToIndex))
        {
            _localPlayer.Sequence.Add(command);
        }//If the slot is not empty in the sequence bar, insert the command at the index
        else
        {
            _localPlayer.Sequence.Insert(arg0.ToIndex, command);
        }
    }

    private void RearrangeElementsInSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        //This function is called by reorderable list multiple times, 
        //if its trying to alter the same index and the same list, ignore.

        print($"From index: {arg0.FromIndex}, To index: {arg0.ToIndex}, From list: {arg0.FromList}, To list: {arg0.ToList}");

        if(arg0.ToIndex == arg0.FromIndex && arg0.ToList == arg0.FromList)
        {
            return;
        }
        List<int> toIndices = new List<int>();
        List<int> fromIndices = new List<int>();

        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if(arg0.ToList.GetComponent<SlotScript>() != null)
        {
            SlotScript commandSlotScript = arg0.ToList.GetComponent<SlotScript>();

            //indices of the location the element is to be dropped to
            toIndices.AddRange(commandSlotScript.indices);
            toIndices.Add(arg0.ToIndex);
        }//The list we're dropping to is the sequence bar
        else
        {
            toIndices.Add(arg0.ToIndex);
        }

        //If true, we're taking from inside a loop
        if(arg0.FromList.GetComponent<SlotScript>() != null)
        {
            SlotScript commandSlotScript = arg0.FromList.GetComponent<SlotScript>();

            //indices of the location the element is to be dropped to
            fromIndices.AddRange(commandSlotScript.indices);
            fromIndices.Add(arg0.FromIndex);

        }//The list we're dropping from is the sequence bar
        else
        {
            fromIndices.Add(arg0.FromIndex);
        }

        //Swap the commands
        _localPlayer.Sequence.RemoveFromIndicesAndAddToIndices(fromIndices, toIndices, arg0.ToList != arg0.FromList);

    }

    private void ClearSequenceBar(bool isMainSequenceBar)
    {
        for(int i = 0; i < _commandsListPanel.transform.childCount; i++)
        {
            if(_commandsListPanel.transform.GetChild(i).GetComponent<Image>() != null)
            {
                Destroy(_commandsListPanel.transform.GetChild(i).gameObject);
            }
        }
    }

    private void UpdateSequenceBar(List<BaseCommand> commands, Transform parent)
    {
        print($"{nameof(SequenceBarBehaviour)}: commands: {commands.Count} parent: {parent} isMainSequenceBar: {_isMainSequenceBar}");
        for(int i = 0; i < commands.Count; i++)
        {
            print($"{nameof(SequenceBarBehaviour)} in for loop: i {i}");
            var amountOfLoops = 0;
            if(commands[i] is LoopCommand)
            {
                amountOfLoops = ((LoopCommand)commands[i]).LoopCount;
                print($"LoopCount: {amountOfLoops}");
            }

            GameObject slot = CreateSequenceBarSlot(_isMainSequenceBar, i, commands[i].Icon, commands[i] is LoopCommand, amountOfLoops);
            slot.transform.SetParent(parent, false);
            print($"{nameof(SequenceBarBehaviour)}: we determined the slot");

            //Edit the SlotScript so that the right index is added to the slot
            //Check if im in a deeper level than the sequence bar
            var slotSlotScript = slot.GetComponent<SlotScript>();

            if(slot.transform.parent != null && slot.transform.parent.parent.GetComponent<SlotScript>() != null)
            {
                slotSlotScript.indices.AddRange(slot.transform.parent.parent.GetComponent<SlotScript>().indices);
                slotSlotScript.indices.Add(i);
            }
            //Im directly in the sequence bar, add the index
            else
            {
                slotSlotScript.indices.Add(i);
            }

            if(commands[i] is LoopCommand && ((LoopCommand)commands[i]).Sequence != null)
            {
                print($"The command is a LoopCommand");
                //Update sequence bar with the new slots inside of the loop
                UpdateSequenceBar(((LoopCommand)commands[i]).Sequence.Commands, slot.transform.GetChild(1));

                print($"We have done the recursion call");
                //Set the loop and its contents widths to fit the children
                SetLoopToWidthOfChildren(slot.transform.GetChild(1).gameObject);
                slot.GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth - 30;
                print($"End of if");
            }
        }
        UpdateSequencebarInputWidth(_commandsListPanel.GetComponent<LayoutElement>());
        UpdateStarPositions();
        print($"End of Update sequence bar");
    }

    private void UpdateStarPositions()
    {
        List<BaseCommand> expandedCommands = _gameInfo.LocalPlayer.Player.Sequence.Expanded(true).ToList();
        int loopCount = 0;
        int standardCommandCount = 0;
        float loopWidthInHighestScoreSegment = 0;
        float loopWidthInDecentScoreSegment = 0;

        foreach (var command in expandedCommands)
        {
            if (command is LoopCommand)
            {
                float additionalWidth = 0;
                if (((LoopCommand) command).Sequence.Count > 0)
                    additionalWidth = _loopWidth - _commandSize;
                else
                    additionalWidth = _loopWidth;

                if (standardCommandCount < _highestScore)
                {
                    loopWidthInHighestScoreSegment += additionalWidth;
                }else if (standardCommandCount >= _highestScore && standardCommandCount < _decentScore)
                {
                    loopWidthInDecentScoreSegment += additionalWidth;
                }
            }
            else
            {
                standardCommandCount++;
            }
        }

        _panelScript.UpdateStarPanelWidths(loopWidthInHighestScoreSegment, loopWidthInDecentScoreSegment);

    }

    private void UpdateSequencebarInputWidth(LayoutElement commandsListLayoutElement)
    {
        List<BaseCommand> playerCommands = _localPlayer.Sequence.Expanded(true).ToList();
        float width = 200;
        foreach (var a in playerCommands)
            if (a is LoopCommand)
                width += _loopWidth;
            else
                width += 100;

        if (width > _sequenceInputWidth)
            commandsListLayoutElement.preferredWidth = width;
    }

    private void SetLoopToWidthOfChildren(GameObject loop)
    {
        float width = _isMainSequenceBar ? 55 : 45;

        foreach (Transform child in loop.transform)
        {
            width += child.GetComponent<LayoutElement>().preferredWidth + 5;
        }

        if (loop.transform.childCount == 0)
        {
            width = _loopWidth;
        }

        var itemLayout = loop.GetComponent<LayoutElement>();
        itemLayout.preferredWidth = width;
    }

    private void AmountOfLoopsEdited(string newAmountOfLoops, List<int> indices)
    {
        _localPlayer.Sequence.LoopEdited(newAmountOfLoops, indices);
    }


    private GameObject CreateSequenceBarSlot(bool isMainSequenceBar, int index, Sprite image, bool isLoopCommandSlot, int amountOfLoops)
    {
        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();

        slotLayoutElement.preferredHeight = _commandSize;
        slotLayoutElement.preferredWidth = _commandSize;
        slotImage.sprite = image;

        //if Loop command, add reorderable list setup
        if(isLoopCommandSlot)
        {
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

            AddReorderableListToComponent(slot, listInSlotFlow, _mainPanel, true, true);
            var slotReorderableList = slot.GetComponent<ReorderableList>();
            slotReorderableList.isContainerCommandList = true;
            slotReorderableList.indexInParent = index;
            //slotReorderableList.OnElementAdded.AddListener(EventManager.OnElementDroppedToMainSequenceBar);

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
            loopImage.AddComponent<Image>().sprite = image;
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
            loopInputField.text = amountOfLoops.ToString();
            loopInputField.targetGraphic = loopInputImage;
            loopInputField.contentType = InputField.ContentType.IntegerNumber;
            loopInputField.onEndEdit.AddListener((string newAmountOfLoops) => AmountOfLoopsEdited(newAmountOfLoops, slotSlotScript.indices));
            loopInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, _commandSize);

            inputText.transform.SetParent(loopInputField.transform, false);
            loopInput.transform.SetParent(loopInputField.transform, false);
            slot.AddComponent<ReorderableListElement>().IsGrabbable = false;

        }

        if(isMainSequenceBar)
        {
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

    private void SlotClicked(ReorderableListElement slotListElement, List<int> i)
    {
        //Only execute if not being dragged
        if(!slotListElement._isDragging)
        {
            _localPlayer.Sequence.RemoveAt(i);
        }
    }

    public void OnSequenceChanged(List<BaseCommand> commands)
    {
        ClearSequenceBar(_isMainSequenceBar);

        UpdateSequenceBar(commands, _commandsListPanel.transform);
    }

}
