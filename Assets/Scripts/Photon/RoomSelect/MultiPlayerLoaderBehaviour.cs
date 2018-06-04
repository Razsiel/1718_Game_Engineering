using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class MultiPlayerLoaderBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _roomButtonPrefab;
        [SerializeField] private GameObject _roomSelect;
        [SerializeField] private GameObject _photonManager;
        [SerializeField] private GameObject _InRoomScreen;
        [SerializeField] private GameObject _InRoomScreenPlayerObjectPool;
        [SerializeField] private SceneField _levelSelectScene;

        public void Awake()
        {
            var roomSelect = Instantiate(_roomSelect);
            var roomListView = roomSelect.GetComponentInChildren<RoomListView>();
            roomListView.RoomButtonPrefab = _roomButtonPrefab;
            print($"roomlistview: {roomListView}");

            var photonManager = Instantiate(_photonManager);
            var photonConnectionManager = photonManager.GetComponent<PhotonConnectionManager>();
            photonConnectionManager.LevelSelectScene = _levelSelectScene;
            photonConnectionManager.RoomListView = roomListView;
            photonManager.GetComponent<PhotonView>().viewID = (int)PhotonViewIndices.RoomSelect;
            
            var inRoomScreenPlayerObjectPool = Instantiate(_InRoomScreenPlayerObjectPool);
            print($"{nameof(inRoomScreenPlayerObjectPool)} {inRoomScreenPlayerObjectPool}");
            var inRoomScreen = Instantiate(_InRoomScreen);
            var inRoomView = inRoomScreen.GetComponent<InRoomView>();

            inRoomView.PlayerPanelObjectPool = inRoomScreenPlayerObjectPool.GetComponent<SimpleObjectPool>();

            RoomEventManager.AllGameObjectsSpawned();
            Destroy(this.gameObject);
        }
    }
}
