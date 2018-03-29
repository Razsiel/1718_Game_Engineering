using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {

    public List<BaseCommand> commands = new List<BaseCommand>();
    private CommandLibrary commandLibrary;
    Player player;
    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.GetInstance();
        player = gameManager.players[0]; // TODO: make dynamic for 2 players
        commandLibrary = gameManager.commandLibrary;
    }

    public void OnMoveButtonClicked()
    {
        player.AddCommand(commandLibrary.moveCommand);
    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        player.AddCommand(commandLibrary.turnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        player.AddCommand(commandLibrary.turnRightCommand);
    }

    public void WaitCommand()
    {
        player.AddCommand(commandLibrary.waitCommand);
    }

    public void InteractCommand()
    {
        player.AddCommand(commandLibrary.interactCommand);
    }

    public void ClearButtonClicked()
    {
        player.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
