using System;
using System.Collections;
using Assets.Data.Levels;
using UnityEngine;

namespace Assets.Data.Command {
    [Serializable]
    public abstract class BaseCommand : ScriptableObject {
        public Sprite Icon;
        public string Name;
        public int Priority;
        public abstract IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player, SequenceCycle cycle);

        public virtual BaseCommand Init() {
            return this;
        }

        public override string ToString() {
            return this.GetType().Name;
        }
    }
}