using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandController : MonoBehaviour {

    private CommandLibrary commandLibrary;
    public SequenceBar1 sequenceBar;
    Player player;
    GameManager gameManager;
    

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.GetInstance();
        player = gameManager.Players[0]; // TODO: make dynamic for 2 players
        commandLibrary = gameManager.CommandLibrary;
    }

    public void OnMoveButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.moveCommand;

        player.AddCommand(commandLibrary.moveCommand);

    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnLeftCommand;

        player.AddCommand(commandLibrary.turnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnRightCommand;

        player.AddCommand(commandLibrary.turnRightCommand);
    }

    public void WaitCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.waitCommand;

        player.AddCommand(commandLibrary.waitCommand);
    }

    public void InteractCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.interactCommand;

        player.AddCommand(commandLibrary.interactCommand);
    }

    public void ClearButtonClicked()
    {
        sequenceBar.ClearImages();
        GameManager gameManager = GameManager.GetInstance();

        player.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        player.ReadyButtonClicked();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
