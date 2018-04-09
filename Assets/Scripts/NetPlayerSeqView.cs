using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayerSeqView : MonoBehaviour {

    public SequenceBar SequenceBar;
	
    public void UpdateSequenceBar(List<BaseCommand> commands)
    {
        print(commands);
        foreach(BaseCommand c in commands)
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            print("nextfreeslot: " + nextFreeSlot);
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            print(image);
            image.sprite = c.Icon;
        }
    }
}
