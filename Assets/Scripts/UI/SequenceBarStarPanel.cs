using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SequenceBarStarPanel : MonoBehaviour
{

    public GameObject StarPanel;
    public GameObject StarsScrollRect;
    public GameObject StarsScrollContent;
    public Sprite ThreeStarsSprite;
    public Sprite TwoStarsSprite;
    public Sprite OneStarSprite;

    private GameObject _threeStarPanel;
    private GameObject _twoStarPanel;
    private GameObject _oneStarPanel;
    private GameObject _starsScrollRect;
    private GameObject _starsScrollContent;
    private Vector2 _startingPosition;

    private float _startingWidthThreeStarPanel;
    private float _startingWidthTwoStarPanel;

    private ScrollRect _scrollRect;

    public void Initialize(uint highestScore, uint decentScore, float commandSize)
    {
        _starsScrollRect = Instantiate(StarsScrollRect, transform, false);
        _starsScrollContent = Instantiate(StarsScrollContent, _starsScrollRect.transform, false);
        _threeStarPanel = Instantiate(StarPanel, _starsScrollContent.transform, false);
        _twoStarPanel = Instantiate(StarPanel, _starsScrollContent.transform, false);
        _oneStarPanel = Instantiate(StarPanel, _starsScrollContent.transform, false);

        _starsScrollRect.transform.SetSiblingIndex(1);

        _scrollRect = _starsScrollRect.GetComponent<ScrollRect>();

        _scrollRect.content = _starsScrollContent.GetComponent<RectTransform>();

        Image threeStarImage = _threeStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image twoStarImage = _twoStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image oneStarImage = _oneStarPanel.transform.GetChild(0).GetComponent<Image>();
        _twoStarPanel.transform.GetChild(0).GetComponent<LayoutElement>().preferredWidth =
            2 * _twoStarPanel.GetComponent<LayoutElement>().preferredHeight;
        _oneStarPanel.transform.GetChild(0).GetComponent<LayoutElement>().preferredWidth =
            _oneStarPanel.GetComponent<LayoutElement>().preferredHeight;

        threeStarImage.sprite = ThreeStarsSprite;
        twoStarImage.sprite = TwoStarsSprite;
        oneStarImage.sprite = OneStarSprite;

        _startingWidthThreeStarPanel = highestScore * (commandSize + 5);
        _startingWidthTwoStarPanel = (decentScore - highestScore) * (commandSize + 5);
        _threeStarPanel.GetComponent<LayoutElement>().preferredWidth = _startingWidthThreeStarPanel;
        _twoStarPanel.GetComponent<LayoutElement>().preferredWidth = _startingWidthTwoStarPanel;
        _oneStarPanel.GetComponent<LayoutElement>().preferredWidth = 50;


        _startingPosition = new Vector2(1000f, -25f);
        _starsScrollContent.GetComponent<RectTransform>().anchoredPosition = _startingPosition;
    }

    public void OnSequenceScroll(float value)
    {
        Vector2 _newPosition = new Vector3((_startingPosition.x + value), _startingPosition.y);
        _starsScrollContent.GetComponent<RectTransform>().anchoredPosition = _newPosition;
    }

    public void UpdateStarPanelWidths(float highScoreExtraWidth, float decentScoreExtraWidth)
    {
        _threeStarPanel.GetComponent<LayoutElement>().preferredWidth = _startingWidthThreeStarPanel;
        _twoStarPanel.GetComponent<LayoutElement>().preferredWidth = _startingWidthTwoStarPanel;

        _threeStarPanel.GetComponent<LayoutElement>().preferredWidth += highScoreExtraWidth;
        _twoStarPanel.GetComponent<LayoutElement>().preferredWidth += decentScoreExtraWidth;
    }
}
