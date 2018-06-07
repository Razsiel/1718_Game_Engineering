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

namespace Assets.Scripts.Photon.LevelSelect
{
    public class LevelSelectPhotonManager : MonoBehaviour
    {
        public SceneField LevelScene;
        private GameInfo _gameInfo;
        //public LevelLibrary LevelLibrary;
        private HorizontalScrollSnap _horizontalScrollSnap;

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
                print("Setting players again because the previous attempt failed");
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
    }
}
