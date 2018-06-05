using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using SmartLocalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WinScreenBehaviour : MonoBehaviour {
    [SerializeField] private GameObject _playerScoreContainer;

    [SerializeField] private Text _complimentText;
    [SerializeField] private GameObject _playerScorePrefab;
    [SerializeField] private Image _scoreImage;
    [SerializeField] private Sprite[] _scoreImages;
    [SerializeField] private GameObject[] _objectToDisableMP;

    private bool _isMultiplayer;

    void Awake() {
        Assert.IsNotNull(_complimentText);
        Assert.IsNotNull(_playerScorePrefab);
        Assert.IsNotNull(_scoreImage);
        Assert.IsNotNull(_scoreImages);
        Assert.IsNotNull(_objectToDisableMP);
        GlobalData.SceneDataLoader.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(GameInfo gameInfo) {
        GlobalData.SceneDataLoader.OnSceneLoaded -= OnSceneLoaded;
        _isMultiplayer = gameInfo.IsMultiplayer;
        EventManager.OnPlayersScoreDetermined += ShowWinScreen;
        this.gameObject.SetActive(false);
    }

    void OnDestroy() {
        GlobalData.SceneDataLoader.OnSceneLoaded -= OnSceneLoaded;
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;
    }

    private void ShowWinScreen(Dictionary<TGEPlayer, int> playerInfo, int totalScore) {
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;
        this.gameObject.SetActive(true);

        foreach (var playerWithScore in playerInfo) {
            var playerScoreObject = Instantiate(_playerScorePrefab, _playerScoreContainer.transform);
            var playerScoreUiBehaviour = playerScoreObject.GetComponent<PlayerScoreUIBehaviour>();
            Assert.IsNotNull(playerScoreUiBehaviour);
            playerScoreUiBehaviour.ShowPlayerScore(playerWithScore.Key, playerWithScore.Value, _isMultiplayer);
        }

        if (_isMultiplayer) {
            var isMaster = PhotonNetwork.player.IsMasterClient;
            foreach (var go in _objectToDisableMP) {
                go.SetActive(isMaster);
            }
        }

        //HOTFIX
        Canvas.ForceUpdateCanvases();

        List<string> compliments;
        switch (totalScore) {
            case 3:
                compliments = new List<string>() {
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_1"),
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_2"),
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_3")
                };
                break;
            case 2:
                compliments = new List<string>() {
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_1"),
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_2"),
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_3")
                };
                break;
            default:
                compliments = new List<string>() {
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_ONE_STAR_1"),
                    LanguageManager.Instance.GetTextValue("COMPLIMENT_ONE_STAR_2")
                };
                break;
        }

        _complimentText.text = GetRandomfromList(compliments);
        _scoreImage.sprite = _scoreImages[totalScore - 1];
    }

    private string GetRandomfromList(List<string> listOfStrings) {
        return listOfStrings[Random.Range(0, listOfStrings.Count)];
    }
}