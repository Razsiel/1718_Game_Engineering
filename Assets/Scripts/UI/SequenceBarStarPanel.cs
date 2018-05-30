using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBarStarPanel : MonoBehaviour
{

    public GameObject starPanel;
    public Sprite ThreeStarsSprite;
    public Sprite TwoStarsSprite;
    public Sprite OneStarSprite;

    private GameObject _threeStarPanel;
    private GameObject _twoStarPanel;
    private GameObject _oneStarPanel;

    public void Initialize(uint highestScore, uint decentScore)
    {
        _threeStarPanel = _twoStarPanel = _oneStarPanel = Instantiate(starPanel);

        Image threeStarImage = _threeStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image twoStarImage = _twoStarPanel.transform.GetChild(0).GetComponent<Image>();
        Image oneStarImage = _oneStarPanel.transform.GetChild(0).GetComponent<Image>();

        threeStarImage.sprite = ThreeStarsSprite;
        twoStarImage.sprite = TwoStarsSprite;
        oneStarImage.sprite = OneStarSprite;

        _threeStarPanel.transform.SetParent(transform.GetChild(1).GetChild(0));
        _twoStarPanel.transform.SetParent(transform.GetChild(1).GetChild(0));
        _oneStarPanel.transform.SetParent(transform.GetChild(1).GetChild(0));

        _threeStarPanel.GetComponent<LayoutElement>().preferredWidth = highestScore * 
    }
}
