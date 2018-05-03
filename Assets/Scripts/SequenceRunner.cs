using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

public class SequenceRunner : MonoBehaviour {
    
    // TODO: Create SequenceCycle object with the 2 Commands of that step + CommandList -> List<SequenceCycle>

    void Awake()
    {
        EventManager.OnSimulate += ExecuteSequences;
    }

    private void ExecuteSequences(LevelData levelData, List<TGEPlayer> Players)
    {

        // OrderedCommandList based on Command.Priority
        List<Tuple<Player, BaseCommand>> CommandList = new List<Tuple<Player, BaseCommand>>();

        // Singleplayer only
        if (Players.ElementAtOrDefault(1) == null)
        {
            foreach (BaseCommand command in Players[0].Player.Sequence)
            {
                CommandList.Add(new Tuple<Player, BaseCommand>(Players[0].Player, command));
            }

            StartCoroutine(RunSingleSequence(levelData, CommandList));
            return;
        }
        
        // Get both sequences (players)
        Player player1 = Players[0].Player;
        Player player2 = Players[1].Player;

        // Add all commands to the CommandList (based on Command.Priority)
        for (int i = 0; i < Mathf.Max(player1.Sequence.Count, player2.Sequence.Count); i++)
        {
            // If one seq is longer than other, last commands will only be of one player.
            // Auto-take the others command
            if (player1.Sequence[i] == null)
            {
                CommandList.Add(new Tuple<Player, BaseCommand>(player2, player2.Sequence[i]));
                continue;
            }
            if (player2.Sequence[i] == null)
            {
                CommandList.Add(new Tuple<Player, BaseCommand>(player1, player1.Sequence[i]));
                continue;
            }

            BaseCommand command1 = player1.Sequence[i];
            BaseCommand command2 = player2.Sequence[i];
            
            if (command1.Priority < command2.Priority)
            {
                // Higher priority (Player2) first
                CommandList.Add(new Tuple<Player, BaseCommand>(player2, command2));
                CommandList.Add(new Tuple<Player, BaseCommand>(player1, command1));
            }
            else
            {
                // Default (Player1 first)
                CommandList.Add(new Tuple<Player, BaseCommand>(player1, command1));
                CommandList.Add(new Tuple<Player, BaseCommand>(player2, command2));
            }
        }

        // execute commands in ordered list
        StartCoroutine(RunBothSequences(levelData, CommandList));
    }

    private IEnumerator RunBothSequences(LevelData levelData, List<Tuple<Player, BaseCommand>> CommandList)
    {

        // Run every SequenceStep
        for (int i = 0; i < CommandList.Count; i++)
        {
            Tuple<Player, BaseCommand> command1 = CommandList[i]; // Prio Command
            Tuple<Player, BaseCommand> command2 = CommandList[i+1]; // Non-Prio Command

            DateTime beforeExecute = DateTime.Now;
            StartCoroutine(command1.Item2.Execute(this, levelData, command1.Item1));
            yield return StartCoroutine(command2.Item2.Execute(this, levelData, command2.Item1));
            DateTime afterExecute = DateTime.Now;

            // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
            float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator RunSingleSequence(LevelData levelData, List<Tuple<Player, BaseCommand>> CommandList)
    {

        // Run every SequenceStep
        for (int i = 0; i < CommandList.Count; i++)
        {
            Tuple<Player, BaseCommand> command1 = CommandList[i]; // Prio Command

            DateTime beforeExecute = DateTime.Now;
            yield return StartCoroutine(command1.Item2.Execute(this, levelData, command1.Item1));
            DateTime afterExecute = DateTime.Now;

            // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
            float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;

            yield return new WaitForSeconds(delay);
        }
    }
}
