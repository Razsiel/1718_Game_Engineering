using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeingDragged;

    Transform _startParent;
    Vector3 _startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeingDragged = gameObject;

        _startPosition = transform.position;
        _startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //item.GetComponent<LayoutElement>().ignoreLayout = true;
        //item.transform.SetParent(item.transform.parent.parent);
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeingDragged = null;
        if (transform.parent == _startParent)
        {
            transform.position = _startPosition;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //item.GetComponent<LayoutElement>().ignoreLayout = false;
    }

}
