using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrowdAI
{

    public class LODPoolManager
    {
        private int _crowdNumber;
        private int _lodLayers;
        
        private string[] _objectNames;
        private GameObject[] _crowdLODObjects;
        private int[] _objectAmounts;

        private Hashtable _mainPool = new Hashtable();

        private List<GameObject> _tempList;

        public LODPoolManager(int size, int layers, GameObject[] objects, string[] objectNames)
        {
            _crowdNumber = size;
            _lodLayers = layers;
            _crowdLODObjects = objects;

            _tempList = new List<GameObject>();

            objectNames = new string[_lodLayers];
            _crowdLODObjects = new GameObject[_lodLayers];
            _objectAmounts = new int[_lodLayers];

            for (int i = 0; i < objectNames.Length; i++)
            {
                objectNames[i] = i.ToString();
            }

            for (int i = 0; i < _objectAmounts.Length; i++)
            {
                _objectAmounts[i] = _crowdNumber;
            }

            for (int i = 0; i < objectNames.Length; i++)
            {
                List<GameObject> objList = new List<GameObject>();

                for (int j = 0; j < _objectAmounts[i]; j++)
                {
                    GameObject obj = GameObject.Instantiate(_crowdLODObjects[i]);
                    objList.Add(obj);
                }

                _mainPool.Add(objectNames[i], objList);
            }
        }

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
