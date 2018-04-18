using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.Goal {
    public abstract class LevelGoal : ScriptableObject {
        public abstract bool HasBeenReached(IEnumerable<Scripts.Player> players);
    }
}
