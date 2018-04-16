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

        int instanceId = gameObject.GetInstanceID();
        print(instanceId);

        ItemBeingDragged = gameObject;
        ItemBeingDragged.GetComponent<Button>().enabled = false;

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
            replacement = Instantiate(gameObject, _startParent);
            replacement.GetComponent<Button>().enabled = true;
            replacement.GetComponent<DragHandler>().enabled = true;
            print("replacement: " + replacement.GetInstanceID());
        }   
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //item.GetComponent<LayoutElement>().ignoreLayout = false;
    }

}
