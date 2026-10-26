using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject Root;
    private string ReganStatus = "NAN";  

     void Awake() { //finishes Singleton creation
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public GameObject getRoot()
    {
        return Root;
    }

    public void setRootFalse()
    {
        Root.SetActive(false);
    }
    public void setRootTrue()
    {
        Root.SetActive(true);
    }
    public void setVictoryStatus(string status)
    {
        //REGAN STATUS SHOULD BE SET TO NAN WHEN REGAN AFTER THE INTERACTION IS DONE
        //ELSE REGAN REGAN STATUS CAN BE EITHER WIN OR LOSE
        ReganStatus = status;
    }
    
    public string getVictoryStatus()
    {
        return ReganStatus;
    }
    // Update is called once per frame

}
