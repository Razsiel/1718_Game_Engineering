using System;
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
            Debug.Log("changeSequenmce");
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

        public void Add(BaseCommand command, List<int> indexes)
        {
            List<BaseCommand> commands = Commands;

            for (int i = 0; i < indexes.Count; i++)
            {
                //If we're at the last index, we need to add the command at that index
                if (i == indexes.Count - 1)
                {
                    Debug.Log("Voeg toe aan commands");

                    commands.Insert(indexes[i], command);
                    SequenceChanged();
                    return;
                }
                //If its a loop, get the commands inside the loop
                if (commands[indexes[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand)Commands[indexes[i]]).Sequence != null)
                    {
                        commands = ((LoopCommand)Commands[indexes[i]]).Sequence.Commands;
                    }//If the loop has no children, initialize the loop 
                    else
                    {
                        ((LoopCommand) Commands[indexes[i]]).Init();
                        commands = ((LoopCommand) Commands[indexes[i]]).Sequence.Commands;
                    }

                }//If its not a loop then i dont know what is going on
                else
                {
                    Debug.Log("Huh??");
                }
            }
            
        }

        public void AddRange(IEnumerable<BaseCommand> items) {
            foreach (var item in items) {
                Commands.Add(item);
            }
            SequenceChanged();
        }

        public void Clear() {
            Commands.Clear();
            SequenceChanged();
        }

        public void SwapAtIndexes(int i, int j)
        {
            BaseCommand temp = Commands[j];
            Commands[j] = Commands[i];
            Commands[i] = temp;
            SequenceChanged();
        }

        public bool isEmpty(int index)
        {
            return Commands.Count <= index;
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

        public void RemoveAt(List<int> indexes)
        {
            //Item being removed is directly in the sequence bar
            if (indexes.Count == 1)
            {
                RemoveAt(indexes[0]);
                return;
            }

            List<BaseCommand> commands = Commands;
            for (int i = 0; i < indexes.Count; i++)
            {
                //If its a loop, get the commands inside the loop
                if (commands[indexes[i]] is LoopCommand)
                {
                    //If the loop has children, get them
                    if (((LoopCommand) Commands[indexes[i]]).Sequence != null)
                    {
                        Debug.Log("pak de kinderen van de loop");

                        commands = ((LoopCommand)Commands[indexes[i]]).Sequence.Commands;
                    }//If the loop has no children, remove the loop
                    else
                    {
                        Debug.Log("verwijderen de lege loop");
                    
                        commands.RemoveAt(indexes[i]);
                        
                    }
                }//If its not a loop, the command has to be deleted
                else
                {
                    commands.RemoveAt(indexes[i]);
                    //I dont know why sequence changed is not called unless i put it here
                    SequenceChanged();
                }
            }
        }

        public BaseCommand this[int index] {
            get { return Commands[index]; }
            set { Commands[index] = value; }
        }

        public IEnumerator Run(MonoBehaviour coroutineRunner, LevelData level, Player player) {
            foreach (BaseCommand command in this)
            {
                DateTime beforeExecute = DateTime.Now;
                yield return coroutineRunner.StartCoroutine(command.Execute(coroutineRunner, level, player));
                DateTime afterExecute = DateTime.Now;

                // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
                float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
