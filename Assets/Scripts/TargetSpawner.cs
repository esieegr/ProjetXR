using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public int numberOfTargets = 5;
    public float minDistanceFromFurniture = 0.5f;
    public float targetRadius = 0.2f;
    public int maxSpawnAttempts = 50;

    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(OnSceneLoaded);
    }

    void OnSceneLoaded()
    {
        SpawnTargets();
    }

    void SpawnTargets()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            Debug.LogWarning("No MRUK room found");
            return;
        }

        List<Vector3> spawnedPositions = new List<Vector3>();
        int successfulSpawns = 0;

        for (int i = 0; i < numberOfTargets; i++)
        {
            Vector3? spawnPos = FindValidSpawnPosition(room, spawnedPositions);
            
            if (spawnPos.HasValue)
            {
                Instantiate(targetPrefab, spawnPos.Value, Quaternion.identity);
                spawnedPositions.Add(spawnPos.Value);
                successfulSpawns++;
            }
        }

        Debug.Log($"Successfully spawned {successfulSpawns}/{numberOfTargets} targets");
    }

    Vector3? FindValidSpawnPosition(MRUKRoom room, List<Vector3> existingPositions)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            if (room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, minDistanceFromFurniture, new LabelFilter(~MRUKAnchor.SceneLabels.FLOOR), out Vector3 randomPos, out Vector3 normal))
            {
                if (IsPositionValid(randomPos, existingPositions))
                {
                    return randomPos;
                }
            }
        }

        return null;
    }

    bool IsPositionValid(Vector3 position, List<Vector3> existingPositions)
    {
        foreach (Vector3 existingPos in existingPositions)
        {
            if (Vector3.Distance(position, existingPos) < targetRadius * 2)
            {
                return false;
            }
        }
        return true;
    }
}
