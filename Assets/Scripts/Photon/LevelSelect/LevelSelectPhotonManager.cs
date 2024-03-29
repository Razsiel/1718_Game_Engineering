﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Utilities;
using MonoBehaviour = Photon.MonoBehaviour;
using Assets.Scripts.UI;
using Photon;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Photon.LevelSelect
{
    public class LevelSelectPhotonManager : PunBehaviour
    {
        public SceneField LevelScene;
        private GameInfo _gameInfo;     
        private HorizontalScrollSnap _horizontalScrollSnap;        
        public SceneField MainScene;

        public static UnityAction TGEOnOtherPlayerLeft;

        public void Init(SceneField levelScene, GameInfo gameInfo, HorizontalScrollSnap scrollSnap, GameObject[] buttonsToDisableInMp)
        {
            this.LevelScene = levelScene;
            this._gameInfo = gameInfo;
            this._horizontalScrollSnap = scrollSnap;
            this.photonView.viewID = (int)PhotonViewIndices.LevelSelect;
            var isMaster = PhotonNetwork.player.IsMasterClient;
            foreach (var go in buttonsToDisableInMp) {
                go.SetActive(isMaster);
            }

            if (isMaster) {
                _horizontalScrollSnap.OnSelectionPageChangedEvent.AddListener(page => {
                    this.photonView.RPC(nameof(SelectedLevelChanged), PhotonTargets.Others, page);
                });
            }

            TGEOnOtherPlayerLeft += OnOtherPlayerDisconnected;
            BackButtonBehaviour.OnLeaveScene += OnLeaveScene;
        }

        private void OnLeaveScene()
        {
            PhotonNetwork.LeaveRoom(false);
        }

        private void OnOtherPlayerDisconnected()
        {
            PhotonNetwork.LeaveRoom(false);
            SceneManager.LoadScene(MainScene);
        }

        [PunRPC]
        private void SelectedLevelChanged(int page)
        {
            _horizontalScrollSnap.GoToScreen(page);
        }

        public void StartLevel(LevelData level)
        {
            this.photonView.RPC(nameof(SetLevel), PhotonTargets.Others, level.Name);
            PhotonNetwork.LoadLevel(LevelScene);
        }

        [PunRPC]
        public void SetLevel(string level, PhotonMessageInfo info)
        {
            _gameInfo.Level = _gameInfo.LevelLibrary.Levels.Single(x => x.Name == level);
            if (_gameInfo.Players == null || _gameInfo.Players.Count < 2)
            {               
                _gameInfo.Players = new List<TGEPlayer>
                {
                    new TGEPlayer { photonPlayer = PhotonNetwork.masterClient },
                    new TGEPlayer
                    {
                        photonPlayer =
                            PhotonNetwork.playerList.Single(x => !x.IsMasterClient)
                    }
                };
            }
        }

        public void OnDestroy()
        {
            BackButtonBehaviour.OnLeaveScene -= OnLeaveScene;
            TGEOnOtherPlayerLeft -= OnOtherPlayerDisconnected;
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            TGEOnOtherPlayerLeft?.Invoke();
        }
    }
}
