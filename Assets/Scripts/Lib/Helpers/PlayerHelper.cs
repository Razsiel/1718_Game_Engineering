using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using System.Linq;
using UnityEngine.Assertions;
using Assets.Scripts;

namespace Assets.Scripts.Lib.Helpers
{
    public static class PlayerHelper
    {
        public static T GetLocalPlayer<T>(this List<T> players) where T : TGEPlayer
        {
            Assert.IsNotNull(players);
            return players.Single(p => p.photonPlayer == null || p.photonPlayer.IsLocal);
        }

        public static T GetNetworkPlayer<T>(this List<T> players) where T : TGEPlayer
        {
            Assert.IsNotNull(players);
            return players.Single(x => !x.photonPlayer.IsLocal);
        }

        public static T GetMasterClientPlayer<T>(this List<T> players) where T : TGEPlayer
        {
            Assert.IsNotNull(players);
            return players.Single(x => x.photonPlayer.IsMasterClient);
        }
    }
}
