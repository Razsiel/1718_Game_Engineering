﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Data.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Sequence : IList<BaseCommand> {
        public List<BaseCommand> Commands { get; set; }
        private List<BaseCommand> _localPlayerCommands;

        public Sequence() {
            Commands = new List<BaseCommand>();
        }

        public void SequenceChanged() {
            EventManager.SequenceChanged(Commands);
        }

        public IEnumerator<BaseCommand> GetEnumerator() {
            return Commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(BaseCommand item)
        {
            Commands.Add(item);
            SequenceChanged();
        }

        public void Add(BaseCommand command, List<int> indices)
        {
            List<BaseCommand> commands = Commands;

            for (int i = 0; i < indices.Count; i++)
            {
                //If we're at the last index, we need to add the command at that index
                if (i == indices.Count - 1)
                {
                    Debug.Log("Voeg toe aan commands");

                    commands.Insert(indices[i], command);
                    SequenceChanged();
                    return;
                }
                //If its a loop, get the commands inside the loop
                if (commands[indices[i]] is LoopCommand)
                {
                    //Initialize loop if you need to
                    if (((LoopCommand)commands[indices[i]]).Sequence == null)
                        ((LoopCommand)commands[indices[i]]).Init();

                    commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                }
            }
            
        }

        public void AddRange(IEnumerable<BaseCommand> items, bool callEvent) {
            foreach (var item in items) {
                Commands.Add(item);
            }
            if(callEvent)
                SequenceChanged();
        }

        public void Clear() {
            Commands.Clear();
            SequenceChanged();
        }

        public void Clear(bool sendEvent)
        {
            Commands.Clear();
            if (sendEvent)
            {
                SequenceChanged();
            }
        }

        public void SwapAtindices(int i, int j)
        {
            BaseCommand temp = Commands[j];
            Commands[j] = Commands[i];
            Commands[i] = temp;
            SequenceChanged();
        }

        public void MoveFromIndexToIndex(int fromIndex, int toIndex, BaseCommand command)
        {
            RemoveAt(fromIndex);
            Insert(toIndex, command);
        }

        public void RemoveFromIndicesAndAddToIndices(List<int> fromIndices, List<int> toIndices, bool addToLoop)
        {
            //If the swap is taking place at the surface level
            if (fromIndices.Count == 1 && toIndices.Count == 1)
            {
                MoveFromIndexToIndex(fromIndices[0], toIndices[0], Commands[fromIndices[0]]);
            }
            else
            {
                BaseCommand temp = null;
                BaseCommand fromCommand = null;
                BaseCommand toCommand = null;

                List<BaseCommand> commands = Commands;

                fromCommand = GetCommandForListOfindices(fromIndices, commands);

                if (ToIndicesIsGreaterThanFromIndices(toIndices, fromIndices))
                {
                    Add(fromCommand, toIndices);
                    RemoveAt(fromIndices);
                }
                else
                {
                    RemoveAt(fromIndices);
                    Add(fromCommand, toIndices);
                }
            }
        }

        private bool ToIndicesIsGreaterThanFromIndices(List<int> toIndices, List<int> fromIndices)
        {
            int shortestList = toIndices.Count <= fromIndices.Count ? toIndices.Count : fromIndices.Count;
            for (int i = 0; i < shortestList; i++)
            {
                if (toIndices[i] > fromIndices[i])
                {
                    return true;
                }else if (toIndices[i] < fromIndices[i])
                {
                    return false;
                }
            }

            return true;
        }

        private BaseCommand GetCommandForListOfindices(List<int> indices, List<BaseCommand> commands)
        {
            if (indices.Count == 1)
            {
                return commands[indices[0]];
            }

            BaseCommand command = null;
            for (int i = 0; i < indices.Count; i++)
            {
                //If we're at the last index to check, return the command
                if (i == indices.Count - 1)
                {
                    return commands[indices[i]];
                }

                if (commands[indices[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand)commands[indices[i]]).Sequence != null &&
                        ((LoopCommand)commands[indices[i]]).Sequence.Commands.Count > 0)
                    {
                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    } //If the loop has no children
                    else
                    {
                        command = commands[indices[i]];
                    }
                }
            }

            return command;
        }

        public bool isEmpty(int index)
        {
            return Commands.Count <= index;
        }

        //See if index is empty in deeper level
        public bool isEmpty(List<int> indices)
        {
            if (indices.Count == 1)
            {
                return isEmpty(indices[0]);
            }

            List<BaseCommand> commands = Commands;
            BaseCommand command = null;

            command = GetCommandForListOfindices(indices, commands);

            if (command == null)
                return true;
            else
                return false;
            
        }

        public bool Contains(BaseCommand item) {
            return Commands.Contains(item);
        }

        public void CopyTo(BaseCommand[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(BaseCommand item) {
            var isRemoved = Commands.Remove(item);
            SequenceChanged();
            return isRemoved;
        }

        public int Count => Commands.Count;
        public bool IsReadOnly => false;
        public int IndexOf(BaseCommand item) {
            throw new NotImplementedException();
        }

        public void Insert(int index, BaseCommand item) {
            Commands.Insert(index, item);
            SequenceChanged();
        }

        public void RemoveAt(int index)
        {
            if (Commands.ElementAtOrDefault(index) == null)
                return;

            Commands.RemoveAt(index);
            SequenceChanged();
        }

        public void RemoveAt(List<int> indices)
        {
            //Item being removed is directly in the sequence bar
            if (indices.Count == 1)
            {
                RemoveAt(indices[0]);
                return;
            }

            List<BaseCommand> commands = Commands;
            for (int i = 0; i < indices.Count; i++)
            {
                //If its a loop, get the commands inside the loop
                if (commands[indices[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand) commands[indices[i]]).Sequence != null && 
                        ((LoopCommand)commands[indices[i]]).Sequence.Commands.Count != 0)
                    {

                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    }//If the loop has no children, remove the loop
                    else
                    {                    
                        commands.RemoveAt(indices[i]);
                        SequenceChanged();
                        return;
                    }
                }//If its not a loop, the command has to be deleted
                else
                {

                    commands.RemoveAt(indices[i]);
                    SequenceChanged();
                    return;
                }
            }   
        }

        public void LoopEdited(string newAmountOfLoops, List<int> indices)
        {
            LoopCommand command = null;
            command = (LoopCommand) GetCommandForListOfindices(indices, Commands);
            command.LoopCount = int.Parse(newAmountOfLoops);
            SequenceChanged();
        }

        public BaseCommand this[int index] {
            get { return Commands[index]; }
            set { Commands[index] = value; }
        }
        
        public IEnumerable<BaseCommand> Expanded(bool returnLoop) {
            var expanded = new List<BaseCommand>();
            foreach (var command in Commands) {
                if (command is LoopCommand) {
                    if(returnLoop)
                        expanded.Add(command);

                    var loop = command as LoopCommand;
                    if (!returnLoop)
                    {
                        for (int i = 0; i < loop.LoopCount; i++)
                        {
                            expanded.AddRange(loop.Sequence.Expanded(returnLoop));
                        }
                    }
                    else
                    {
                        expanded.AddRange(loop.Sequence.Expanded(returnLoop));
                    }
                }
                else
                {
                    expanded.Add(command);
                }
            }
            return expanded;
        }
    }
}
