using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Reflection;

public class PhotonManager : Photon.PunBehaviour
{
    //Our singleton instance of the Photonmanager
    public static PhotonManager Instance
    {
        get { if(instance == null) { instance = new PhotonManager(); } return instance; }
        private set { instance = value; }
    }

    //Backing field of our singleton instance
    private static PhotonManager instance;

    #region RoomManager SingleTon
    public RoomManager RoomManager
    {
        get { if(roomManager == null) roomManager = new RoomManager(); return roomManager; }
        private set { roomManager = value; }
    }
    public RoomManager roomManager;
    #endregion

    public event UnityAction TGEOnJoinedLobby;
    public event UnityAction<PhotonPlayer> TGEOnPhotonPlayerConnected;
    public event UnityAction<object[]> TGEOnJoinLobbyFailed;
   
    private PhotonManager() { }
 
    public void FireEvent(Guid instanceId, string handler)
    {

        // Note: this is being fired from a method with in the same class that defined the event (i.e. "this").
        EventArgs e = new EventArgs();

        MulticastDelegate eventDelagate =
              (MulticastDelegate)this.GetType().GetField(handler,
               System.Reflection.BindingFlags.Instance |
               System.Reflection.BindingFlags.NonPublic).GetValue(this);

        Delegate[] delegates = eventDelagate.GetInvocationList();

        foreach(Delegate dlg in delegates)
        {
            dlg.Method.Invoke(dlg.Target, new object[] { this, e });
        }
    }

    #region PhotonCallbacks
    public override void OnJoinedLobby()
    {
        Debug.Log("InPUNCAll");
        TGEOnJoinedLobby.Invoke();
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        TGEOnPhotonPlayerConnected.Invoke(newPlayer);
    }

    public override void OnConnectedToPhoton()
    {
        Debug.Log("Connected");
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