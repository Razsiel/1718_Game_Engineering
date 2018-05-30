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

    private ScrollRect _scrollRect;

    public void Initialize(uint highestScore, uint decentScore, float commandSize)
    {
        _threeStarPanel = Instantiate(StarPanel);
        _twoStarPanel = Instantiate(StarPanel);
        _oneStarPanel = Instantiate(StarPanel);
        _starsScrollRect = Instantiate(StarsScrollRect);
        _starsScrollContent = Instantiate(StarsScrollContent);

        _starsScrollRect.transform.SetParent(transform, false);
        _starsScrollRect.transform.SetSiblingIndex(1);
        _starsScrollContent.transform.SetParent(_starsScrollRect.transform, false);

        _scrollRect = _starsScrollRect.GetComponent<ScrollRect>();

        _scrollRect.content = _starsScrollContent.GetComponent<RectTransform>();

        Image threeStarImage = _threeStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image twoStarImage = _twoStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image oneStarImage = _oneStarPanel.transform.GetChild(0).GetComponent<Image>();

        threeStarImage.sprite = ThreeStarsSprite;
        twoStarImage.sprite = TwoStarsSprite;
        oneStarImage.sprite = OneStarSprite;

        _threeStarPanel.transform.SetParent(_starsScrollContent.transform, false);
        _twoStarPanel.transform.SetParent(_starsScrollContent.transform, false);
        _oneStarPanel.transform.SetParent(_starsScrollContent.transform, false);

        _threeStarPanel.GetComponent<LayoutElement>().preferredWidth = highestScore * commandSize;
        _twoStarPanel.GetComponent<LayoutElement>().preferredWidth = decentScore * commandSize;
        _oneStarPanel.GetComponent<LayoutElement>().preferredWidth = 50;


        _startingPosition = new Vector2(1000f, -25f);
        _starsScrollContent.GetComponent<RectTransform>().localPosition = _startingPosition;
    }

    public void OnSequenceScroll(float value)
    {
        Vector2 _newPosition = new Vector2((_startingPosition.x + value), _startingPosition.y);
        print(_newPosition.x);

        _starsScrollContent.GetComponent<RectTransform>().localPosition = _newPosition;
    }
}
