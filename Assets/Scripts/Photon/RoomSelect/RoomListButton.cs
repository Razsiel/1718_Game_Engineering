using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Photon.RoomSelect
{
    public class RoomListButton : MonoBehaviour
    {
        [SerializeField] private Button _buttonComponent;
        [SerializeField] private Text _roomNameText;
        [SerializeField] private Text _playerCountText;

        private string _roomName;

        void Awake()
        {
            Assert.IsNotNull(_buttonComponent);
            Assert.IsNotNull(_roomNameText);
            Assert.IsNotNull(_playerCountText);
        }
    
        public void Setup(RoomInfo roomInfo)
        {
            Assert.IsNotNull(roomInfo);
            _roomName = roomInfo.Name;
            _roomNameText.text = _roomName;
            _playerCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
        }

        public void OnClick() {
            Assert.IsNotNull(_roomName);
            Debug.Log("HandleClick: RoomListView" + " RoomName: " + _roomName);
            RoomSelectPhotonManager.Instance.JoinRoom(_roomName);
        }
    }
}
