using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;

    Vector3 ShowPosition = new Vector3(0f, 0f, 0f);
    Vector3 HidePosition = new Vector3(0.0f, 2500.0f, 0.0f);

    void Start()
    {
        _rectTransform = gameObject.transform.parent.GetComponent<RectTransform>();
        EventManager.OnClickedMenu += ShowMenu;
    }

    // Show Hide slide
    public void ShowMenu()
    {
        _rectTransform.DOLocalMove(ShowPosition, 1f);
    }

    void HideMenu()
    {
        _rectTransform.DOLocalMove(HidePosition, 1f);
    }


    // Buttons
    public void OnClickBackToGame()
    {
        HideMenu();
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuitGame()
    {
#if (!UNITY_EDITOR)
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
}
