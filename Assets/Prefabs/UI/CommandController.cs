using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {

    public List<BaseCommand> commands = new List<BaseCommand>();
    private CommandLibrary commandLibrary;
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
        playerA.AddCommand(commandLibrary.moveCommand);
    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        playerA.AddCommand(commandLibrary.turnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        playerA.AddCommand(commandLibrary.turnRightCommand);
    }

    public void WaitCommand()
    {
        playerA.AddCommand(commandLibrary.waitCommand);
    }

    public void InteractCommand()
    {
        Player playerA = gameManager.playerA;
        playerA.AddCommand(commandLibrary.interactCommand);
    }

    public void ClearButtonClicked()
    {
        GameManager gameManager = GameManager.GetInstance();
        Player playerA = gameManager.playerA;
        playerA.ClearCommands();
    }

    public void ReadyButtonClicked()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
