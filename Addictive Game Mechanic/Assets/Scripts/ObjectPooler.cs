using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public GameObject pooledObject; // current poolable object
    public int pooledAmount; // size of the pool
    List<GameObject> pooledObjects; // the list of objects in the pool

	void Start ()
    {
        // create the list
        pooledObjects = new List<GameObject>();
        // for every object in the pool
        for (int i = 0; i < pooledAmount; i++)
        {
            // make a gameobject, cast it as a gameobject
            GameObject obj = (GameObject)Instantiate(pooledObject);
            // set it to false so we dont see it
            obj.SetActive(false);
            // add it to the list
            pooledObjects.Add(obj);
        }
	}

    public GameObject GetPooledObject()
    {
        // for every object in the pooled list
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // if its not active in the hierarchy
            if(!pooledObjects[i].activeInHierarchy)
            {
                // return it because it can be used at the next platform
                return pooledObjects[i];
            }
        }
        // if the list doesnt have any non active platforms ready to be reused
        // make a new one similar to what happened in start
        GameObject obj = (GameObject)Instantiate(pooledObject);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        // after making another instance, return what we just made
        return obj;
    }
}
