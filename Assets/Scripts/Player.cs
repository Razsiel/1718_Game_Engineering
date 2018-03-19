using Assets.ScriptableObjects.PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementData data;

    List<BaseCommand> commands;

    // Use this for initialization
    void Start()
    {
        commands = new List<BaseCommand>();


        commands.Add(ScriptableObject.CreateInstance("MoveCommand") as BaseCommand);
        commands.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        commands.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        commands.Add((WaitCommand)ScriptableObject.CreateInstance("WaitCommand"));
        commands.Add((InteractCommand)ScriptableObject.CreateInstance("InteractCommand"));
        commands.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        commands.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        commands.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        commands.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        StartCoroutine(ExecuteCommands());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ExecuteCommands()
    {
        foreach (BaseCommand command in commands)
        {
            StartCoroutine(command.Execute(this));
            yield return new WaitForSeconds(1);
        }
    }

    public void AddCommand(BaseCommand command)
    {
        commands.Add(command);
    }

    public void ClearCommands()
    {
        commands.Clear();
    }
}
