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
        public GameObject RoomButtonPrefab;
        public GameObject Panel;
        public Vector3 Margin;

        public void UpdateListView(RoomInfo[] rooms)
        {
            RemoveButtons();
            AddButtons(rooms);      
        }

        private void AddButtons(IReadOnlyCollection<RoomInfo> rooms) {
            print("Creating buttons for rooms: " + rooms.Count);
            //Add button foreach room
            foreach (RoomInfo roomInfo in rooms)
            {
                GameObject newButton = Instantiate(RoomButtonPrefab, Panel.transform, false);

                RoomListButton button = newButton.GetComponent<RoomListButton>();
                button.Setup(roomInfo);
            }
        }

        private void RemoveButtons()
        {
            foreach(Transform trans in Panel.transform)
            {
                Destroy(trans.gameObject);
            }
        }     
    }
}
