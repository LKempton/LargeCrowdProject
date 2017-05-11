using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrowdAI
{
    [System.Serializable]
    public class LODPoolManager
    {

        [SerializeField]
        private int _lodLayers = 5; //the number of levels of detail models for each crowd character

        private Hashtable _mainPool = new Hashtable();

        private List<GameObject> _tempList;

		/// <summary>
		/// Creates instances of each individual crowd model object and pools them
		/// </summary>
		/// <param name="size">array storing the number of instances to create of each crowd model object (one for each level of detail for each crowd member type e.g. medium detail model for woman character in red group)</param>
		/// <param name="objects">the array containing the crowd model gameobjects to have instances made of (one for each level of detail for each crowd member type e.g. medium detail model for woman character in red group)</param>
		/// <param name="objectNames">the array containing the names of each crowd model gameobject to have instances made of (one for each level of detail for each crowd member type e.g. Red_2_3)</param>
        public LODPoolManager(int[] size, GameObject[] objects, string[] objectNames)
        {
      
            _tempList = new List<GameObject>();

            //instantiate all pooled objects
            //go through each individual crowd model gameobject
            for (int i = 0; i < objectNames.Length; i++)
            {
                List<GameObject> tempObjList = new List<GameObject>();

                //instantiate the number of instances to be pooled of the crowd model gameobject
                for (int j = 0; j < size[i]; j++)
                {
                    GameObject obj = GameObject.Instantiate(objects[i]);
                    tempObjList.Add(obj);
                }

                //add instances of this 
                _mainPool.Add(objectNames[i], tempObjList);
            }
        }

		/// <summary>
		/// Searches the object pool to find the next available instance of a crowd model gameobject to activate.
		/// Returns if found, throws exception if not
		/// </summary>
		/// <returns>The next available instance of a crowd model gameobject in the pool</returns>
		/// <param name="name">the name of the crowd model gameobject to search for</param>
        public GameObject GetPooledObject(string name)
        {
            //if a crowd model gameobject by the name searched for is in the pool
            if (_mainPool.ContainsKey(name))
            {
                //create a list of all instances of the crowd model gameobject
                _tempList = (List<GameObject>)_mainPool[name];

                //find the first instance that is inactive and not null and return it
                for (int i = 0; i < _tempList.Count; i++)
                {
                    if (_tempList[i] != null)
                    {
                        if (!_tempList[i].activeInHierarchy)
                        {
                            return _tempList[i];
                        }
                    }
                }
                throw new System.Exception("There are no more inactive instances of the object you searched for in the pooler");
            }
            throw new System.Exception("There are no objects in the pool with the name you searched for.");
        }

		/// <summary>
		/// Sets all objects in the pool to inactive
		/// </summary>
        public void ResetPool()
        {
            for (int i = 0; i < _tempList.Count; i++)
            {
                if (_tempList[i] != null)
                {
                    _tempList[i].SetActive(false);
                }
            }
        }
    }
}
