using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
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
            this.photonView.RPC(nameof(SetLevel), PhotonTargets.Others, level);
            PhotonNetwork.LoadLevel(LevelScene);
        }

        [PunRPC]
        public void SetLevel(LevelData level, PhotonMessageInfo info)
        {
            _gameInfo.Level = level;
        }
    }
}
