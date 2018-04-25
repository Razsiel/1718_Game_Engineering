using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using UnityEngine.Assertions;

namespace Assets.Scripts {
    public class GameInfo {
        public LevelData Level { get; set; }
        public List<TGEPlayer> Players { get; set; }
        public bool IsMultiplayer { get; set; }

        public TGEPlayer LocalPlayer {
            get {
                Assert.IsNotNull(Players);
                Assert.IsTrue(Players.Any());
                var localPlayer = IsMultiplayer ? Players.SingleOrDefault(p => p.photonPlayer.IsLocal) ?? Players.First() : Players.First();
                Assert.IsNotNull(localPlayer);
                return localPlayer;
            }
        }
    }
}