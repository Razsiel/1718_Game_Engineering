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

        if (totalScore == 3)
        {
            List<string> threeStarCompliments = new List<string>()
            {
                LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_1"),
                LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_2"),
                LanguageManager.Instance.GetTextValue("COMPLIMENT_THREE_STARS_3")

            };

            _complimentText.text = GetRandomfromList(threeStarCompliments);
        }
        else if (totalScore == 2)
        {
            List<string> twoStarCompliments = new List<string>()
            {
                LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_1"),
                LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_2"),
                LanguageManager.Instance.GetTextValue("COMPLIMENT_TWO_STARS_3")

            };
            _complimentText.text = GetRandomfromList(twoStarCompliments);
        }
        else
        {
            List<string> oneStarCompliments = new List<string>()
            {
                LanguageManager.Instance.GetTextValue("COMPLIMENT_ONE_STAR_1"),
                LanguageManager.Instance.GetTextValue("COMPLIMENT_ONE_STAR_2")
            };
            _complimentText.text = GetRandomfromList(oneStarCompliments);
        }
    }

    private string GetRandomfromList(List<string> listOfStrings)
    {
        return listOfStrings[Random.Range(0, listOfStrings.Count)];
    }
}