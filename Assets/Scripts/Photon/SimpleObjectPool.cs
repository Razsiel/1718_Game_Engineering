using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Photon
{
    /// <summary>
    /// A simple objectpool for recycling objects like buttons in a listview for example.
    /// Works like a recyclerview in Android.
    /// </summary>
    public class SimpleObjectPool : MonoBehaviour
    {
        public GameObject prefab;

        private Stack<GameObject> inactiveInstances = new Stack<GameObject>();
       
        /// <summary>
        /// Gets a object from our recycler
        /// </summary>
        /// <returns>A object from our pool</returns>
        public GameObject GetObject()
        {            
            GameObject spawnedGameObject;

            if(inactiveInstances.Count > 0)
                //We have an inactive object to give back
                spawnedGameObject = inactiveInstances.Pop();
            else
            {                
                //Create a new one of given prefab set in editor
                spawnedGameObject = Instantiate(prefab);

                //Set the pool the object belongs to
                PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
                pooledObject.pool = this;
            }

            spawnedGameObject.transform.SetParent(null);
            spawnedGameObject.SetActive(true);

            return spawnedGameObject;
        }

        /// <summary>
        /// Returns 'destroys' the inactive object back to the recycler
        /// </summary>
        /// <param name="toReturn">The object we don't need anymore</param>
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
