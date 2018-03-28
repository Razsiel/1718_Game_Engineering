using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Photon
{
    /// <summary>
    /// A simple objectpool for recycling listview items
    /// </summary>
    public class SimpleObjectPool : MonoBehaviour
    {
        public GameObject prefab;

        private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

        void Awake()
        {
            //prefab = GameObject.Find("RoomButton");
        }

        public GameObject GetObject()
        {
            GameObject spawnedGameObject;

            if(inactiveInstances.Count > 0)
                spawnedGameObject = inactiveInstances.Pop();
            else
            {
                Debug.Log(prefab);
                spawnedGameObject = Instantiate(prefab);

                PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
                pooledObject.pool = this;
            }

            spawnedGameObject.transform.SetParent(null);
            spawnedGameObject.SetActive(true);

            return spawnedGameObject;
        }

        public void ReturnObject(GameObject toReturn)
        {
            PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

            if(pooledObject != null && pooledObject.pool == this)
            {
                toReturn.transform.SetParent(transform);
                toReturn.SetActive(false);
                inactiveInstances.Push(toReturn);
            }
            else
            {
                Destroy(toReturn);
            }
        }
    }

    public class PooledObject : MonoBehaviour
    {
        public SimpleObjectPool pool;
    }
}
