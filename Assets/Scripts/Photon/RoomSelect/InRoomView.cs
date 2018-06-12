using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Extensions;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class InRoomView : MonoBehaviour
    {
        public SimpleObjectPool PlayerPanelObjectPool;
        public GameObject PlayersViewPanel;
        public Button LeaveButton;
        public Button PlayButton;
        public Text RoomName;
        
        public void Awake()
        {
            Init();
        }

        public void Init()
        {            
            PlayButton.onClick.AddListener(StartGame);
#if !UNITY_EDITOR
            PhotonNetwork.player.NickName = Environment.UserName + " " + Guid.NewGuid().ToString().Substring(0, 6);           
#else
            PhotonNetwork.player.NickName = Guid.NewGuid().ToString();
#endif

            RoomEventManager.OnAllGameObjectsSpawned += OnAllGameObjectsSpawned;
            Disable();
        }

        private void OnAllGameObjectsSpawned()
        {
            print($"{nameof(InRoomView)}: {nameof(OnAllGameObjectsSpawned)} with objectpool: {PlayerPanelObjectPool}");
            RoomEventManager.OnLocalPlayerLeftRoom += Disable;
            RoomEventManager.OnLocalPlayerJoinedRoom += Enable;
            RoomEventManager.OnNetworkPlayerChanged += UpdatePlayersView;
            RoomEventManager.OnAllPlayersReady += OnAllPlayersReady;
            RoomEventManager.OnAnyPlayerUnready += OnAnyPlayerUnready;
            RoomEventManager.OnBecomingMasterClient += OnBecomingMasterClient;
        }

        void OnDestroy()
        {
            RoomEventManager.OnAllGameObjectsSpawned -= OnAllGameObjectsSpawned;
            RoomEventManager.OnLocalPlayerJoinedRoom -= Enable;
            RoomEventManager.OnLocalPlayerLeftRoom -= Disable;
            RoomEventManager.OnNetworkPlayerChanged -= UpdatePlayersView;
            RoomEventManager.OnAllPlayersReady -= OnAllPlayersReady;
            RoomEventManager.OnAnyPlayerUnready -= OnAnyPlayerUnready;
            RoomEventManager.OnBecomingMasterClient -= OnBecomingMasterClient;
        }

        private void OnBecomingMasterClient()
        {
            PlayButton.gameObject.SetActive(true);
        }

        private void OnAnyPlayerUnready()
        {
            PlayButton.interactable = false;
        }

        private void OnAllPlayersReady()
        {
            PlayButton.interactable = true;
        }

        public void Enable()
        {
            this.gameObject.SetActive(true);
            RoomName.text = PhotonNetwork.room.Name;
            OnAnyPlayerUnready();
            LeaveButton.onClick.RemoveAllListeners();
            LeaveButton.onClick.AddListener(LeaveRoom);
            UpdatePlayersView(RoomSelectPhotonManager.Instance.GetAllPlayersInRoom());
        }

        public void Disable()
        {
            PlayButton.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }

        public void UpdatePlayersView(List<PhotonPlayer> players)
        {            
            ClearPlayersView();

            foreach (var player in players)
            {
                var hashtable = player.CustomProperties;
                bool isReady = false;
                hashtable.TryGetTypedValue(RoomSelectPhotonManager.ReadyKey, out isReady);

                GameObject newPlayerPanel = PlayerPanelObjectPool.GetObject();
                newPlayerPanel.transform.SetParent(PlayersViewPanel.gameObject.transform, false);

                InRoomPlayerView playerPanel = newPlayerPanel.GetComponent<InRoomPlayerView>();
                print(playerPanel);
                playerPanel.Setup(player.NickName, this, player.IsLocal, isReady);              
            }           
        }

        public void ClearPlayersView()
        {
            foreach (Transform trans in PlayersViewPanel.transform)
            {
                Destroy(trans.gameObject);
            }
        }

        public void LeaveRoom()
        {
            RoomSelectPhotonManager.Instance.LeaveRoom();
        }

        public void StartGame()
        {            
            RoomSelectPhotonManager.Instance.SendGoToLevelSelect();
        }
    }
}