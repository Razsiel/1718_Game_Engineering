using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController : MonoBehaviour {

    public List<BaseCommand> commands = new List<BaseCommand>();

    public void OnMoveButtonClicked()
    {
        commands.Add(new MoveCommand());
    }

    public void OnTurnLeftButtonClicked()
    {
        //Turn left or turn right?
        commands.Add(new TurnCommand());
    }

    public void OnTurnRightButtonClicked()
    {
        commands.Add(new TurnCommand());
    }

    public void WaitCommand()
    {
        commands.Add(new WaitCommand());
    }

    public void InteractCommand()
    {
        commands.Add(new InteractCommand());
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
