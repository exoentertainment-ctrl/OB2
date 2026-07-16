using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] GameObject objectToPool;
    [SerializeField] int amountToPool;

    #endregion

    private GameObject poolObject;
    public List<GameObject> pooledObjects;
    
    void Start()
    {
        poolObject = new GameObject();
        poolObject.name = transform.root.name + transform.name + " Pool";
        SetObjectToPool();
    }

    public int GetPooledObjectCount()
    {
        return pooledObjects.Count;
    }
    
    public void SetObjectToPool()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, poolObject.transform);
            //tmp.transform.SetParent(poolObject.transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    void OnDestroy()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            Destroy(pooledObjects[i]);
        }
        
        Destroy(poolObject);
    }

    public void Upgrade()
    {
        foreach (GameObject pooledObject in pooledObjects)
        {
            pooledObject.TryGetComponent<IUpgrade>(out IUpgrade hit);
            hit?.Upgrade();
        }
    }
}
