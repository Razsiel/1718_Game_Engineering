using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data.Goal {
    public abstract class LevelGoalData : ScriptableObject {
        public abstract bool HasBeenReached(IEnumerable<Scripts.Player> players);
    }
}
