using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandController : TGEMonoBehaviour{
    
    private CommandLibrary commandLibrary;
    public SequenceBar sequenceBar;
    Player player;
    GameManager _gameManager;

    public override void Start() {
        _gameManager = GameManager.GetInstance();
        Assert.IsNotNull(_gameManager);
        commandLibrary = _gameManager.CommandLibrary;
        Assert.IsNotNull(commandLibrary);
        _gameManager.PlayerInitialized += playerInitialized => {
            this.player = playerInitialized;
        };
    }

    public void OnMoveButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.MoveCommand);
        GameObject moveCommand = Instantiate(sequenceBar.moveCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnTurnLeftButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.TurnLeftCommand);
        GameObject moveCommand = Instantiate(sequenceBar.turnLeftCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.TurnRightCommand);
        GameObject moveCommand = Instantiate(sequenceBar.turnRightCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnWaitButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.WaitCommand);
        GameObject moveCommand = Instantiate(sequenceBar.waitCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnInteractButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.InteractCommand);
        GameObject moveCommand = Instantiate(sequenceBar.interactCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ClearButtonClicked()
    {
        sequenceBar.ClearImages();
        GameManager gameManager = GameManager.GetInstance();

        player.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        Debug.Log(player);
        player.ReadyButtonClicked();
    }
}