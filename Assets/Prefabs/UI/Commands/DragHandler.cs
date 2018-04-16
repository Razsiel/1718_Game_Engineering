using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Prefabs.UI;
using Assets.Scripts;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ItemBeingDragged;

    Transform _startParent;
    Vector3 _startPosition;
    private GameObject _replacement;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _startParent = transform.parent;

        ItemBeingDragged = gameObject;
        ItemBeingDragged.GetComponent<Button>().enabled = false;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    bool IsPointerOverGameObject(int fingerId)
    {
        EventSystem eventSystem = EventSystem.current;
        return (eventSystem.IsPointerOverGameObject(fingerId)
                && eventSystem.currentSelectedGameObject.GetComponent<SlotScript>() != null);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        if (IsPointerOverGameObject(eventData.pointerId))
        {
            print("a");
        }
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
            _replacement = Instantiate(gameObject, _startParent);
            _replacement.GetComponent<CanvasGroup>().blocksRaycasts = true;
            _replacement.GetComponent<Button>().enabled = true;
        }   
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //item.GetComponent<LayoutElement>().ignoreLayout = false;
    }

}
