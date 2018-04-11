using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandController : TGEMonoBehaviour {
    
    private CommandLibrary commandLibrary;
    public SequenceBar sequenceBar;
    public SequenceBar otherSequencebar;
    Player player;
    GameManager _gameManager;

    public override void Start() {
        _gameManager = GameManager.GetInstance();
        Assert.IsNotNull(_gameManager);
        commandLibrary = _gameManager.CommandLibrary;
        Assert.IsNotNull(commandLibrary);
        _gameManager.PlayersInitialized += /*(Player playerInitialized)*/ () => {
            this.player = _gameManager.Players[0].player;
            print("player shoudl be filled");
            Assert.IsNotNull(player);
        };
    }

    private void CheckPlayerIsFound()
    {
        if(player == null)
            player = GameManager.GetInstance().Players[0].player;
    }

    public void OnMoveButtonClicked()
    {        
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.moveCommand;
        Debug.Log(player + "" + sequenceBar + "" + image);
        player.AddCommand(commandLibrary.Commands.GetValue(CommandEnum.MoveCommand));
    }

    public void OnTurnLeftButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnLeftCommand;
        player.AddCommand(commandLibrary.Commands.GetValue(CommandEnum.TurnLeftCommand));
            
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.turnRightCommand;
        player.AddCommand(commandLibrary.Commands.GetValue(CommandEnum.TurnRightCommand));
    }

    public void WaitCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.waitCommand;
        player.AddCommand(commandLibrary.Commands.GetValue(CommandEnum.WaitCommand));
            
    }

    public void InteractCommand()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        image.sprite = sequenceBar.interactCommand;
        player.AddCommand(commandLibrary.Commands.GetValue(CommandEnum.InteractCommand));
    }

    public void ClearButtonClicked()
    {
        sequenceBar.ClearImages();
       
        player.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        Debug.Log(player);
        player.ReadyButtonClicked();
    }

    //public void UpdateOtherPlayersSequenceBar(List<BaseCommand> commands)
    //{
    //    foreach(BaseCommand c in commands)
    //    {
    //        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
    //        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
    //        image.sprite = c.Icon;
    //    }
    //}
}