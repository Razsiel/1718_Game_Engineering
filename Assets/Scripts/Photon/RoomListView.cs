using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Photon;

public class RoomListView : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject panel;
    public Vector3 margin;
    public SimpleObjectPool buttonObjectPool;

    public void Start()
    {
        buttonObjectPool = new SimpleObjectPool();
        UpdateListView(null);
    }

    public void UpdateListView(List<RoomInfo> rooms)
    {
        
        AddButtons();
        //for(int i = 0; i < 50/*rooms.Count*/; i++)
        //{
        //    GameObject button;
        //    if(i == 0)
        //        button = GameObject.Instantiate(buttonPrefab, panel.transform.position + margin, Quaternion.identity);
        //    else           
        //        button = GameObject.Instantiate(buttonPrefab, (buttons[i - 1] as GameObject).transform.position = margin, Quaternion.identity);
        //}
    }

    private void AddButtons()
    {
        //Add button foreach room
        for(int i = 0; i < 50; i++)
        {
            //Room room = rooms[i];
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(panel.transform);

            RoomListButton button = newButton.GetComponent<RoomListButton>();
            button.Setup(i, this);
        }
    }

    private void RemoveButtons()
    {
        while(panel.transform.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    void AddRoom(RoomInfo roomToAdd)
    {
        PhotonManager.Instance.RoomManager.AddRoom(roomToAdd);
    }

    void CloseRoom(RoomInfo roomToClose)
    {
        PhotonManager.Instance.RoomManager.CloseRoom(roomToClose);
    }

    public void HandleClick(int roomNumber)
    {
        Debug.Log("HandleClick: RoomListView");
    }

    private void UpdateUI()
    {
        
    }
}
