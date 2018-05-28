using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundBehaviour : MonoBehaviour {

    void Awake() {
        EventManager.OnLoadLevel += OnLoadLevel;
    }

    private void OnLoadLevel(GameInfo gameInfo) {
        EventManager.OnLoadLevel -= OnLoadLevel;
        Rect rect = new Rect(0, 0, 1920, 1080);
        var imageComponent = GetComponent<Image>();
        imageComponent.sprite = Sprite.Create(gameInfo.Level.BackgroundImage, rect, Vector2.zero);
    }
}
