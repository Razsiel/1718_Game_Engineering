using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Utilities;

namespace Assets {
    public class MainMenuBehaviour : MonoBehaviour {
        [SerializeField] private SceneField LevelSelect;
        [SerializeField] private SceneField LobbyScene;

        [SerializeField] private LevelLibrary SinglePlayerLevels;
        [SerializeField] private LevelLibrary MultiPlayerLevels;

        private GameInfo _gameInfo;

        void Awake() {
            GlobalData.SceneDataLoader.OnSceneLoaded += (previousScene, gameInfo) => {
                _gameInfo = gameInfo;
            };
        }

        public void StartSingleplayer() {
            Assert.IsNotNull(LevelSelect);
            _gameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            _gameInfo.LevelLibrary = SinglePlayerLevels;
            SceneManager.LoadScene(LevelSelect);
        }

        public void StartMultiplayer() {
            Assert.IsNotNull(LobbyScene);
            _gameInfo.IsMultiplayer = true;
            _gameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            _gameInfo.LevelLibrary = MultiPlayerLevels;
            SceneManager.LoadScene(LobbyScene);
        }

        public void HideMainMenuPanel()
        {
            gameObject.SetActive(false);
        }

        public void ShowMainMenuPanel()
        {
            gameObject.SetActive(true);
        }

        public void CloseApplication() {
            Application.Quit();
        }
    }
}