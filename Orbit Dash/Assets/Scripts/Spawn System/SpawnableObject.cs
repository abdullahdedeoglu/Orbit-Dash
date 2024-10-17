using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnable", menuName = "Spawnable Object")]
public class SpawnableObject : ScriptableObject
{
    public GameObject prefab;  
    public float spawnInterval; 
    public float lifeTime;      
    public int maxCount;       
}
