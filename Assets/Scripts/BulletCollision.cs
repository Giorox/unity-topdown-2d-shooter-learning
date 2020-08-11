using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour
{
    public int damage;
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        var hit = collision2D.gameObject;
        var health = hit.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}