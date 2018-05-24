using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class BackButtonBehaviour : MonoBehaviour {
        private string _previousScene;

        void Awake() {
            GlobalData.SceneDataLoader.OnSceneLoaded += (previousScene, gameInfo) => {
                this._previousScene = previousScene;
            };
        }

        public void Back() {
            SceneManager.LoadScene(_previousScene);
        }
    }
}
