using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RandomSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dirtPrefab;
    public Rigidbody2D prize;
    public Rigidbody2D rb;
    public int amount;
    // Update is called once per frame
    private void Start() {
        rb = GetComponent<Rigidbody2D>();

         for(int i = 0; i < amount; i++){
            Vector2 randomSpawnPosition = new Vector2(rb.transform.position.x + Random.Range(0,4), rb.transform.position.y + Random.Range(0,4));
            Instantiate(dirtPrefab, randomSpawnPosition, Quaternion.identity);
        }    
        Vector3 SpawnPrize = new Vector3(Random.Range(-8,8), Random.Range(-4,4), 0);
        prize.transform.position += SpawnPrize;

    }
 
}
