using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class TargetBehaviour : MonoBehaviour, IHittable
{
    [Header("Destroy Settings")]
    [SerializeField] GameObject destroyEffect;
    [SerializeField] int pointsValue = 1;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float minRotationAngle = 100f;
    [SerializeField] float maxRotationAngle = 260f;

    [Header("Light Pillar Settings")]
    [SerializeField] bool enableLightPillar = true;
    [SerializeField] float pillarHeight = 5f;
    [SerializeField] float pillarRadius = 0.075f;
    [SerializeField] Color laserColor = new Color(0f, 1f, 1f, 0.8f);

    Rigidbody rb;
    LightPillar lightPillar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        if (enableLightPillar)
        {
            lightPillar = gameObject.AddComponent<LightPillar>();
            lightPillar.Initialize(pillarHeight, pillarRadius, laserColor);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 movement = transform.forward * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                Hit(projectile.damagePoints);
                Destroy(collision.gameObject);
            }
            return;
        }

        bool isClockwise = Random.value > 0.5f;
        float rotationAngle = Random.Range(minRotationAngle, maxRotationAngle);
        
        if (!isClockwise)
        {
            rotationAngle = -rotationAngle;
        }
        
        transform.Rotate(0, rotationAngle, 0);
    }

    public void Hit(int damage)
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(pointsValue);
            ScoreManager.Instance.TargetDestroyed();
        }

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

