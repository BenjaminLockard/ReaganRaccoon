using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class MashingMinigame : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public float timesToMash = 20f; 
    private LoadNext loadNext;
    private bool finish;
    void Start(){
        loadNext = GetComponent<LoadNext>();
        
    }
    public void OnSpace(InputValue space){
        if(finish != true){
        Debug.Log("Press Space " + timesToMash);
        timesToMash--;
        }
        
        
    }
    void Update(){
        mainText.text = "Press Space " + timesToMash;
        if(timesToMash == 0f && finish != true){
            mainText.text = "FART";
            finish = true;
            loadNext.switchScene("switchback","MashingMinigame");
        }
    }    
}
