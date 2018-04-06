using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandController : TGEMonoBehaviour {
    
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
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.moveCommand;
        player.AddCommand(commandLibrary.MoveCommand);
    }

    public void OnTurnLeftButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnLeftCommand;
        player.AddCommand(commandLibrary.TurnLeftCommand);
            
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnRightCommand;
        player.AddCommand(commandLibrary.TurnRightCommand);
    }

    public void WaitCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.waitCommand;
        player.AddCommand(commandLibrary.WaitCommand);
            
    }

    public void InteractCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.interactCommand;
        player.AddCommand(commandLibrary.InteractCommand);
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