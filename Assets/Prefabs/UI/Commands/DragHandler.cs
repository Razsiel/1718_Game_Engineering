using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeingDragged;

    Transform _startParent;
    Vector3 _startPosition;
    private GameObject replacement;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _startParent = transform.parent;

        ItemBeingDragged = gameObject;
        ItemBeingDragged.GetComponent<Button>().enabled = false;

        replacement = gameObject;

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
        else
        {
            replacement.transform.SetParent(_startParent, false);
            print("replacement parent: " + replacement.transform.parent);
            Instantiate(replacement);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;



        //item.GetComponent<LayoutElement>().ignoreLayout = false;
    }

}
