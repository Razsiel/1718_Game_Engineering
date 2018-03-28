using Assets.ScriptableObjects.PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.DataStructures;

public class Player : MonoBehaviour
{
    public PlayerMovementData data;
    private GameManager gameManager;
    public CardinalDirection viewDirection;

    List<BaseCommand> sequence;

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.GetInstance();
        sequence = new List<BaseCommand>();


        sequence.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        sequence.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        sequence.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        sequence.Add((WaitCommand)ScriptableObject.CreateInstance("WaitCommand"));
        sequence.Add((InteractCommand)ScriptableObject.CreateInstance("InteractCommand"));
        sequence.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        sequence.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
        sequence.Add((TurnCommand)ScriptableObject.CreateInstance("TurnCommand"));
        sequence.Add((MoveCommand)ScriptableObject.CreateInstance("MoveCommand"));
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

    IEnumerator ExecuteCommands()
    {
        foreach (BaseCommand command in sequence)
        {
            StartCoroutine(command.Execute(this));
            yield return new WaitForSeconds(1);
        }
    }

    public void AddCommand(BaseCommand command)
    {
        sequence.Add(command);
    }

    public void ClearCommands()
    {
        sequence.Clear();
    }
}
