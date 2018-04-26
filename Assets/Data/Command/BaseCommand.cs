using System;
using System.Collections;
using UnityEngine;

namespace Assets.Data.Command {
    [Serializable]
    public abstract class BaseCommand : ScriptableObject {
        public Sprite Icon;
        public string Name;
        public abstract IEnumerator Execute(Scripts.Player player);

        public override string ToString() {
            return this.GetType().Name;
        }
    }
}