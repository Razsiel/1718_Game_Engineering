using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenBehaviour : MonoBehaviour
{
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

    public void Initialize()
    {
        EventManager.OnAllLevelGoalsReached += ShowWinScreen;
        mainPanel = Instantiate(_mainPanel, transform, false);
        player1Panel = Instantiate(_player1Panel, mainPanel.transform, false);
        player2Panel = Instantiate(_player2Panel, mainPanel.transform, false);
        textAtTop = Instantiate(_textAtTop, mainPanel.transform, false);
        totalStars = Instantiate(_totalStars, mainPanel.transform, false);
        continueButton = Instantiate(_continueButton, mainPanel.transform, false);

        player1Name = Instantiate(_player1Name, player1Panel.transform, false);
        player1Image = Instantiate(_player1Image, player1Panel.transform, false);
        player1StarsImage = Instantiate(_player1StarsImage, player1Panel.transform, false);

        player2Name = Instantiate(_player2Name, player2Panel.transform, false);
        player2Image = Instantiate(_player2Image, player2Panel.transform, false);
        player2StarsImage = Instantiate(_player2StarsImage, player2Panel.transform, false);

        continueButton.GetComponent<Button>().onClick.AddListener(ContinueButtonClicked);
        UpdateWinScreenValues();
        gameObject.SetActive(false);
    }

    public void UpdateWinScreenValues()
    {
        textAtTop.GetComponent<Text>().text = "HIER KOMT EEN COMPLIMENT";

        player1Name.GetComponent<Text>().text = "Player1 Naam";
        player1Image.GetComponent<Image>().sprite = null;
        player1StarsImage.GetComponent<Image>().sprite = null;

        player2Name.GetComponent<Text>().text = "Player2 Naam";
        player2Image.GetComponent<Image>().sprite = null;
        player2StarsImage.GetComponent<Image>().sprite = null;

        totalStars.GetComponent<Image>().sprite = null;

    }

    public void ShowWinScreen()
    {
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
