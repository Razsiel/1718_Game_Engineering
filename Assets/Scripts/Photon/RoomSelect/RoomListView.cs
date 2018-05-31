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

        private void AddButtons(IReadOnlyCollection<RoomInfo> rooms) {
            print("Creating buttons for rooms: " + rooms.Count);
            //Add button foreach room
            foreach (RoomInfo roomInfo in rooms) {
                GameObject newButton = ButtonObjectPool.GetObject();
                newButton.transform.SetParent(Panel.transform, false);

                RoomListButton button = newButton.GetComponent<RoomListButton>();
                button.Setup(roomInfo);
            }
        }

        private void RemoveButtons()
        {
            //while (Panel.transform.childCount > 0)
            //{
            //    print("gonna remove a button: " + Panel.transform.childCount);
            //    GameObject toRemove = Panel.transform.GetChild(0).gameObject;
            //    ButtonObjectPool.ReturnObject(toRemove);
            //}
            print(Panel.transform);
            print(Panel.transform.childCount);
            foreach(Transform trans in Panel.transform)
            {
                print("Deleting child obj");
                GameObject toRemove = trans.gameObject;
                ButtonObjectPool.ReturnObject(toRemove);
            }
        }     
    }
}
