using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject Root;
    
     void Awake() { //finishes Singleton creation
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(Root);
    }

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
    // Update is called once per frame

}
