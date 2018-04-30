using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class CreateRoomModal : ModalPanel
    {
        public InputField RoomName;

        private void Awake()
        {
            this.Panel = this.gameObject;            
        }

        public override void Cancel()
        {
            base.Close();
        }

        public override void Submit()
        {
            string roomName = RoomName.text;
            print(roomName);
            PhotonConnectionManager.Instance.CreateRoom(roomName);
        }
    }
}
