using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Photon
{
    class RoomListButton : MonoBehaviour
    {
        public Button buttonComponent;
        private RoomListView listView;
        private string _roomName;
        public string RoomName
        {
            get
            {
                return _roomName;
            }
            private set
            {
                _roomName = value;
            }
        }

        void Awake()
        {            
            //To:do think of a way to make this button get the component
            buttonComponent = this.GetComponent<Button>();
            
            //buttonComponent.onClick.AddListener(() => HandleClick());

        }
    
        public void Setup(string roomName, RoomListView listView)
        {
            this.RoomName = roomName;
            this.listView = listView;
            Assert.IsNotNull(buttonComponent);
            Assert.IsNotNull(listView);
            buttonComponent.GetComponentInChildren<Text>().text = roomName;
            
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        public void HandleClick()
        {
            print("In HandleClick of our room");
            listView.HandleClick(_roomName);
        }
    }
}
