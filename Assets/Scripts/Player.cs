using Assets.ScriptableObjects.PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Grid.DataStructure;

public class Player : MonoBehaviour
{
    public PlayerMovementData Data;
    private GameManager _gameManager;
    public CardinalDirection ViewDirection = CardinalDirection.North;

    private List<BaseCommand> _sequence;

    // Use this for initialization
    void Start()
    {
        _gameManager = GameManager.GetInstance();
        _sequence = new List<BaseCommand>
        {
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand"),
            (TurnCommand) ScriptableObject.CreateInstance("TurnCommand"),
            (MoveCommand) ScriptableObject.CreateInstance("MoveCommand")
        };

        StartCoroutine(WaitForInput());
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Press Spacebar to run sequence
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetAxis("Jump") != 0);

            yield return StartCoroutine(ExecuteCommands());

            yield return new WaitForSeconds(2);
        }
    }

    public void ReadyButtonClicked()
    {
        StartCoroutine(ExecuteCommands());
    }

    IEnumerator ExecuteCommands()
    {
        foreach (BaseCommand command in _sequence)
        {
            StartCoroutine(command.Execute(this));
            yield return new WaitForSeconds(1);
        }
    }

    public void AddCommand(BaseCommand command)
    {
        _sequence.Add(command);
    }

    public void ClearCommands()
    {
        _sequence.Clear();
    }
}