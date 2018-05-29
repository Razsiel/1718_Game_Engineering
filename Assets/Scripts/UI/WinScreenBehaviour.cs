using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanelSelector;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _player1Panel;
    [SerializeField] private GameObject _player2Panel;
    [SerializeField] private GameObject _textAtTop;
    [SerializeField] private GameObject _totalStars;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _player1Name;
    [SerializeField] private GameObject _player1Image;
    [SerializeField] private GameObject _player1StarsImage;
    [SerializeField] private GameObject _player2Name;
    [SerializeField] private GameObject _player2Image;
    [SerializeField] private GameObject _player2StarsImage;
    [SerializeField] private Sprite _oneStarSprite;
    [SerializeField] private Sprite _twoStarsSprite;
    [SerializeField] private Sprite _threeStarSprite;


    private GameObject mainPanelSelector;
    private GameObject mainPanel;
    private GameObject player1Panel;
    private GameObject player2Panel;
    private GameObject textAtTop;
    private GameObject totalStars;
    private GameObject continueButton;
    private GameObject player1Name;
    private GameObject player1Image;
    private GameObject player1StarsImage;
    private GameObject player2Name;
    private GameObject player2Image;
    private GameObject player2StarsImage;
    private bool _isMultiplayer;


    public void Initialize(bool isMultiplayer)
    {
        _isMultiplayer = isMultiplayer;
        EventManager.OnPlayersScoreDetermined += ShowWinScreen;
        mainPanelSelector = Instantiate(_mainPanelSelector, transform, false);
        mainPanel = Instantiate(_mainPanel, mainPanelSelector.transform, false);

        player1Panel = Instantiate(_player1Panel, mainPanel.transform, false);
        if(!isMultiplayer)
        {
            player1Panel.GetComponent<RectTransform>().offsetMin = new Vector2(600, 200);
            player1Panel.GetComponent<RectTransform>().offsetMax = new Vector2(-600, -200);
        }

        textAtTop = Instantiate(_textAtTop, mainPanel.transform, false);
        totalStars = Instantiate(_totalStars, mainPanel.transform, false);
        continueButton = Instantiate(_continueButton, mainPanel.transform, false);

        player1Name = Instantiate(_player1Name, player1Panel.transform, false);
        player1Image = Instantiate(_player1Image, player1Panel.transform, false);
        player1StarsImage = Instantiate(_player1StarsImage, player1Panel.transform, false);

        if(isMultiplayer)
        {
            player2Panel = Instantiate(_player2Panel, mainPanel.transform, false);
            player2Name = Instantiate(_player2Name, player2Panel.transform, false);
            player2Image = Instantiate(_player2Image, player2Panel.transform, false);
            player2StarsImage = Instantiate(_player2StarsImage, player2Panel.transform, false);
        }


        continueButton.GetComponent<Button>().onClick.AddListener(ContinueButtonClicked);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;
    }

    private void ShowWinScreen(Dictionary<TGEPlayer, int> playerInfo, int totalScore)
    {
        EventManager.OnPlayersScoreDetermined -= ShowWinScreen;

        textAtTop.GetComponent<Text>().text = "HIER KOMT EEN COMPLIMENT";

        if(_isMultiplayer)
        {
            var master = playerInfo.Single(x => x.Key.photonPlayer.IsMasterClient);

            player1Name.GetComponent<Text>().text = master.Key.photonPlayer.NickName;
            player1Image.GetComponent<Image>().sprite = null;

            player1StarsImage.GetComponent<Image>().sprite =
                master.Value == 1 ? _oneStarSprite
                : master.Value == 2 ? _twoStarsSprite
                : _threeStarSprite;

            var client = playerInfo.Single(x => !x.Key.photonPlayer.IsMasterClient);
            player2Name.GetComponent<Text>().text = client.Key.photonPlayer.NickName;
            player2Image.GetComponent<Image>().sprite = null;
            player2StarsImage.GetComponent<Image>().sprite =
                client.Value == 1 ? _oneStarSprite :
                client.Value == 2 ? _twoStarsSprite :
                _threeStarSprite;
        }
        else
        {
            var singlePlayer = playerInfo.First();
            player1Name.GetComponent<Text>().text = "Player";
            player1Image.GetComponent<Image>().sprite = null;
            player1StarsImage.GetComponent<Image>().sprite =
                singlePlayer.Value == 1 ? _oneStarSprite :
                singlePlayer.Value == 2 ? _twoStarsSprite :
                _threeStarSprite;
        }

        switch(totalScore)
        {
            case 1:
                totalStars.GetComponent<Image>().sprite = _oneStarSprite;
                break;
            case 2:
                totalStars.GetComponent<Image>().sprite = _twoStarsSprite;
                break;
            default:
                totalStars.GetComponent<Image>().sprite = _threeStarSprite;
                break;
        }

        gameObject.SetActive(true);
    }

    public void OnClickContinue()
    {
        // Restart level? To level Select?
        print("Level Completed!");
        gameObject.SetActive(false);
    }

    public void ContinueButtonClicked()
    {
        gameObject.SetActive(false);
        EventManager.WinScreenContinueClicked();
    }
}
