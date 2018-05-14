using Assets.Data.Command;
using System.Collections;
using System.Collections.Generic;
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


    public void Initialize(bool isMainSequenceBar, RectTransform mainPanel, GameInfo gameInfo)
    {
        _isMainSequenceBar = isMainSequenceBar;
        _mainPanel = mainPanel;
        _gameInfo = gameInfo;
        _localPlayer = gameInfo.LocalPlayer.Player;

        if (isMainSequenceBar)
        {
            EventManager.OnSequenceChanged += OnSequenceChanged;
            EventManager.OnElementDroppedToMainSequenceBar += AddDroppedElementToMainSequence;
        }
        else
        {
            EventManager.OnSecondarySequenceChanged += OnSequenceChanged;
        }

        InitializeCommandsList(isMainSequenceBar);
        
    }

    private void InitializeCommandsList(bool isMainCommandsList)
    {
        GameObject commandsListPanel = new GameObject("Commands list");
        GameObject sequenceBar = gameObject;
        commandsListPanel.transform.SetParent(sequenceBar.transform, false);

        var commandsListContentSizeFitter = commandsListPanel.AddComponent<ContentSizeFitter>();
        var commandsListLayoutElement = commandsListPanel.AddComponent<LayoutElement>();
        var commandsListFlowLayoutGroup = commandsListPanel.AddComponent<FlowLayoutGroup>();

        commandsListLayoutElement.preferredWidth = isMainCommandsList ? 2000 : 1100;
        commandsListLayoutElement.preferredHeight = isMainCommandsList ? 125 : 75;
        commandsListLayoutElement.minHeight = 0;

        commandsListContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        commandsListContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        commandsListFlowLayoutGroup.childAlignment = isMainCommandsList ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft;
        commandsListFlowLayoutGroup.spacing = isMainCommandsList ? new Vector2(5f, 0f) : new Vector2(3f, 0f);
        commandsListFlowLayoutGroup.horizontal = true;
        commandsListFlowLayoutGroup.padding.top = isMainCommandsList ? 0 : 10;


        sequenceBar.GetComponent<ScrollRect>().content = commandsListPanel.GetComponent<RectTransform>();

        AddReorderableListToComponent(sequenceBar, commandsListFlowLayoutGroup, _mainPanel, isMainCommandsList, isMainCommandsList);

        _commandsListPanel = commandsListPanel;

        if (isMainCommandsList)
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

        if (isRearrangeable)
            reorderableList.OnElementAdded.AddListener(RearrangeElementsInSequence);
    }

    public void AddDroppedElementToMainSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        //Fix when dropping from loop to main sequence bar
        //if (arg0.SourceObject.GetComponent<CommandPanelCommand>() == null)
        //    return;

        BaseCommand command = arg0.SourceObject.GetComponent<CommandPanelCommand>().command;
        if (command is LoopCommand)
        {
            command = ScriptableObject.CreateInstance<LoopCommand>();
            command.Icon = _gameInfo.AllCommands.LoopCommand.Icon;
            command.Name = _gameInfo.AllCommands.LoopCommand.Name;
            command.Priority = _gameInfo.AllCommands.LoopCommand.Priority;
        }


        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if (arg0.ToList.GetComponent<SlotScript>() != null)
        {
            SlotScript commandSlotScript = arg0.ToList.GetComponent<SlotScript>();
            List<int> indices = new List<int>();
            indices.AddRange(commandSlotScript.indices);
            indices.Add(arg0.ToIndex);

            _localPlayer.Sequence.Add(command, indices);
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

    private void RearrangeElementsInSequence(ReorderableList.ReorderableListEventStruct arg0)
    {
        //This function is called by reorderable list multiple times, 
        //if its trying to alter the same index and the same list, ignore.

        print($"From index: {arg0.FromIndex}, To index: {arg0.ToIndex}, From list: {arg0.FromList}, To list: {arg0.ToList}");

        if (arg0.ToIndex == arg0.FromIndex && arg0.ToList == arg0.FromList)
        {
            return;
        }
        List<int> toIndices = new List<int>();
        List<int> fromIndices = new List<int>();
        
        //If the list that we're dropping to has a slotscript, its not the sequence bar.
        //Therefore we have to get the list of indices to determine where to add the command to the player sequence.
        if (arg0.ToList.GetComponent<SlotScript>() != null)
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
        if (arg0.FromList.GetComponent<SlotScript>() != null)
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
        for (int i = 0; i < _commandsListPanel.transform.childCount; i++)
        {
            if (_commandsListPanel.transform.GetChild(i).GetComponent<Image>() != null)
            {
                Destroy(_commandsListPanel.transform.GetChild(i).gameObject);
            }
        }
    }

    private void UpdateSequenceBar(List<BaseCommand> commands, Transform parent, bool isMainSequenceBar)
    {
        for (int i = 0; i < commands.Count; i++)
        {
            var amountOfLoops = 0;
            if (commands[i] is LoopCommand)
            {
                amountOfLoops = ((LoopCommand)commands[i]).LoopCount;
            }

            GameObject slot = CreateSequenceBarSlot(isMainSequenceBar, i, commands[i].Icon, commands[i] is LoopCommand, amountOfLoops);
            slot.transform.SetParent(parent, false);

            //Edit the SlotScript so that the right index is added to the slot
            //Check if im in a deeper level than the sequence bar
            var slotSlotScript = slot.GetComponent<SlotScript>();

            if (slot.transform.parent != null && slot.transform.parent.parent.GetComponent<SlotScript>() != null)
            {
                slotSlotScript.indices.AddRange(slot.transform.parent.parent.GetComponent<SlotScript>().indices);
                slotSlotScript.indices.Add(i);
            }
            //Im directly in the sequence bar, add the index
            else
            {
                slotSlotScript.indices.Add(i);
            }

            if (commands[i] is LoopCommand && ((LoopCommand)commands[i]).Sequence != null)
            {
                //Update sequence bar with the new slots inside of the loop
                UpdateSequenceBar(((LoopCommand)commands[i]).Sequence.Commands, slot.transform.GetChild(1), true);

                //Set the loop and its contents widths to fit the children
                SetWidthOfChildren(slot.transform.GetChild(1).gameObject);
                slot.GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth;
                slot.transform.GetChild(0).GetChild(0).GetComponent<LayoutElement>().preferredWidth =
                    slot.transform.GetChild(1).GetComponent<LayoutElement>().preferredWidth - 30;

            }
        }
    }

    private void AmountOfLoopsEdited(string newAmountOfLoops, List<int> indices)
    {
        _localPlayer.Sequence.LoopEdited(newAmountOfLoops, indices);
    }

    private void SetWidthOfChildren(GameObject item)
    {
        List<GameObject> children = new List<GameObject>();
        float width = 55;

        foreach (Transform child in item.transform)
        {
            width += child.GetComponent<LayoutElement>().preferredWidth + 5;
        }

        if (item.transform.childCount == 0)
        {
            width = 155;
        }

        var itemLayout = item.GetComponent<LayoutElement>();
        itemLayout.preferredWidth = width;
    }

    private GameObject CreateSequenceBarSlot(bool isMainSequenceBar, int index, Sprite image, bool isLoopCommandSlot, int amountOfLoops)
    {
        int size = isMainSequenceBar ? 95 : 55;

        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();

        slotLayoutElement.preferredHeight = size;
        slotLayoutElement.preferredWidth = size;
        slotImage.sprite = image;

        //if Loop command, add reorderable list setup
        if (isLoopCommandSlot)
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
            listInSlotLayout.preferredHeight = 125;
            listInSlotLayout.preferredWidth = 155;

            listInSlot.transform.SetParent(slot.transform, false);

            AddReorderableListToComponent(slot, listInSlotFlow, _mainPanel, true, true);
            var slotReorderableList = slot.GetComponent<ReorderableList>();
            slotReorderableList.isContainerCommandList = true;
            slotReorderableList.indexInParent = index;
            //slotReorderableList.OnElementAdded.AddListener(EventManager.OnElementDroppedToMainSequenceBar);

            listInSlotFlow.childAlignment = TextAnchor.MiddleLeft;
            listInSlotFlow.padding.left = 15;
            listInSlotFlow.spacing = new Vector2(5f, 0);

            slotLayoutElement.preferredHeight = size;
            slotLayoutElement.preferredWidth = 155;

            GameObject loopImageAndInput = new GameObject("loop image & input");
            var loopAndImageFlowLayout = loopImageAndInput.AddComponent<FlowLayoutGroup>();
            var loopAndImageLayoutElement = loopImageAndInput.AddComponent<LayoutElement>();
            var loopAndImageContentSizeFitter = loopImageAndInput.AddComponent<ContentSizeFitter>();

            loopAndImageContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopAndImageContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            loopAndImageLayoutElement.preferredWidth = 155;
            loopAndImageLayoutElement.preferredHeight = 95;

            GameObject loopImage = new GameObject("image");
            loopImage.AddComponent<Image>().sprite = image;
            var loopImageContentSizeFitter = loopImage.AddComponent<ContentSizeFitter>();

            loopImageContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopImageContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var loopImageLayoutElement = loopImage.AddComponent<LayoutElement>();
            loopImageLayoutElement.preferredWidth = 125;
            loopImageLayoutElement.preferredHeight = 95;

            GameObject loopInput = new GameObject("input");
            var loopInputLayoutElement = loopInput.AddComponent<LayoutElement>();
            var loopInputContentSizeFitter = loopInput.AddComponent<ContentSizeFitter>();
            var loopInputImage = loopInput.AddComponent<Image>();

            loopInputContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            loopInputContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            loopInputLayoutElement.preferredHeight = 95;
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
            inputText.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 95);

            loopInputField.textComponent = inputTextText;
            loopInputField.characterLimit = 1;
            loopInputField.text = amountOfLoops.ToString();
            loopInputField.targetGraphic = loopInputImage;
            loopInputField.contentType = InputField.ContentType.IntegerNumber;
            loopInputField.onEndEdit.AddListener((string newAmountOfLoops) => AmountOfLoopsEdited(newAmountOfLoops, slotSlotScript.indices));
            loopInputField.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 95);

            inputText.transform.SetParent(loopInputField.transform, false);
            loopInput.transform.SetParent(loopInputField.transform, false);

        }

        if (isMainSequenceBar)
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
        if (!slotListElement._isDragging)
        {
            _localPlayer.Sequence.RemoveAt(i);
        }
    }

    public void OnSequenceChanged(List<BaseCommand> commands)
    {
        ClearSequenceBar(true);

        UpdateSequenceBar(commands, _commandsListPanel.transform, true);
    }
}
