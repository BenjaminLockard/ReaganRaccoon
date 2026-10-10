using UnityEngine;

public class Goal : MonoBehaviour
{
    private LoadNext loadNext;
    public string name;
    void Start(){
        loadNext = GetComponent<LoadNext>();
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            loadNext.switchScene("switchback", name);
        }
    }
}
