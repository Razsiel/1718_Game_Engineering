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

        public override void Show()
        {
            base.Show();
            RoomName.Select();
        }

        public override void Cancel()
        {
            RoomName.text = string.Empty;
            base.Close();
        }

        public override void Submit()
        {
            PhotonConnectionManager.Instance.CreateRoom(RoomName.text);
            RoomName.text = string.Empty;
            base.Close();
        }
    }
}
