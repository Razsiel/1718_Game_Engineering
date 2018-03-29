using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBar : MonoBehaviour {

    public GameObject slots;
    public int amountOfCommandsAllowed;
    public GameObject[] commandSlots;
    public Sprite moveCommand;
    public Sprite turnLeftCommand;
    public Sprite turnRightCommand;
    public Sprite interactCommand;
    public Sprite waitCommand;
    float xMin = 0f;
    float xMax = 0.05f;

    // Use this for initialization
    void Start () {

        commandSlots = new GameObject[amountOfCommandsAllowed];
        for (int i = 0; i < amountOfCommandsAllowed; i++)
        {
            GameObject slot = (GameObject)Instantiate(slots);
            commandSlots[i] = slot;
            slot.transform.SetParent(this.gameObject.transform, false);

            slot.GetComponent<RectTransform>().anchorMin = new Vector3(xMin, 0, 0);
            slot.GetComponent<RectTransform>().anchorMax = new Vector3(xMax, 1, 0);

                //Image slotImage = slot.transform.GetChild(0).GetComponent<Image>();

                //slotImage.sprite = asd;

            xMin += 0.05f;
            xMax += 0.05f;
        }
	}

    public int GetNextEmptySlotIndex()
    {
        for (int i = 0; i < amountOfCommandsAllowed; i++)
        {
            if(commandSlots[i].transform.GetChild(0).GetComponent<Image>().sprite == null)
            {
                return i;
            }
        }
        return 100;
    }

    public void ClearImages()
    {
        for (int i = 0; i < amountOfCommandsAllowed; i++)
        {
            commandSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
