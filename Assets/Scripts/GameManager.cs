using UnityEngine;

class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Player playerA;
    public Player playerB;
    public CommandLibrary commandLibrary;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        PhotonManager.Instance.RoomManager.UpdateGUI();
    }
   
    public static GameManager GetInstance()
    {
        return instance;
    }
}
