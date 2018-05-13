using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GlobalData : TGEMonoBehaviour {

        [SerializeField] private CommandLibrary AllCommands;
        public new GameInfo GameInfo { get; } = new GameInfo();

        public static GlobalData Instance;

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
                return;
            }

            GameInfo.AllCommands = AllCommands;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode args) {
            print($"GLOBAL: Loaded new scene {sceneLoaded.name}");
            gameObject.AddComponent<SceneDataLoader>();
        }

        public class SceneDataLoader : MonoBehaviour
        {
            public static UnityAction<GameInfo> OnSceneLoaded;
            public void Start()
            {
                // Invoke the event for any interested
                OnSceneLoaded?.Invoke(Instance.GameInfo);
                // Clear the event listeners since it's a one-shot event
                OnSceneLoaded = null;
                // Clean up myself since my job has been done!
                Destroy(this);
            }
        }
    }
}
