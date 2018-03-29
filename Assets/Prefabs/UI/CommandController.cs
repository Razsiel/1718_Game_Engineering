using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandController : MonoBehaviour {

    private CommandLibrary commandLibrary;
    public SequenceBar1 sequenceBar;
    Player playerA;
    GameManager gameManager;
    

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.GetInstance();
        playerA = gameManager.playerA;
        commandLibrary = gameManager.commandLibrary;
    }

    public void OnMoveButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.moveCommand;

        playerA.AddCommand(commandLibrary.moveCommand);

    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnLeftCommand;

        playerA.AddCommand(commandLibrary.turnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnRightCommand;

        playerA.AddCommand(commandLibrary.turnRightCommand);
    }

    public void WaitCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.waitCommand;

        playerA.AddCommand(commandLibrary.waitCommand);
    }

    public void InteractCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.interactCommand;

        Player playerA = gameManager.playerA;
        playerA.AddCommand(commandLibrary.interactCommand);
    }

    public void ClearButtonClicked()
    {
        sequenceBar.ClearImages();
        GameManager gameManager = GameManager.GetInstance();
        Player playerA = gameManager.playerA;
        playerA.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        playerA.ReadyButtonClicked();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
