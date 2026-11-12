using UnityEngine;

public class FishManager : MonoBehaviour

{   public GameObject spawnObject;
    [SerializeField] private GameObject _containerTypeSpawnObject;
    public Rigidbody2D rb;
    public float timer;
    private float speedMultiplier;
    public float timeBetweenSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speedMultiplier += Time.deltaTime * 0.1f;
        timer += Time.deltaTime;
        
        if(timer > timeBetweenSpawn)
        {
            Vector2 randomSpawnPosition = new Vector2(rb.transform.position.x, rb.transform.position.y + Random.Range(-8.0f,7.8f));
            GameObject newBullet = Instantiate(spawnObject, randomSpawnPosition, Quaternion.identity);
            newBullet.transform.parent = _containerTypeSpawnObject.transform; //makes the new spawn the child of the container
            timer = 0;
        }

    }
}
