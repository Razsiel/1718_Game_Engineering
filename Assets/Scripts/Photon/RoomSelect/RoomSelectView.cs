using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomSelectView : TGEMonoBehaviour
    {
        public InputField RoomName;

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

        public void CreateRoom()
        {
            PhotonConnectionManager.Instance.CreateRoom(RoomName.text == "" ? " " : RoomName.text );
        }

        public void Enable()
        {
            base.Activate();
        }

        public void Disable()
        {
            base.Deactivate();
        }

        public void OnDestroy()
        {
            RoomEventManager.OnLocalPlayerJoinedRoom -= Disable;
            RoomEventManager.OnLocalPlayerLeftRoom -= Enable;
        }
    }
}