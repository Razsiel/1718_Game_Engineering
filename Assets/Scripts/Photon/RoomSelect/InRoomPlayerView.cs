using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class InRoomPlayerView : MonoBehaviour
    {
        public Text PlayerName;
        public Toggle PlayerReadyState;
        public InRoomView ParentView;

        public void Awake()
        {
            this.PlayerName = this.gameObject.transform.GetComponentInChildren<Text>();      
            this.PlayerReadyState = this.gameObject.transform.GetComponentInChildren<Toggle>();

            Assert.IsNotNull(PlayerName);
            Assert.IsNotNull(PlayerReadyState);
        }

        public void Setup(string playerName, InRoomView parentView, bool isLocalPlayer, bool isReady)
        {
            this.ParentView = parentView;
            this.PlayerName.text = playerName;
            this.PlayerReadyState.isOn = isReady;
         
            Assert.IsNotNull(parentView);

            PlayerReadyState.interactable = isLocalPlayer;
            if (isLocalPlayer)
            {
                PlayerReadyState.onValueChanged.AddListener(delegate { PlayerReadyStateChanged(); });
            }
            else
            {              
                RoomEventManager.OnNetworkPlayerPropertiesChanged += OnPlayerPropertiesChanged;
            }
        }

        private void OnPlayerPropertiesChanged(bool readyvalue)
        {                       
            PlayerReadyState.isOn = readyvalue;            
        }

        public void PlayerReadyStateChanged()
        {
           RoomEventManager.OnLocalPlayerReadyStateChanged?.Invoke(PlayerReadyState.isOn);
        }
    }
}
