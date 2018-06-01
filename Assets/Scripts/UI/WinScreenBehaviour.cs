using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WinScreenBehaviour : MonoBehaviour {
    [SerializeField] private GameObject _playerScoreContainer;

    [SerializeField] private Text _complimentText;
    [SerializeField] private GameObject _playerScorePrefab;
    [SerializeField] private Image _scoreImage;

    private bool _isMultiplayer;

    void Awake() {
        Assert.IsNotNull(_complimentText);
        Assert.IsNotNull(_playerScorePrefab);
        Assert.IsNotNull(_scoreImage);
        GlobalData.SceneDataLoader.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(GameInfo gameInfo)
    {
        GlobalData.SceneDataLoader.OnSceneLoaded -= OnSceneLoaded;
        _isMultiplayer = gameInfo.IsMultiplayer;
        EventManager.OnPlayersScoreDetermined += ShowWinScreen;
        this.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;
    }

    private void ShowWinScreen(Dictionary<TGEPlayer, int> playerInfo, int totalScore)
    {
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;
        this.gameObject.SetActive(true);

        foreach (var playerWithScore in playerInfo) {
            var playerScoreObject = Instantiate(_playerScorePrefab, _playerScoreContainer.transform);
            var playerScoreUiBehaviour = playerScoreObject.GetComponent<PlayerScoreUIBehaviour>();
            Assert.IsNotNull(playerScoreUiBehaviour);
            playerScoreUiBehaviour.ShowPlayerScore(playerWithScore.Key, playerWithScore.Value, _isMultiplayer);
        }

        //HOTFIX
        Canvas.ForceUpdateCanvases();

        _complimentText.text = "TODO: Hier komt een compliment".ToUpper();
    }
}