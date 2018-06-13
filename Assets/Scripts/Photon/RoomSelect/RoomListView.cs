using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Photon;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Photon.Level;
using UnityEngine.Assertions;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomListView : MonoBehaviour
    {
        public GameObject RoomButtonPrefab;
        public GameObject Panel;
        
        public void UpdateListView(RoomInfo[] rooms)
        {
            RemoveButtons();
            AddButtons(rooms);      
        }

        private void AddButtons(IReadOnlyCollection<RoomInfo> rooms)
        {         
            Assert.IsNotNull(RoomButtonPrefab);
            Assert.IsNotNull(Panel);

            foreach (RoomInfo roomInfo in rooms)
            {
                var newButton = Instantiate(RoomButtonPrefab, Panel.transform, false);

                var button = newButton.GetComponent<RoomListButton>();
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
