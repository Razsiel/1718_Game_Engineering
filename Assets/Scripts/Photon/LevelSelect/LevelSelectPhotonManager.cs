﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Photon.LevelSelect
{
    public class LevelSelectPhotonManager : MonoBehaviour
    {
        public GameObject PlayButton;
        public SceneField LevelScene;
        private GameInfo _gameInfo;
        //public LevelLibrary LevelLibrary;

        public void Init(GameObject playButton, SceneField levelScene, GameInfo gameInfo)
        {
            this.PlayButton = playButton;
            this.LevelScene = levelScene;
            this._gameInfo = gameInfo;
            this.photonView.viewID = (int)PhotonViewIndices.LevelSelect;
            PlayButton.SetActive(PhotonNetwork.player.IsMasterClient);
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
