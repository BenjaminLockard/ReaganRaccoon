using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject Root;
    private string ReganStatus = "NAN";  
    private int counter = 0;
    private string[] levelNames =
     {"BaseBallMinigame","ClawMinigame","FishingMinigame",
    "PlatformingMinigame 1","PlatformingMinigame 2"
    ,"PlatformingMinigame 3","MashingMinigame"};
     void Awake() { //finishes Singleton creation
        Instance = this;
        Shuffle(levelNames);
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

    public string getLeveName()
    {
        string temp = levelNames[counter];
        if(counter >= levelNames.Length - 1)// 6
        {  
            //Debug.Log("SHUFFTING");
            Shuffle(levelNames);
            counter = 0;
        }
        else
        {
            counter++;
        }
        //Debug.Log("Counter is at: " + counter);
        return temp;
    }
    
    void Shuffle (string[] deck) {
    for (int i = 0; i < deck.Length; i++) {
        string temp = deck[i];
        int randomIndex = Random.Range(i, deck.Length);
        deck[i] = deck[randomIndex];
        deck[randomIndex] = temp;
    }
}
    // Update is called once per frame

}
