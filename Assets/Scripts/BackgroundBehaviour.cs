using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundBehaviour : MonoBehaviour {

    void Awake() {
        EventManager.OnLoadLevel += (level, players) => {
            Rect rect = new Rect(0, 0, 1920, 1080);
            var imageComponent = GetComponent<Image>();
            imageComponent.sprite = Sprite.Create(level.BackgroundImage, rect, Vector2.zero);
            imageComponent.color = Color.gray;
        };
    }

}
