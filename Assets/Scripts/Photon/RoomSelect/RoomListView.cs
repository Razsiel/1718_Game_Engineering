using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Photon;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon.Level;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomListView : MonoBehaviour
    {
        //public GameObject buttonPrefab;
        public GameObject Panel;
        public Vector3 Margin;
        public SimpleObjectPool ButtonObjectPool;       

        public void UpdateListView(RoomInfo[] rooms)
        {
            AddButtons(rooms);      
        }

        private void AddButtons(RoomInfo[] rooms)
        {
            print("Creating buttons for rooms: " + rooms.Length);
            //Add button foreach room
            for (int i = 0; i < rooms.Length; i++)
            {
                GameObject newButton = ButtonObjectPool.GetObject();
                newButton.transform.SetParent(Panel.transform);

                RoomListButton button = newButton.GetComponent<RoomListButton>();
                string roomName = rooms[i].Name + " Players: " + rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers;
               
                button.Setup(roomName, this);

                newButton.GetComponent<Button>().onClick.AddListener(() => HandleClick(button.RoomName));

            }
        }

        private void RemoveButtons()
        {
            while (Panel.transform.childCount > 0)
            {
                GameObject toRemove = transform.GetChild(0).gameObject;
                ButtonObjectPool.ReturnObject(toRemove);
            }
        }

        public void HandleClick(string roomName)
        {
            Debug.Log("HandleClick: RoomListView");
            PhotonManager.Instance.JoinRoom(roomName);
        }

        private void UpdateUI()
        {

        }
    }
}
