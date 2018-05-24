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
            RemoveButtons();
            AddButtons(rooms);      
        }

        private void AddButtons(RoomInfo[] rooms)
        {
            print("Creating buttons for rooms: " + rooms.Length);
            //Add button foreach room
            for (int i = 0; i < rooms.Length; i++)
            {
                GameObject newButton = ButtonObjectPool.GetObject();
                newButton.transform.SetParent(Panel.transform, false);

                RoomListButton button = newButton.GetComponent<RoomListButton>();
                button.RoomNameExPlayers = rooms[i].Name;
                string roomName = rooms[i].Name + " Players: " + rooms[i].PlayerCount + "/" + rooms[i].MaxPlayers;
                
                button.Setup(roomName, this);

                newButton.GetComponent<Button>().onClick.AddListener(() => HandleClick(button.RoomNameExPlayers));

            }
        }

        private void RemoveButtons()
        {
            while (Panel.transform.childCount > 0)
            {
                print("gonna remove a button: " + Panel.transform.childCount);
                GameObject toRemove = Panel.transform.GetChild(0).gameObject;
                ButtonObjectPool.ReturnObject(toRemove);
            }
            print(Panel.transform);
            print(Panel.transform.childCount);
            //foreach (Transform trans in Panel.transform)
            //{
            //    print("Deleting child obj");
            //    GameObject toRemove = trans.gameObject;
            //    ButtonObjectPool.ReturnObject(toRemove);
            //}
        }

        public void HandleClick(string roomName)
        {
            Debug.Log("HandleClick: RoomListView" + " RoomName: " + roomName);
            PhotonConnectionManager.Instance.JoinRoom(roomName);
        }      
    }
}
