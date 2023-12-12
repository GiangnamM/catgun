using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extension
{
    public class PoolManager : MonoBehaviour
    {
        public static List<PoolObjectInfo> objectPools = new List<PoolObjectInfo>();

        public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            PoolObjectInfo pool = objectPools.Find(p => p.lookupString == objectToSpawn.name);

            if (pool == null)
            {
                pool = new PoolObjectInfo() { lookupString = objectToSpawn.name };
                objectPools.Add(pool);
            }

            GameObject spawnableObj = pool.inactiveObjects.FirstOrDefault();
            if (spawnableObj == null)
            {
                spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            }
            else
            {
                spawnableObj.transform.position = spawnPosition;
                spawnableObj.transform.rotation = spawnRotation;
                pool.inactiveObjects.Remove(spawnableObj);
                spawnableObj.SetActive(true);
            }

            return spawnableObj;
        }

        public static void ReturnObjectToPool(GameObject obj)
        {
            string goName = obj.name.Substring(0, obj.name.Length - 7);
            PoolObjectInfo pool = objectPools.Find(p => p.lookupString == goName);
            if (pool == null)
            {
                Debug.LogWarning("Try to release an object that is not pooled: " + obj.name);
            }
            else
            {
                obj.SetActive(false);
                pool.inactiveObjects.Add(obj);
            }
        }

        public static void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
        }
    }

    public class PoolObjectInfo
    {
        public string lookupString;
        public List<GameObject> inactiveObjects = new List<GameObject>();
    }
}