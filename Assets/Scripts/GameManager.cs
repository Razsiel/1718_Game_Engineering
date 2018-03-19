using UnityEngine;

class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Player playerA;
    public Player playerB;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
   
    public static GameManager GetInstance()
    {
        return instance;
    }
}
