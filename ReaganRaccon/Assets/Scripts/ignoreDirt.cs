using UnityEngine;

public class ignoreDirt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Dirt"))
        {
             Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
}
}
