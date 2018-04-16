using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using System.Linq;
using UnityEngine.Assertions;

public static class PlayerHelper
{
    public static T GetLocalPlayer<T>(this List<T> players) where T : TGEPlayer
    {
        Assert.IsNotNull(players);
        return players.Single(x => x.photonPlayer.IsLocal);
    }

    public static T GetNetworkPlayers<T>(this List<T> players) where T : TGEPlayer
    {
        Assert.IsNotNull(players);
        return players.Single(x => !x.photonPlayer.IsLocal);
    }
}
