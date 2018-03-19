using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomListView : MonoBehaviour
{
    private List<GameObject> buttons = new List<GameObject>();
    public GameObject buttonPrefab;
    public GameObject panel;
    public Vector3 margin;

    public void UpdateListView(List<RoomInfo> rooms)
    {
        buttons.Clear();

        for(int i = 0; i < rooms.Count; i++)
        {
            GameObject button;
            if(i == 0)
                button = GameObject.Instantiate(buttonPrefab, panel.transform.position + margin, Quaternion.identity);
            else           
                button = GameObject.Instantiate(buttonPrefab, buttons[i - 1].transform.position = margin, Quaternion.identity);
        }
    }

    private void UpdateUI()
    {
        
    }
}
