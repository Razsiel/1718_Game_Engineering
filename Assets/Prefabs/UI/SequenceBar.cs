using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBar : MonoBehaviour {

    public GameObject slots;
    public int amountOfCommandsAllowed;
    public GameObject[] commandSlots;
    public GameObject moveCommand;
    public GameObject turnLeftCommand;
    public GameObject turnRightCommand;
    public GameObject interactCommand;
    public GameObject waitCommand;

    void Awake()
    {
        EventManager.InitializeUi += Initialize;
    }

    void Initialize() {

        commandSlots = new GameObject[amountOfCommandsAllowed];
        for (int i = 0; i < amountOfCommandsAllowed; i++)
        {
            GameObject slot = (GameObject)Instantiate(slots);
            commandSlots[i] = slot;
            slot.transform.SetParent(this.gameObject.transform, false);
        }
        print(commandSlots.Length);
	}

    public int GetNextEmptySlotIndex()
    {
        for (int i = 0; i < amountOfCommandsAllowed; i++)
        {
            if(commandSlots[i].transform.childCount < 1)
            {
                return i;
            }
        }
        //slaat nergens op
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
