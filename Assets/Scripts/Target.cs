using UnityEngine;

public class Target : MonoBehaviour
{
    public int pointsValue = 1;
    public GameObject destroyEffect;

    public void Hit(int damage)
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(pointsValue);
        }

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

