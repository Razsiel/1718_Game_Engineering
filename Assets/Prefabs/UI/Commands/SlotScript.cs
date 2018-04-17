using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SlotScript : MonoBehaviour, IDropHandler
{
    private SequenceBar _sequenceBar;

    void Start()
    {
        _sequenceBar = this.transform.parent.gameObject.GetComponent<SequenceBar>();
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
    

    public void OnDrop(PointerEventData eventData)
    {
        if (!Item)
        {
            _sequenceBar.HasChanged(int.Parse(name), DragHandler.ItemBeingDragged);
        }
        else
        {
            _sequenceBar.HasChangedInsert(int.Parse(name), DragHandler.ItemBeingDragged);
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