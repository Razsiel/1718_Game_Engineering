using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotScript : MonoBehaviour, IDropHandler
{
    //public SequenceBar SequenceBar;
    public GameObject Item
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
        if (!Item)
        {
            DragHandler.ItemBeingDragged.transform.SetParent(transform);
        }
    }

    public void OnSlotClicked()
    {
        //if (item)
        //{
        //    Destroy(transform.GetChild(0).gameObject);
        //    int a = SequenceBar.GetNextEmptySlotIndex();
        //    print(a);
        //}
    }
}