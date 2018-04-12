using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotScript : MonoBehaviour, IDropHandler
{

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.gameObject;
            }
            return null;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragHandler.item.transform.SetParent(transform);
        }
    }
}