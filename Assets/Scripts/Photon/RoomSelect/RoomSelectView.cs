using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomSelectView : TGEMonoBehaviour
    {
        public override void Awake()
        {
            Init();
        }

        public void Init()
        {
            Enable();
            RoomEventManager.OnLocalPlayerJoinedRoom += Disable;
            RoomEventManager.OnLocalPlayerLeftRoom += Enable;
        }

        public void Enable()
        {
            base.Activate();
        }

        public void Disable()
        {
            base.Deactivate();
        }
    }
}