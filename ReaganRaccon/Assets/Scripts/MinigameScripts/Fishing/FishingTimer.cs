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
        timer = surviveTimeLimit;
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
        mainText.text = "Time: " + Mathf.RoundToInt(timer);
        if(timer <= 0 )
        {
            
            loadNext.switchScene("FishingMinigame","switchback");
        }
    }
}
