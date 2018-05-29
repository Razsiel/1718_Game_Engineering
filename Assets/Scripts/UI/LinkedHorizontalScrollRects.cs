using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedHorizontalScrollRects : MonoBehaviour
{

    public GameObject SequenceBar;
    private float _lastValue;

    public void OnOtherScrollValueChanged(float value)
    {
        RectTransform rectTransform = SequenceBar.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        float newValue = rectTransform.localPosition.x;
        print(newValue);
    }
}
