using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ScriptableObjects.Player;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
    public class TGEPlayer
    {
        //public PlayerData playerData;
        public PhotonPlayer photonPlayer;
        public Player player;
        //Other fields which could be interesting

        public GameObject PlayerObject;
    }
}
