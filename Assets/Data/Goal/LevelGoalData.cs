using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Data.Goal {
    public abstract class LevelGoalData : PresentableScriptableObject {
        public abstract bool HasBeenReached(IEnumerable<Scripts.Player> players);
    }
}
