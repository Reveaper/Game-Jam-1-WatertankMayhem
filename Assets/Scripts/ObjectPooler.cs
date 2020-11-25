using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    public static ObjectPooler Instance;

    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        /*
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
            pooledObject.OnObjectSpawn();*/

        PoolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
