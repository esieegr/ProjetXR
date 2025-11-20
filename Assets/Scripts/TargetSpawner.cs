using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform spawnAreaCenter;
    public float spawnRadius = 5f;

    void Start()
    {
        SpawnTarget();
    }

    void SpawnTarget()
    {
        Vector3 randomPos = spawnAreaCenter.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            1f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        Instantiate(targetPrefab, randomPos, Quaternion.identity);
    }
}
