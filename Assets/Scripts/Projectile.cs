using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int damagePoints = 1;

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.1f);
    }
}

