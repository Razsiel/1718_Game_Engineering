using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GlobalData : TGEMonoBehaviour
    {
        public new GameInfo GameInfo { get; } = new GameInfo();

        private static GlobalData _instance;
        public static GlobalData Instance {
            get { return _instance ?? (_instance = Spawn<GlobalData>(nameof(GlobalData))); }
            private set { _instance = value; }
        }

        public override void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode args) {
            print($"GLOBAL: Loaded new scene {sceneLoaded.name}");
        }
    }
}
