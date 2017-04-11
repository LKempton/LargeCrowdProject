using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrowdAI
{

    public class LODPoolManager
    {
        private int _crowdNumber;
		[SerializeField]
        private int _lodLayers = 5;
        
        private string[] _objectNames;
        private GameObject[] _crowdLODObjects;
        private int[] _objectAmounts;

        private Hashtable _mainPool = new Hashtable();

        private List<GameObject> _tempList;

		/// <summary>
		/// Instantiates and initialises pooled objects
		/// </summary>
		/// <param name="size">the number of </param>
		/// <param name="objects">the array containing the crowd model objects to be pooled (one for each LOD for each crowd member type e.g. medium detail model for woman model on red team)</param>
		/// <param name="objectNames">the array containing the names of each crowd model object to be pooled (one for each LOD for each crowd member type e.g. mediumDetailWomanRed</param>
        public LODPoolManager(int size, GameObject[] objects, string[] objectNames)
        {
			_crowdNumber = size;

            _tempList = new List<GameObject>();

            _objectNames = objectNames;
			_crowdLODObjects = objects;

			//initialise the amounts array
            _objectAmounts = new int[_lodLayers];
            for (int i = 0; i < _objectAmounts.Length; i++)
            {
                _objectAmounts[i] = _crowdNumber;
            }

			//instantiate the pooled objects
            for (int i = 0; i < objectNames.Length; i++)
            {
				//create temp list to store objects for this LOD
				List<GameObject> tempObjList = new List<GameObject>();

				//instantiate the number of objects to pool for this LOD
                for (int j = 0; j < _objectAmounts[i]; j++)
                {
                    GameObject obj = GameObject.Instantiate(_crowdLODObjects[i]);
                    tempObjList.Add(obj);
                }

				//add objects for this LOD to pool
                _mainPool.Add(objectNames[i], tempObjList);
            }
        }

		/// <summary>
		/// Searches the pooled objects to find the next available object to activate.
		/// Returns if found, throws exception if not
		/// </summary>
		/// <returns>The next available pooled object to be activated</returns>
		/// <param name="name">the name of the type of pooled object to look for</param>
        public GameObject GetPooledObject(string name)
        {
            if (_mainPool.ContainsKey(name))
            {
                _tempList =  (List<GameObject> )_mainPool[name];

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

            throw new System.Exception("There are no objects in the pooler with the name you searched for.");
        }

		/// <summary>
		/// Deactivates all pooled objects
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
