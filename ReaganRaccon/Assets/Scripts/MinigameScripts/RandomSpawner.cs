using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RandomSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dirtPrefab;
    public Rigidbody2D rb;
    // Update is called once per frame
    private void Start() {
        rb = GetComponent<Rigidbody2D>();

         for(int i = 0; i < Random.Range(5,7); i++){
            Vector2 randomSpawnPosition = new Vector2(rb.transform.position.x + Random.Range(0,2), rb.transform.position.y + Random.Range(0,2));
            Instantiate(dirtPrefab, randomSpawnPosition, Quaternion.identity);
        }    
    }
 
}
