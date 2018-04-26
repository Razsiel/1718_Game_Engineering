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

        public UnityAction<List<BaseCommand>> OnSequenceChanged;
        public void SequenceChanged(List<BaseCommand> commands) {
            OnSequenceChanged?.Invoke(commands);
        }

        public IEnumerator<BaseCommand> GetEnumerator() {
            return Commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(BaseCommand item) {
            Commands.Add(item);
            SequenceChanged(Commands);
        }

        public void AddRange(IEnumerable<BaseCommand> items) {
            foreach (var item in items) {
                Commands.Add(item);
            }
            SequenceChanged(Commands);
        }

        public void Clear() {
            Commands.Clear();
            SequenceChanged(Commands);
        }

        public bool Contains(BaseCommand item) {
            return Commands.Contains(item);
        }

        public void CopyTo(BaseCommand[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(BaseCommand item) {
            var isRemoved = Commands.Remove(item);
            SequenceChanged(Commands);
            return isRemoved;
        }

        public int Count => Commands.Count;
        public bool IsReadOnly => false;
        public int IndexOf(BaseCommand item) {
            throw new NotImplementedException();
        }

        public void Insert(int index, BaseCommand item) {
            Commands.Insert(index, item);
            SequenceChanged(Commands);
        }

        public void RemoveAt(int index) {
            Commands.RemoveAt(index);
            SequenceChanged(Commands);
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
