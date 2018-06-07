using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using UnityEngine.Assertions;

namespace Assets.Scripts {
    public class GameInfo {
        public LevelLibrary LevelLibrary { get; set; }
        public LevelData Level { get; set; }
        public List<TGEPlayer> Players { get; set; }
        public bool IsMultiplayer { get; set; }
        public CommandLibrary AllCommands { get; set; }

        public TGEPlayer LocalPlayer {
            get {
                Assert.IsNotNull(Players);
                Assert.IsTrue(Players.Any());
                var _localPlayer = IsMultiplayer
                    ? Players.SingleOrDefault(p => p.photonPlayer.IsLocal) ?? Players.First()
                    : Players.First();
                Assert.IsNotNull(_localPlayer);
                return _localPlayer;
            }
        }

        private IEnumerable<BaseCommand> _cachedAllowedCommands;

        public IEnumerable<BaseCommand> AllowedCommands {
            get {
                // Only do the hard work the first time
//                if (_cachedAllowedCommands == null) {
//                    Assert.IsNotNull(Level);
//                    Assert.IsNotNull(Level.AllowedCommands);
//                    Assert.IsNotNull(AllCommands);
                _cachedAllowedCommands = Level.AllowedCommands
                                              .Where(
                                                  c => c
                                                      .IsAllowed) // only select allowed commands before cross-referencing enum to values
                                              .Join(AllCommands.Commands, // cross-ref source
                                                    command => command
                                                        .CommandType, // key of outer list to cross ref with inner key
                                                    kvp => kvp.Key, // key of inner list
                                                    (command, kvp) => kvp.Value); // select the baseCommand
//                }
                return _cachedAllowedCommands;
            }
        }
    }
}