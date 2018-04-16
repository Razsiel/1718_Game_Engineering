using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundBehaviour : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
        Rect rect = new Rect(0, 0, 1920, 1080);
        var imageComponent = GetComponent<Image>();
        imageComponent.sprite = Sprite.Create(GameManager.GetInstance().LevelData.BackgroundImage, rect, Vector2.zero);
        imageComponent.color = Color.gray;
    }

}
