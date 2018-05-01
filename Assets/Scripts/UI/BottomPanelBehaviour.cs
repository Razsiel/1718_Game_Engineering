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

public class BottomPanelBehaviour : MonoBehaviour
{
    public GameObject MainSequenceBar;
    public GameObject SecondarySequenceBar;
    private GameObject _mainCommandsListPanel;
    private GameObject _secondaryCommandsListPanel;
    private GameInfo _gameInfo;
    private GameObject _mainSequenceBar;
    private GameObject _secondarySequenceBar;
    private List<BaseCommand> _mainBaseCommandsList;

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    EventManager.OnUserInputEnable += ShowBottomPanel;
	    EventManager.OnUserInputDisable += HideBottomPanel;

	}

    void Initialize(GameInfo gameInfo)
    {
        _gameInfo = gameInfo;
        InitializeSequenceBars();
        InitializeReadyButton();
    }

    private void InitializeSequenceBars()
    {

        //InitializeMainSequenceBar();
        InitializeSecondarySequenceBar();

    }

    private void InitializeMainSequenceBar()
    {
        _mainSequenceBar = Instantiate(MainSequenceBar);
        _mainSequenceBar.transform.SetParent(transform.GetChild(2), false);
        _mainSequenceBar.transform.SetSiblingIndex(1);


        InitializeMainCommandsList();
    }

    private void InitializeSecondarySequenceBar()
    {
        _secondarySequenceBar = Instantiate(SecondarySequenceBar);

        _secondarySequenceBar.transform.SetParent(transform.GetChild(0), false);
        _secondarySequenceBar.transform.SetSiblingIndex(1);

    }

    private void InitializeMainCommandsList()
    {
        _mainCommandsListPanel = new GameObject("Main Commands list");
        _mainCommandsListPanel.transform.SetParent(_mainSequenceBar.transform, false);

        var commandsListContentSizeFitter = _mainCommandsListPanel.AddComponent<ContentSizeFitter>();
        var commandsListLayoutElement = _mainCommandsListPanel.AddComponent<LayoutElement>();
        var commandsListFlowLayoutGroup = _mainCommandsListPanel.AddComponent<FlowLayoutGroup>();

        commandsListLayoutElement.preferredWidth = 2000;
        commandsListLayoutElement.preferredHeight = 125;

        commandsListContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        commandsListContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        commandsListFlowLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
        commandsListFlowLayoutGroup.spacing = new Vector2(5f, 0f);
        commandsListFlowLayoutGroup.horizontal = true;

        InitializeSequenceBarSlots(15, true);

        _mainSequenceBar.GetComponent<ScrollRect>().content = _mainCommandsListPanel.GetComponent<RectTransform>();
        var mainSequenceBarReorderableList = _mainSequenceBar.AddComponent<ReorderableList>();
        mainSequenceBarReorderableList.ContentLayout = commandsListFlowLayoutGroup;
        mainSequenceBarReorderableList.DraggableArea = transform.parent.GetComponent<RectTransform>();
        mainSequenceBarReorderableList.IsDraggable = true;

    }

    private void InitializeSecondaryCommandsList()
    {
        _secondaryCommandsListPanel = new GameObject("Secondary Commands list");
        _secondaryCommandsListPanel.transform.SetParent(_secondarySequenceBar.transform, false);
    }

    private void InitializeReadyButton()
    {
        GameObject readyButton = new GameObject("ReadyButton");
        var readyButtonButton = readyButton.AddComponent<Button>();
        var readyButtonSprite = readyButton.AddComponent<Image>();
        var readyButtonLayoutElement = readyButton.AddComponent<LayoutElement>();
        var readyButtonContentSizeFitter = readyButton.AddComponent<ContentSizeFitter>();

        readyButton.transform.SetParent(transform.GetChild(2), false);

        readyButtonLayoutElement.preferredWidth = 125;
        readyButtonLayoutElement.preferredHeight = 125;

        readyButtonContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        readyButtonContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        readyButtonSprite.sprite = Resources.Load("Images/Img_Play_Temp") as Sprite;

        readyButtonButton.onClick.AddListener(() => ReadyButtonClicked());
    }

    private void ReadyButtonClicked()
    {
        print("Ready button clicked");

    }

    public void OnSequenceChanged(List<BaseCommand> commands)
    {
        _mainBaseCommandsList = commands;
        ClearMainSequenceBar();

        UpdateSequenceBar(commands, true);
    }

    private void ClearMainSequenceBar()
    {
        for (int i = 0; i < _mainCommandsListPanel.transform.childCount; i++)
        {
            _mainCommandsListPanel.transform.GetChild(i).GetComponent<Image>().sprite = null;
        }
    }

    private void UpdateSequenceBar(List<BaseCommand> commands, bool isMainSequenceBar)
    {
        Transform parent = isMainSequenceBar ? _mainCommandsListPanel.transform : _secondarySequenceBar.transform;

        for (int i = 0; i < commands.Count; i++)
        {
            Image slotImage = parent.GetChild(i).GetComponent<Image>();
            slotImage.sprite = commands[i].Icon;
        }
    }
    private void InitializeSequenceBarSlots(int amountOfSlots, bool isMainSequenceBar)
    {
        print("filling sequence bar..");

        int size = isMainSequenceBar ? 95 : 55;
        Transform parent = isMainSequenceBar ? _mainCommandsListPanel.transform : _secondarySequenceBar.transform;

        for (int i = 0; i < amountOfSlots; i++)
        {
            GameObject slot = CreateSequenceBarSlot(size, i);
            slot.transform.SetParent(parent, false);
        }
    }

    private GameObject CreateSequenceBarSlot(int size, int index)
    {
        GameObject slot = new GameObject(index.ToString());
        var slotImage = slot.AddComponent<Image>();
        var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
        var slotLayoutElement = slot.AddComponent<LayoutElement>();
        var slotSlotScript = slot.AddComponent<SlotScript>();
        var slotButton = slot.AddComponent<Button>();
        var slotListItem = slot.AddComponent<ReorderableListElement>();

        slotSlotScript.index = index;
        slotButton.onClick.AddListener(() => SlotClicked(slotSlotScript.index));

        slotContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        slotContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        slotLayoutElement.preferredHeight = size;
        slotLayoutElement.preferredWidth = size;

        slotListItem.IsGrabbable = true;
        slotListItem.IsTransferable = true;

        return slot;
    }


    private void SlotClicked(int i)
    {
        print("You clicked slot: " + i);
    }

    void HideBottomPanel()
    {

    }

    void ShowBottomPanel()
    {
    }
}
