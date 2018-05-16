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


    public void Initialize()
    {
        EventManager.OnAllLevelGoalsReached += ShowWinScreen;
        GameObject mainPanel = Instantiate(_mainPanel, transform, false);
        GameObject player1Panel = Instantiate(_player1Panel, mainPanel.transform, false);
        GameObject player2Panel = Instantiate(_player2Panel, mainPanel.transform, false);
        Instantiate(_textAtTop, mainPanel.transform, false);
        Instantiate(_totalStars, mainPanel.transform, false);
        GameObject continueButton = Instantiate(_continueButton, mainPanel.transform, false);

        Instantiate(_player1Name, player1Panel.transform, false);
        Instantiate(_player1Image, player1Panel.transform, false);
        Instantiate(_player1StarsImage, player1Panel.transform, false);

        Instantiate(_player2Name, player2Panel.transform, false);
        Instantiate(_player2Image, player2Panel.transform, false);
        Instantiate(_player2StarsImage, player2Panel.transform, false);

        continueButton.GetComponent<Button>().onClick.AddListener(ContinueButtonClicked);
        gameObject.SetActive(false);
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
