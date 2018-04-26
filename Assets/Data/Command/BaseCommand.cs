using System;
using System.Collections;
using Assets.Data.Levels;
using UnityEngine;

namespace Assets.Data.Command {
    [Serializable]
    public abstract class BaseCommand : ScriptableObject {
        public Sprite Icon;
        public string Name;
        public abstract IEnumerator Execute(MonoBehaviour coroutineRunner, LevelData level, Scripts.Player player);

        public override string ToString() {
            return this.GetType().Name;
        }
    }
}