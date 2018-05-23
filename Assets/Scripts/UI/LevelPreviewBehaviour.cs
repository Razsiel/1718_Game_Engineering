﻿using Assets.Data.Levels;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelPreviewBehaviour : MonoBehaviour {

    [Range(0, 1)]
    [SerializeField] private float _nonSelectedScale;
    [SerializeField] private Color _nonSelectedColor = Color.gray;

    [SerializeField] private Text _levelTitle;
    [SerializeField] private Image _levelPreview;
    [SerializeField] private GameObject _contentContainer;

    private Tweener _currentAnimation;

    public void Init(int chapter, int levelNumber, LevelData levelData) {
        Assert.IsNotNull(_levelTitle);
        Assert.IsNotNull(_levelPreview);
        Assert.IsNotNull(levelData);

        _levelTitle.text = $"{chapter}-{levelNumber}";
        _levelPreview.sprite = levelData.PreviewImage ?? Resources.Load<Sprite>("logo");
    }

    public void ChangeSelectedState(bool selected) {
        _levelPreview.color = selected ? Color.white : _nonSelectedColor;
        _currentAnimation?.Complete();
        _currentAnimation = _contentContainer.transform.DOScale(selected ? Vector3.one : Vector3.one * _nonSelectedScale, 0.5f);
    }
}