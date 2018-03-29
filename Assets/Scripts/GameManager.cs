using UnityEngine;

class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Player playerA;
    public Player playerB;
    public CommandLibrary commandLibrary;

    void Awake()
    {
        Debug.Log("In gamemanager awake");       
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        Debug.Log("In start, updating GUI");
        print("gonna call this: " + PhotonManager.Instance.RoomManager);
        //PhotonManager.Instance.RoomManager.UpdateGUI();
    }

    public void StartMultiplayerGame()
    {
        //Lets do some GameStarting logic here
    }
   
    public static GameManager GetInstance()
    {
        return instance;
    }
}
