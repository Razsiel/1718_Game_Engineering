using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Scripts.Photon.Level;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class GameLoaderBehaviour : TGEMonoBehaviour
    {
        [SerializeField]
        private CommandLibrary _commandLibrary;

        [SerializeField]
        private GameObject _cameraContainerPrefab;
        [SerializeField]
        private GameObject _uiPrefab;
        [SerializeField]
        private GameObject _monologuePrefab;
        [SerializeField]
        private GameObject _playerPrefab;
        [SerializeField]
        private GameObject _sequenceRunnerPrefab;
        [SerializeField]
        private GameObject _photonPrefab;

        // Use this for initialization
        public override void Start()
        {
            Debug.Log($"{nameof(GameLoaderBehaviour)}: Start setting up the scene");
            var managersRoot = new GameObject("Managers");
            var gameRoot = new GameObject("Game");

            // Managers
            var gameStateManager = Spawn<GameStateManager>("GameStateManager", managersRoot);
            var levelPresenter = Spawn<LevelManager>("LevelPresentation", managersRoot);
            levelPresenter.PlayerPrefab = _playerPrefab;

            var photonManager = Instantiate(_photonPrefab, managersRoot.transform);
            photonManager.GetComponent<PhotonManager>().CommandLib = _commandLibrary;

            var scoreManager = Spawn<PlayerScoreManager>("ScoreManager", managersRoot);

            // Gameworld
            var cameraContainer = GameObject.Instantiate(_cameraContainerPrefab, gameRoot.transform);
            var ui = GameObject.Instantiate(_uiPrefab, gameRoot.transform);
            var monologue = GameObject.Instantiate(_monologuePrefab, gameRoot.transform);
            var sequenceRunner = GameObject.Instantiate(_sequenceRunnerPrefab, gameRoot.transform);

            Debug.Log($"{nameof(GameLoaderBehaviour)}: Finished setting up the scene. Cleaning myself up");
            Destroy(this.gameObject);
        }
    }
}