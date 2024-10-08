using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public GameObject obstaclePrefab;
    public GameObject deathTrapPrefab;
    public GameObject collectiblePrefab;

    public Transform plain;

    public float minimumSpawnDistance = 0.6f;

    public float obstacleSpawnInterval = 2f;
    public float deathTrapSpawnInterval = 6f;
    public float collectibleSpawnInterval = 11f;

    public float obstacleLifeTime = 5f;
    public float deathTrapLifeTime = 7f;
    public float collectibleLifeTime = 10f;

    public int maxObstacles = 3;
    public int maxDeathTraps = 2;
    public int maxCollectibles = 1;

    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private List<GameObject> spawnedDeathTraps = new List<GameObject>();
    private List<GameObject> spawnedCollectibles = new List<GameObject>();

    private List<Vector3> objectPositions = new List<Vector3>();

    [SerializeField] private float queueProcessWaitTime = 1f;
    private Queue<System.Action> spawnQueue = new Queue<System.Action>();
    private bool isProcessingQueue = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InvokeRepeating("QueueObstacleSpawn", 2f, obstacleSpawnInterval);
        InvokeRepeating("QueueDeathTrapSpawn", 6f, deathTrapSpawnInterval);
        InvokeRepeating("QueueCollectibleSpawn", 11f, collectibleSpawnInterval);
    }

    private void QueueObstacleSpawn()
    {
        spawnQueue.Enqueue(() => SpawnObstacle());
        ProcessQueue();
    }

    private void QueueDeathTrapSpawn()
    {
        spawnQueue.Enqueue(() => SpawnDeathTrap());
        ProcessQueue();
    }

    private void QueueCollectibleSpawn()
    {
        spawnQueue.Enqueue(() => SpawnCollectible());
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (isProcessingQueue || spawnQueue.Count == 0) return;
        StartCoroutine(ProcessQueueCoroutine());
    }

    private IEnumerator ProcessQueueCoroutine()
    {
        isProcessingQueue = true;
        while (spawnQueue.Count > 0)
        {
            var spawnAction = spawnQueue.Dequeue(); 
            spawnAction(); 
            yield return new WaitForSeconds(queueProcessWaitTime); 
        }
        isProcessingQueue = false;
    }

    private void SpawnObstacle()
    {
        if (spawnedObstacles.Count >= maxObstacles) return;
        SpawnObject(obstaclePrefab, obstacleLifeTime, spawnedObstacles);
    }

    private void SpawnDeathTrap()
    {
        if (spawnedDeathTraps.Count >= maxDeathTraps) return;
        SpawnObject(deathTrapPrefab, deathTrapLifeTime, spawnedDeathTraps);
    }

    private void SpawnCollectible()
    {
        if (spawnedCollectibles.Count >= maxCollectibles) return;
        SpawnObject(collectiblePrefab, collectibleLifeTime, spawnedCollectibles);
    }

    private void SpawnObject(GameObject prefab, float lifeTime, List<GameObject> objectList)
    {
        Vector3 randomPosition;
        Vector3 spawnPosition;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            randomPosition = new Vector3(Random.Range(-0.45f, 0.45f), 1f, Random.Range(-0.45f, 0.45f));
            spawnPosition = plain.TransformPoint(randomPosition);
            attempts++;
        }
        while (!IsFarEnoughFromOthers(spawnPosition) && attempts < maxAttempts);

        if (attempts >= maxAttempts) return;

        GameObject spawnedObject = Instantiate(prefab, spawnPosition, plain.rotation);
        spawnedObject.transform.parent = plain;

        objectList.Add(spawnedObject);
        objectPositions.Add(spawnPosition);

        StartCoroutine(DestroyObjectAfterTime(spawnedObject, spawnPosition, lifeTime, objectList));
    }

    private bool IsFarEnoughFromOthers(Vector3 newPosition)
    {
        foreach (Vector3 position in objectPositions)
        {
            if (Mathf.Abs(newPosition.x - position.x) < minimumSpawnDistance && Mathf.Abs(newPosition.z - position.z) < minimumSpawnDistance)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator DestroyObjectAfterTime(GameObject obj, Vector3 position, float delay, List<GameObject> objectList)
    {
        yield return new WaitForSeconds(delay);
        objectList.Remove(obj);
        objectPositions.Remove(position);
        Destroy(obj);
    }
}
