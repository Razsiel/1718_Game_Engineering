using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Photon
{
    class RoomListButton : MonoBehaviour
    {
        public Button buttonComponent;
        private RoomListView listView;
        private int roomNumber;

        void Start()
        {
            buttonComponent.onClick.AddListener(HandleClick);
        }

        public void Setup(int roomNumber, RoomListView listView)
        {
            this.roomNumber = roomNumber;
            this.listView = listView;
            buttonComponent.GetComponentInChildren<Text>().text = "Room " + roomNumber;
        }

        public void HandleClick()
        {
            listView.HandleClick(roomNumber);
        }
    }
}
