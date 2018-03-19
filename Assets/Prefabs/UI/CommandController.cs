using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {

    public List<BaseCommand> commands = new List<BaseCommand>();
    
    private CommandLibrary commandLibrary;


    public void OnMoveButtonClicked()
    {
        commands.Add(commandLibrary.moveCommand);        
    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        commands.Add(commandLibrary.turnLeftCommand);
    }

    public void OnTurnRightButtonClicked()
    {
        commands.Add(commandLibrary.turnRightCommand);
    }

    public void WaitCommand()
    {
        commands.Add(commandLibrary.waitCommand);
    }

    public void InteractCommand()
    {
        commands.Add(commandLibrary.interactCommand);
    }

    public void ClearButtonClicked()
    {
        commands.Clear();
    }

    public void ReadyButtonClicked()
    {
        
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
