using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI {
    public class BackButtonBehaviour : MonoBehaviour {
        private GameInfo _gameInfo;
        private string _previousScene;

        void Awake() {
            GlobalData.SceneDataLoader.OnSceneLoaded += (previousScene, gameInfo) => {
                this._previousScene = previousScene;
                this._gameInfo = gameInfo;
            };
        }

        public void Back() {
            if (_gameInfo.IsMultiplayer) {
                PhotonNetwork.LoadLevel(_previousScene);
            }
            else {
                SceneManager.LoadScene(_previousScene);
            }
        }
    }
}