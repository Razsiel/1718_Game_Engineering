using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelBehaviour : MonoBehaviour
{
    public GameObject MainSequenceBar;
    public GameObject SecondarySequenceBar;

    void Awake ()
	{
	    EventManager.OnInitializeUi += Initialize;
	    EventManager.OnUserInputEnable += ShowBottomPanel;
	    EventManager.OnUserInputDisable += HideBottomPanel;
        InitializeSequenceBars();
    }

    private void InitializeSequenceBars()
    {
        MainSequenceBar = Instantiate(MainSequenceBar);
        SecondarySequenceBar = Instantiate(SecondarySequenceBar);

        SecondarySequenceBar.transform.SetParent(transform.GetChild(0));
        SecondarySequenceBar.transform.SetSiblingIndex(1);

        MainSequenceBar.transform.SetParent(transform.GetChild(2));
        MainSequenceBar.transform.SetSiblingIndex(1);

        CreateSequenceBarSlots(15, MainSequenceBar, 95);
        CreateSequenceBarSlots(15, SecondarySequenceBar, 55);

    }

    private void CreateSequenceBarSlots(int amountOfSlots, GameObject sequenceBar, int size)
    {
        for (int i = 0; i < amountOfSlots; i++)
        {
            GameObject slot = new GameObject(i.ToString());
            var slotImage = slot.AddComponent<Image>();
            var slotContentSizeFitter = slot.AddComponent<ContentSizeFitter>();
            var slotLayoutElement = slot.AddComponent<LayoutElement>();

            slotContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            slotContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            slotLayoutElement.preferredHeight = size;
            slotLayoutElement.preferredWidth = size;

            slot.transform.SetParent(sequenceBar.transform, false);
        }

    }

    void Initialize(GameInfo gameInfo)
    {
    }

    void HideBottomPanel()
    {

    }

    void ShowBottomPanel()
    {
    }
}
