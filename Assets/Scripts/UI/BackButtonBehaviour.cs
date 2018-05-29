using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Utilities;

namespace Assets.Scripts.UI {
    public class BackButtonBehaviour : MonoBehaviour {
        private GameInfo _gameInfo;
        [SerializeField] private SceneField _previousSinglePlayerScene;
        [SerializeField] private SceneField _previousMultiplayerPlayerScene;

        void Awake() {
            Assert.IsNotNull(_previousSinglePlayerScene);
            Assert.IsNotNull(_previousMultiplayerPlayerScene);
            GlobalData.SceneDataLoader.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(GameInfo gameInfo) {
            GlobalData.SceneDataLoader.OnSceneLoaded -= OnSceneLoaded;
            this._gameInfo = gameInfo;
        }

        public void Back() {
            if (_gameInfo.IsMultiplayer) {
                PhotonNetwork.LoadLevel(_previousMultiplayerPlayerScene);
            }
            else {
                SceneManager.LoadScene(_previousSinglePlayerScene);
            }
        }
    }
}