using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Grid.DataStructure;
using UnityEngine;

public class SequenceRunner : MonoBehaviour {
    
    void Awake()
    {
        EventManager.OnSimulate += ExecuteSequences;
    }

    private void ExecuteSequences(LevelData levelData, List<TGEPlayer> Players)
    {
        List<Tuple<Player, Sequence>> ExecutionSequences = new List<Tuple<Player, Sequence>>();
        foreach (TGEPlayer player in Players)
        {
            Sequence sequence = new Sequence();
            
            foreach (BaseCommand command in player.Player.Sequence)
            {
                // GetSimpleCommands
                sequence.AddRange(GetContainedCommands(command));
            }
            ExecutionSequences.Add(new Tuple<Player, Sequence>(player.Player, sequence));
        }

        // OrderedCommandList based on Command.Priority
        List<SequenceCycle> sequenceCycles = new List<SequenceCycle>();

        int MaxCycles = ExecutionSequences.Max(t =>t.Item2.Count);

        for (int i = 0; i < MaxCycles; i++)
        {
            SequenceCycle thisCycle = new SequenceCycle();
            foreach (Tuple<Player, Sequence> tuple in ExecutionSequences)
            {
                if (tuple.Item2.Count > i)
                    thisCycle.Commands.Add(new Tuple<Player, BaseCommand>(tuple.Item1, tuple.Item2[i]));
            }

            thisCycle.Commands = thisCycle.Commands.OrderByDescending(c => c.Item2.Priority).ToList();
            sequenceCycles.Add(thisCycle);
        }

        // execute commands in ordered list
        StartCoroutine(RunBothSequences(levelData, sequenceCycles));
    }

    private List<BaseCommand> GetContainedCommands(BaseCommand thisCommand)
    {
        // Only simple commands
        List <BaseCommand> commands = new List<BaseCommand>();

        // Return simple command
        if (!(thisCommand is LoopCommand))
        {
            commands.Add(thisCommand);
            return commands;
        }

        // Get all contained commands
        for (int i = 0; i < ((LoopCommand)thisCommand).LoopCount; i++)
        {
            foreach (BaseCommand command in ((LoopCommand)thisCommand).Sequence)
            {
                commands.AddRange(GetContainedCommands(command));
            }
        }

        return commands;
    }

    private IEnumerator RunBothSequences(LevelData levelData, List<SequenceCycle> Cycles)
    {

        // Run every SequenceStep
        for (int i = 0; i < Cycles.Count; i++)
        {
            List<Tuple<Player, BaseCommand>> thisCycle = Cycles[i].Commands;

            DateTime beforeExecute = DateTime.Now;
            // Execute every player's command (but only wait for the last one)
            for (int j = 0; j < thisCycle.Count - 1; j++)
            {
                StartCoroutine(thisCycle[j].Item2.Execute(this, levelData, thisCycle[j].Item1, Cycles[i]));
            }
            yield return StartCoroutine(thisCycle[thisCycle.Count-1].Item2.Execute(this, levelData, thisCycle[thisCycle.Count-1].Item1, Cycles[i]));
            DateTime afterExecute = DateTime.Now;

            // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
            float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;

            yield return new WaitForSeconds(delay);
        }
    }
}

public class SequenceCycle
{
    public List<Tuple<Player, BaseCommand>> Commands;

    public SequenceCycle()
    {
        Commands = new List<Tuple<Player, BaseCommand>>();
    }
}
