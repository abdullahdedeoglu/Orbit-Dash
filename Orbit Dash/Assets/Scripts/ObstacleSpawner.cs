using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform plain;
    public float spawnInterval = 2f;

    public float spawnRangeX = .45f;
    public float spawnRangeZ = .45f;

    public float obstacleLifeTime = 5f;
    public float minimumSpawnDistance = 0.6f;

    [SerializeField] private List<GameObject> spawnedObstacles = new List<GameObject>();
    [SerializeField] private List<Vector3> obstaclePositions = new List<Vector3>();

    private void Start()
    {
        InvokeRepeating("SpawnObstacle", 1f, spawnInterval);
    }

    private void SpawnObstacle()
    {
        if (spawnedObstacles.Count >= 3) return;

        Vector3 randomPosition;
        Vector3 spawnPosition;

        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            randomPosition = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 1f, Random.Range(-spawnRangeZ, spawnRangeZ));
            spawnPosition = plain.TransformPoint(randomPosition);

            attempts++;
        }

        while (!IsFarEnoughFromOthers(spawnPosition) && attempts < maxAttempts);

        if (attempts > maxAttempts) return;

        GameObject spawnedObstacle=  Instantiate(obstaclePrefab, spawnPosition, plain.rotation);
        spawnedObstacle.transform.parent = plain;

        spawnedObstacles.Add(spawnedObstacle);
        obstaclePositions.Add(spawnPosition);

        StartCoroutine(DestroyObstacleAfterTime(spawnedObstacle, spawnPosition, obstacleLifeTime));
    }

    private bool IsFarEnoughFromOthers(Vector3 newPosition)
    {
        int count = 0;
        foreach(Vector3 position in obstaclePositions)
        {
            count++;

            if(Mathf.Abs(newPosition.x - position.x) < minimumSpawnDistance && Mathf.Abs(newPosition.z - position.z) < minimumSpawnDistance)
            {
                return false;
            }
        }

        return true;
    }


    private IEnumerator DestroyObstacleAfterTime(GameObject obstacle, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        spawnedObstacles.Remove(obstacle);
        obstaclePositions.Remove(position);
        Destroy(obstacle);
    }
}
