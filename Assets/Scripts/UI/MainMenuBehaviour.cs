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

        public void StartSingleplayer() {
            Assert.IsNotNull(LevelSelect);
            var gameInfo = GlobalData.Instance.GameInfo;
            gameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            gameInfo.LevelLibrary = SinglePlayerLevels;
            SceneManager.LoadScene(LevelSelect);
        }

        public void StartMultiplayer() {
            Assert.IsNotNull(LobbyScene);
            var gameInfo = GlobalData.Instance.GameInfo;
            gameInfo.IsMultiplayer = true;
            gameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            gameInfo.LevelLibrary = MultiPlayerLevels;
            SceneManager.LoadScene(LobbyScene);
        }
    }
}