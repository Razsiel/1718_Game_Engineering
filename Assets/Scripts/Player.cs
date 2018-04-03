using Assets.ScriptableObjects.PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.ScriptableObjects;
using Assets.ScriptableObjects.Player;
using UnityEngine;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Grid.DataStructure;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private GameManager _gameManager;
    private List<BaseCommand> _sequence;

    public int PlayerNumber;
    public PlayerData Data;

    public UnityAction<List<BaseCommand>> SequenceChanged;
    public UnityAction OnPlayerReady;

    public CardinalDirection ViewDirection = CardinalDirection.North;

    // Use this for initialization
    void Start()
    {
        _gameManager = GameManager.GetInstance();
        _sequence = new List<BaseCommand>
        { 
            ScriptableObject.CreateInstance<MoveCommand>(),
            ScriptableObject.CreateInstance<TurnCommand>(),
            ScriptableObject.CreateInstance<MoveCommand>(),
            ScriptableObject.CreateInstance<TurnCommand>(),
            ScriptableObject.CreateInstance<MoveCommand>(),
            ScriptableObject.CreateInstance<TurnCommand>(),
            ScriptableObject.CreateInstance<MoveCommand>(),
            ScriptableObject.CreateInstance<TurnCommand>(),
            ScriptableObject.CreateInstance<MoveCommand>(),
            ScriptableObject.CreateInstance<TurnCommand>(),
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
        OnPlayerReady.Invoke();
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
        SequenceChanged.Invoke(_sequence);
        
    }

    public void ClearCommands()
    {
        _sequence.Clear();
        SequenceChanged.Invoke(_sequence);
    }

    [PunRPC]
    public void UpdateCommands(List<BaseCommand> commands)
    {
        //Here we can update the sequence of the other player with the received commands
        //Something like: View.UpdateOtherPlayerSequence(commands);
        print("WE got a RPC with this data: " + commands);
    }
}