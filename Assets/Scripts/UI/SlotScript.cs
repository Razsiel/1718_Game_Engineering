using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

public class SlotScript : MonoBehaviour
{
    //private SequenceBar _sequenceBar;
    public List<int> indices;

    void Awake()
    {
        indices = new List<int>();
    }

    void Start()
    {
        //_sequenceBar = this.transform.parent.gameObject.GetComponent<SequenceBar>();
    }

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
    

    //public void OnDrop(PointerEventData eventData)
    //{
    //    if (!Item)
    //    {
    //        _sequenceBar.HasChanged(int.Parse(name), DragHandler.ItemBeingDragged);
    //    }
    //    else
    //    {
    //        _sequenceBar.HasChangedInsert(int.Parse(name), DragHandler.ItemBeingDragged);
    //    }
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //if (DragHandler.ItemBeingDragged != null)
    //    //{
    //    //    print("erin");
    //    //    _sequenceBar.ShowDropInPoint(int.Parse(name));
    //    //}
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    //if (DragHandler.ItemBeingDragged != null)
    //    //{
    //    //    print("eruit");
    //    //    _sequenceBar.UnShowDropInPoint(int.Parse(name));
    //    //}
    //}
}