using Assets.Data.Levels;
using UnityEngine;

namespace Assets.Scripts.Behaviours {
    public class GameLoaderBehaviour : TGEMonoBehaviour {

        [SerializeField] private LevelData _level;

        [SerializeField] private GameObject _cameraContainerPrefab;
        [SerializeField] private GameObject _uiPrefab;
        [SerializeField] private GameObject _monologuePrefab;
        [SerializeField] private GameObject _audioPrefab;
        [SerializeField] private GameObject _playerPrefab;

        // Use this for initialization
        public override void Start() {
            Debug.Log($"{nameof(GameLoaderBehaviour)}: Start setting up the scene");
            var managersRoot = new GameObject("Managers");
            var gameRoot = new GameObject("Game");

            // Managers
            var gameStateManager = Spawn<GameStateManager>("GameStateManager", managersRoot, manager => {
                manager.Level = _level;
            });
            var levelPresenter = Spawn<LevelManager>("LevelPresentation", managersRoot, manager => {
                manager.GameRoot = gameRoot;
                manager.PlayerPrefab = _playerPrefab;
            });
            var audioManager = Instantiate(_audioPrefab, managersRoot.transform);

            // Gameworld
            var cameraContainer = GameObject.Instantiate(_cameraContainerPrefab, gameRoot.transform);
            var ui = GameObject.Instantiate(_uiPrefab, gameRoot.transform);
            var monologue = GameObject.Instantiate(_monologuePrefab, ui.transform);

            Debug.Log($"{nameof(GameLoaderBehaviour)}: Finished setting up the scene. Cleaning myself up");
            Destroy(this.gameObject);
        }
    }
}