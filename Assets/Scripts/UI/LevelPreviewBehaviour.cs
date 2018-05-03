using Assets.Data.Levels;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelPreviewBehaviour : MonoBehaviour {

    public Text LevelTitle;
    public Image LevelPreview;

    public void Init(int chapter, int levelNumber, LevelData levelData) {
        Assert.IsNotNull(LevelTitle);
        Assert.IsNotNull(LevelPreview);
        Assert.IsNotNull(levelData);

        LevelTitle.text = $"{chapter}-{levelNumber}";
        LevelPreview.sprite = levelData.PreviewImage ?? Resources.Load<Sprite>("logo");
    }
}