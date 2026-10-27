using UnityEngine;
using TMPro;
public class FishingTimer : MonoBehaviour

{
    public float timer;
    public float surviveTimeLimit;
    private LoadNext loadNext;
    public TextMeshProUGUI mainText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadNext = GetComponent<LoadNext>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        mainText.text = "Time: " + timer;
        if(timer > surviveTimeLimit)
        {
            
            loadNext.switchScene("FishingMinigame","switchback");
        }
    }
}
