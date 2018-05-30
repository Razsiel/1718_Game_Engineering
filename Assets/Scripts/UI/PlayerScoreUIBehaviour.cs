using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using SmartLocalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerScoreUIBehaviour : MonoBehaviour {
    [SerializeField] private Text _playerText;
    [SerializeField] private Text _playerName;
    [SerializeField] private Image _playerImage;
    [SerializeField] private Image _playerScore;

    [SerializeField] private Color[] _colors;
    [SerializeField] private Sprite[] _playerSprites;
    [SerializeField] private Sprite[] _scoreSprites;

    void Awake() {
        Assert.IsNotNull(_playerText);
        Assert.IsNotNull(_playerName);
        Assert.IsNotNull(_playerImage);
        Assert.IsNotNull(_playerScore);
        Assert.IsNotNull(_colors);
        Assert.IsNotNull(_playerSprites);
    }

    public void ShowPlayerScore(TGEPlayer tgePlayer, int score, bool isMultiplayer) {
        var player = tgePlayer.Player;
        _playerText.text = $"{LanguageManager.Instance.GetTextValue("PLAYER")} {player.PlayerNumber + 1}";
        _playerText.color = _colors[player.PlayerNumber];
        _playerName.text = isMultiplayer ? tgePlayer.photonPlayer.NickName : _playerText.text;
        _playerImage.sprite = _playerSprites[player.PlayerNumber];
        _playerScore.sprite = _scoreSprites[score - 1];

        var randomRot = Random.Range(-5, 5);
        ((RectTransform) this.gameObject.transform).Rotate(Vector3.forward, randomRot);
    }
}