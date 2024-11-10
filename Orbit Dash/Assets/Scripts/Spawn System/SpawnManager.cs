using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public SpawnableObject obstacleData;
    public SpawnableObject deathTrapData;
    public SpawnableObject collectibleData;

    public Transform plain;

    public float minimumSpawnDistance = 0.6f;

    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private List<GameObject> spawnedDeathTraps = new List<GameObject>();
    private List<GameObject> spawnedCollectibles = new List<GameObject>();

    private List<Vector3> objectPositions = new List<Vector3>();

    [SerializeField] private float queueProcessWaitTime = 1.5f;
    private Queue<System.Action> spawnQueue = new Queue<System.Action>();
    private bool isProcessingQueue = false;

    public GameObject ballPrefab; 
    private GameObject currentBall;

    public bool ifDiedOnce = false;

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
        InvokeRepeating("QueueSpawnCollectible", 2f, collectibleData.spawnInterval);
        InvokeRepeating("QueueSpawnObstacle", 4f, obstacleData.spawnInterval);
        InvokeRepeating("QueueSpawnDeathTrap", 6f, deathTrapData.spawnInterval);
    }

    private void QueueSpawnObstacle()
    {
        if (!GameManager.Instance.isGameActive) return; 
        spawnQueue.Enqueue(() => SpawnObject(obstacleData, spawnedObstacles));
        ProcessQueue();
    }

    private void QueueSpawnDeathTrap()
    {
        if (!GameManager.Instance.isGameActive) return; 
        spawnQueue.Enqueue(() => SpawnObject(deathTrapData, spawnedDeathTraps));
        ProcessQueue();
    }

    private void QueueSpawnCollectible()
    {
        if (!GameManager.Instance.isGameActive) return; 
        spawnQueue.Enqueue(() => SpawnObject(collectibleData, spawnedCollectibles));
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (isProcessingQueue || spawnQueue.Count == 0 || !GameManager.Instance.isGameActive) return; 

        StartCoroutine(ProcessQueueCoroutine());
    }

    private IEnumerator ProcessQueueCoroutine()
    {
        isProcessingQueue = true;
        while (spawnQueue.Count > 0)
        {
            if (!GameManager.Instance.isGameActive) break; 
            var spawnAction = spawnQueue.Dequeue();
            spawnAction();
            yield return new WaitForSeconds(queueProcessWaitTime);
        }
        isProcessingQueue = false;
    }

    private void SpawnObject(SpawnableObject spawnableData, List<GameObject> objectList)
    {
        if (objectList.Count >= spawnableData.maxCount || !GameManager.Instance.isGameActive) return; 

        Vector3 randomPosition;
        Vector3 spawnPosition;
        int maxAttempts = 10;
        int attempts = 0;

        Vector3 ballPosition = currentBall.transform.position; 

        do
        {
            randomPosition = new Vector3(Random.Range(-0.45f, 0.45f), 1f, Random.Range(-0.45f, 0.45f));
            spawnPosition = plain.TransformPoint(randomPosition);
            attempts++;
        }

        while ((!IsFarEnoughFromOthers(spawnPosition) || !IsFarEnoughFromBall(spawnPosition, ballPosition)) && attempts < maxAttempts);

        if (attempts >= maxAttempts) return;

        GameObject spawnedObject = Instantiate(spawnableData.prefab, spawnPosition, plain.rotation);
        spawnedObject.transform.parent = plain;

        objectList.Add(spawnedObject);
        objectPositions.Add(spawnPosition);

        spawnQueue.Enqueue(() => DestroyObjectAfterTime(spawnedObject, spawnPosition, spawnableData.lifeTime, objectList));
    }

    private bool IsFarEnoughFromBall(Vector3 newPosition, Vector3 ballPosition)
    {
        return Vector3.Distance(newPosition, ballPosition) >= minimumSpawnDistance;
    }

    private void DestroyObjectAfterTime(GameObject obj, Vector3 position, float delay, List<GameObject> objectList)
    {
        StartCoroutine(DestroyObjectCoroutine(obj, position, delay, objectList));
    }

    private IEnumerator DestroyObjectCoroutine(GameObject obj, Vector3 position, float delay, List<GameObject> objectList)
    {
        yield return new WaitForSeconds(delay);
        objectList.Remove(obj);
        objectPositions.Remove(position);
        Destroy(obj);
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

    public void RemoveCollectible(GameObject collectible, Vector3 position)
    {
        spawnedCollectibles.Remove(collectible);
        objectPositions.Remove(position);
        Destroy(collectible);

        QueueSpawnCollectible();
    }
    public void ClearAreaAround(Vector3 position, float radius)
    {

        ClearObjectsInRange(spawnedObstacles, position, radius);

        ClearObjectsInRange(spawnedDeathTraps, position, radius);

        ClearObjectsInRange(spawnedCollectibles, position, radius);
    }

    private void ClearObjectsInRange(List<GameObject> objectList, Vector3 center, float range)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (GameObject obj in objectList)
        {
            if (Vector3.Distance(obj.transform.position, center) < range)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach (GameObject obj in objectsToRemove)
        {
            objectList.Remove(obj);
            objectPositions.Remove(obj.transform.position);
            Destroy(obj);
        }
    }
    public void SpawnBall(Vector3 spawnPosition)
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

}
