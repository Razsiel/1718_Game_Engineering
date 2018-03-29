using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Assertions;

public class PhotonManager : Photon.PunBehaviour
{
    //Our singleton instance of the Photonmanager
    public static PhotonManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
  
    //Backing field of our singleton instance
    private static PhotonManager instance;
   
    public RoomManager RoomManager
    {
        get { return roomManager; }
        set { roomManager = value; }
    }
    private RoomManager roomManager;
    //#endregion

    public event UnityAction TGEOnJoinedLobby;
    public event UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
    public event UnityAction<object[]> TGEOnJoinRandomRoomFailed;
    public event UnityAction<object[]> TGEOnJoinRoomFailed;
    public event UnityAction TGEOnCreatedRoom;
    public event UnityAction TGEOnJoinedRoom;
    
    void Awake()
    {
        Instance = this;
        print("In awake of PhotonManager");
        RoomManager = gameObject.GetComponent<RoomManager>();    
        Assert.IsNotNull(roomManager);
    }

    private PhotonManager()
    {
        
    }

    public void CreateRoom(string roomName)
    {
        print("Creating Room!");
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        print("Joining Room!");
        PhotonNetwork.JoinRoom(roomName);
    }

    //public void FireEvent(Guid instanceId, string handler)
    //{

    //    // Note: this is being fired from a method with in the same class that defined the event (i.e. "this").
    //    EventArgs e = new EventArgs();

    //    MulticastDelegate eventDelagate =
    //          (MulticastDelegate)this.GetType().GetField(handler,
    //           System.Reflection.BindingFlags.Instance |
    //           System.Reflection.BindingFlags.NonPublic).GetValue(this);

    //    Delegate[] delegates = eventDelagate.GetInvocationList();

    //    foreach(Delegate dlg in delegates)
    //    {
    //        dlg.Method.Invoke(dlg.Target, new object[] { this, e });
    //    }
    //}

    #region PhotonCallbacks
    public override void OnJoinedLobby()
    {
        Debug.Log("InPUNCAll");
        TGEOnJoinedLobby.Invoke();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        print("InPhotonPlayerConnected");
        TGEOnPhotonPlayerConnected.Invoke(newPlayer);
    }

    public override void OnConnectedToPhoton()
    {
        Debug.Log("Connected");
    }

    public override void OnCreatedRoom()
    {
        if (TGEOnCreatedRoom != null)
        {
            TGEOnCreatedRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        if (TGEOnJoinedRoom != null)
        {
            TGEOnJoinedRoom();
        }

        if (TGEOnPhotonPlayerConnected != null)
        {
            TGEOnPhotonPlayerConnected(PhotonNetwork.player);
        }
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        TGEOnJoinRoomFailed(codeAndMsg);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        TGEOnJoinRandomRoomFailed(codeAndMsg);
    }


    #endregion
}

#region Deprecated
//public class PhotonCallbackReceiver : MonoBehaviour
//{

//    private void FireEvent(string eventName)
//    {
//        PhotonManager.Instance.FireEvent(new Guid(), eventName);
//    }

//    void OnJoinedLobby()
//    {
//        Debug.Log("InPUNCAll");
//        //TGEOnJoinedLobby.Invoke();
//        FireEvent(nameof(PhotonManager.Instance.TGEOnJoinedLobby));
//    }

//    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
//    {
//        //TGEOnPhotonPlayerConnected.Invoke(newPlayer);
//    }

//    void OnConnectedToPhoton()
//    {
//        Debug.Log("Connected");
//    }
//}
#endregion