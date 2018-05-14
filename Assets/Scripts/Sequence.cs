using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Data.Levels;
using NPOI.SS.UserModel;
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
            EventManager.SequenceChanged();
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
                    //If the loop has children, get them
                    if (((LoopCommand)commands[indices[i]]).Sequence != null)
                    {
                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    }//If the loop has no children, initialize the loop 
                    else
                    {
                        ((LoopCommand) commands[indices[i]]).Init();
                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    }

                }//If its not a loop then i dont know what is going on
                else
                {
                    Debug.Log("Huh??");
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

        public void SwapAtindices(int i, int j)
        {
            BaseCommand temp = Commands[j];
            Commands[j] = Commands[i];
            Commands[i] = temp;
            SequenceChanged();
        }

        public void SwapAtindices(List<int> fromindices, List<int> toindices, bool addToLoop)
        {
            //If the swap is taking place at the surface level
            if (fromindices.Count == 1 && toindices.Count == 1)
            {
                SwapAtindices(fromindices[0], toindices[0]);
            }
            else
            {
                BaseCommand temp = null;
                BaseCommand fromCommand = null;
                BaseCommand toCommand = null;

                List<BaseCommand> commands = Commands;

                fromCommand = GetCommandForListOfindices(fromindices, commands, fromCommand);

                commands = Commands;

                Tuple<BaseCommand, int> toCommandInfo = GetLoopAndIndexToInsertOrAdd(toindices, commands, toCommand);

                if (addToLoop)
                {
                    ((LoopCommand)toCommand).Sequence.Add(fromCommand);
                    Remove(fromCommand);
                }
                else
                {
                    //Swap the commands
                    temp = fromCommand;
                    fromCommand = toCommand;
                    toCommand = temp;
                }
            }
        }

        private Tuple<BaseCommand, int> GetLoopAndIndexToInsertOrAdd(List<int> indices, List<BaseCommand> commands, BaseCommand command)
        {
            int returnIndex = 0;

            for (int i = 0; i < indices.Count; i++)
            {
                //The index doesnt fit in the loop, we should add it to the loop
                if (indices[i] >= commands.Count)
                {
                    return new Tuple<BaseCommand, int>((LoopCommand)commands[commands.Count], commands.Count);
                }
                else if (commands[indices[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand)commands[indices[i]]).Sequence != null &&
                        ((LoopCommand)commands[indices[i]]).Sequence.Commands.Count > 0)
                    {
                        //If we're at the last index to check, take the loop

                        Debug.Log("pak de kinderen van de loop");

                        command = ((LoopCommand)commands[indices[i]]);
                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    } //If the loop has no children
                    else
                    {
                        ((LoopCommand)commands[indices[i]]).Init();
                        return commands[indices[i]];
                    }
                }
                else
                {

                    return command;
                }
            }

            return command;
        }

        private BaseCommand GetCommandForListOfindices(List<int> indices, List<BaseCommand> commands, BaseCommand command)
        {
            if (indices.Count == 1)
            {
                return commands[indices[0]];
            }
            for (int i = 0; i < indices.Count; i++)
            {

                if (indices[i] >= commands.Count)
                {
                    return command;
                }
                else if (commands[indices[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand) commands[indices[i]]).Sequence != null && 
                        ((LoopCommand) commands[indices[i]]).Sequence.Commands.Count > 0)
                    {
                        //If we're at the last index to check, take the loop
                        if (i == indices.Count - 1)
                        {
                            command = ((LoopCommand) commands[indices[i]]);
                        }
                        Debug.Log("pak de kinderen van de loop");

                        command = ((LoopCommand) commands[indices[i]]);
                        commands = ((LoopCommand) commands[indices[i]]).Sequence.Commands;
                    } //If the loop has no children
                    else
                    {
                        ((LoopCommand) commands[indices[i]]).Init();
                        return commands[indices[i]];
                    }
                }
                else
                {

                    return command;
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

            command = GetCommandForListOfindices(indices, commands, command);

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
                        Debug.Log("pak de kinderen van de loop");

                        commands = ((LoopCommand)commands[indices[i]]).Sequence.Commands;
                    }//If the loop has no children, remove the loop
                    else
                    {
                        Debug.Log("verwijderen de lege loop");
                    
                        commands.RemoveAt(indices[i]);
                        SequenceChanged();
                        
                    }
                }//If its not a loop, the command has to be deleted
                else
                {
                    commands.RemoveAt(indices[i]);
                    //I dont know why sequence changed is not called unless i put it here
                    SequenceChanged();
                }
            }
        }

        public void LoopEdited(string newAmountOfLoops, List<int> indices)
        {
            LoopCommand command = null;
            command = (LoopCommand) GetCommandForListOfindices(indices, Commands, command);
            command.LoopCount = int.Parse(newAmountOfLoops);
            SequenceChanged();
        }

        public BaseCommand this[int index] {
            get { return Commands[index]; }
            set { Commands[index] = value; }
        }

//        public IEnumerator Run(MonoBehaviour coroutineRunner, LevelData level, Player player) {
//            foreach (BaseCommand command in this)
//            {
//                DateTime beforeExecute = DateTime.Now;
//                yield return coroutineRunner.StartCoroutine(command.Execute(coroutineRunner, level, player));
//                DateTime afterExecute = DateTime.Now;
//
//                // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
//                float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;
//
//                yield return new WaitForSeconds(delay);
//            }
//        }
    }
}
