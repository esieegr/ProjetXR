using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 2f;
    public float moveRadius = 5f;

    private Vector3 targetPosition;
    private float speed;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        float x = Random.Range(-moveRadius, moveRadius);
        float z = Random.Range(-moveRadius, moveRadius);
        targetPosition = new Vector3(x, transform.position.y, z);
        speed = Random.Range(minSpeed, maxSpeed);
    }
}
