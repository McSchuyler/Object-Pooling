using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    public class Pool
    {
        public GameObject prefab;
        public Queue<GameObject> pool;

        public Pool(GameObject obj)
        {
            prefab = obj;
            pool = new Queue<GameObject>();
        }
    }

    Dictionary<string, Pool> poolDictionary;

    [System.Serializable]
    public class PoolReference
    {
        public string tag;
        public GameObject prefab;
        public int count;
    }
    public List<PoolReference> poolReferenceList;

    public static ObjectPooling instance;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        poolDictionary = new Dictionary<string, Pool>();

        foreach (PoolReference poolRef in poolReferenceList)
        {
            Pool newPool = new Pool(poolRef.prefab);
            //add new pool to dictionary
            poolDictionary.Add(poolRef.tag, newPool);

            for (int i = 0; i < poolRef.count; i++)
            {
                GameObject spawnedObject = Instantiate(poolRef.prefab);
                poolDictionary[poolRef.tag].pool.Enqueue(spawnedObject);
                spawnedObject.SetActive(false);
            }
        }
    }

    public void BorrowFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary[tag].pool.Count == 0)
        {
            OnPoolEmpty(tag);
        }

        //remove game object from queue
        GameObject borrowedObject = poolDictionary[tag].pool.Dequeue();
        //set game object to desired position and rotation, also enable it
        borrowedObject.transform.position = position;
        borrowedObject.transform.rotation = rotation;
        borrowedObject.SetActive(true);
    }

    public void ReturnToPool(string tag, GameObject returnedObject)
    {
        returnedObject.SetActive(false);
        poolDictionary[tag].pool.Enqueue(returnedObject);
    }

    private void OnPoolEmpty(string tag)
    {
        GameObject newObject = Instantiate(poolDictionary[tag].prefab);
        poolDictionary[tag].pool.Enqueue(newObject);
    }
}