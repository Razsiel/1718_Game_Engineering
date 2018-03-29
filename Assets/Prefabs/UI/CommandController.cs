using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour
{
    public List<BaseCommand> commands = new List<BaseCommand>();
    private CommandLibrary commandLibrary;
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
        player.AddCommand(commandLibrary.MoveCommand);
    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        player.AddCommand(commandLibrary.TurnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        player.AddCommand(commandLibrary.TurnRightCommand);
    }

    public void WaitCommand()
    {
        player.AddCommand(commandLibrary.WaitCommand);
    }

    public void InteractCommand()
    {
        player.AddCommand(commandLibrary.InteractCommand);
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