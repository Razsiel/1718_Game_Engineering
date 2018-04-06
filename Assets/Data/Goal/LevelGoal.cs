using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.Goal {
    public abstract class LevelGoal : ScriptableObject {
        public abstract bool HasBeenReached(List<Scripts.Player> players);
    }
}
