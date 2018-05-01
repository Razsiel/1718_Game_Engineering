using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class InRoomView : TGEMonoBehaviour
    {
        public SimpleObjectPool PlayerPanelObjectPool;
        public GameObject PlayersViewPanel;
        public Button LeaveButton;
        public Button PlayButton;
    
        public override void Awake()
        {
            Init();
        }

        public void Init()
        {
            Disable();
            RoomEventManager.OnLocalPlayerJoinedRoom += Enable;
            RoomEventManager.OnLocalPlayerLeftRoom += Disable;
            RoomEventManager.OnNetworkPlayerChanged += UpdatePlayersView;
            RoomEventManager.OnAllPlayersReady += OnAllPlayersReady;
            RoomEventManager.OnAnyPlayerUnready += OnAnyPlayerUnready;
        }

        private void OnAnyPlayerUnready()
        {
            PlayButton.GetComponentInChildren<Text>().color = Color.grey;
            PlayButton.interactable = false;
        }

        private void OnAllPlayersReady()
        {
            PlayButton.GetComponentInChildren<Text>().color = Color.black;
            PlayButton.interactable = true;
        }

        public void Enable()
        {
            base.Activate();
            LeaveButton.onClick.RemoveAllListeners();
            LeaveButton.onClick.AddListener(LeaveRoom);
            UpdatePlayersView(PhotonConnectionManager.Instance.GetAllPlayersInRoom());
        }

        public void Disable()
        {
            base.Deactivate();
        }

        public void UpdatePlayersView(List<PhotonPlayer> players)
        {
            ClearPlayersView();

            foreach (var player in players)
            {
                GameObject newPlayerPanel = PlayerPanelObjectPool.GetObject();
                newPlayerPanel.transform.SetParent(PlayersViewPanel.gameObject.transform);

                InRoomPlayerView playerPanel = newPlayerPanel.GetComponent<InRoomPlayerView>();
                print(playerPanel);
                playerPanel.Setup(player.NickName, this, player.IsLocal);
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
            PhotonConnectionManager.Instance.LeaveRoom();
        }

        public void StartGame()
        {
            //Load new scene 
        }
    }
}