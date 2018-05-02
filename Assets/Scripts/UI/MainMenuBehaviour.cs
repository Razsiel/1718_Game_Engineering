using System.Collections.Generic;
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

        public void StartSingleplayer() {
            Assert.IsNotNull(LevelSelect);
            GlobalData.Instance.GameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            SceneManager.LoadScene(LevelSelect);
        }

        public void StartMultiplayer() {
            Assert.IsNotNull(LobbyScene);
            GlobalData.Instance.GameInfo.IsMultiplayer = true;
            GlobalData.Instance.GameInfo.Players = new List<TGEPlayer> {
                new TGEPlayer()
            };
            SceneManager.LoadScene(LobbyScene);
        }
    }
}