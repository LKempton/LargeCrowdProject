using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrowdAI
{

    public class LODPoolManager
    {
        private int crowdNumber;
        private int lodLayers;
        
        private string[] objectNames;
        private GameObject[] crowdLODObjects;
        private int[] objectAmounts;

        private Hashtable mainPool = new Hashtable();

        private List<GameObject> tempList;

        public LODPoolManager(int size, int layers, GameObject[] objects)
        {
            crowdNumber = size;
            lodLayers = layers;
            crowdLODObjects = objects;

            tempList = new List<GameObject>();

            objectNames = new string[lodLayers];
            crowdLODObjects = new GameObject[lodLayers];
            objectAmounts = new int[lodLayers];

            for (int i = 0; i < objectNames.Length; i++)
            {
                objectNames[i] = i.ToString();
            }

            for (int i = 0; i < objectAmounts.Length; i++)
            {
                objectAmounts[i] = crowdNumber;
            }

            for (int i = 0; i < objectNames.Length; i++)
            {
                List<GameObject> objList = new List<GameObject>();

                for (int j = 0; j < objectAmounts[i]; j++)
                {
                    GameObject obj = GameObject.Instantiate(crowdLODObjects[i]);
                    objList.Add(obj);
                }

                mainPool.Add(objectNames[i], objList);
            }
        }

        public GameObject GetPooledObject(string name)
        {
            if (mainPool.ContainsKey(name))
            {
                tempList =  (List<GameObject> )mainPool[name];

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i] != null)
                    {
                        if (!tempList[i].activeInHierarchy)
                        {
                            return tempList[i];
                        }
                    }
                }
                throw new System.Exception("There are no more inactive instances of the object you searched for in the pooler");
            }

            throw new System.Exception("There are no objects in the pooler with the name you searched for.");
        }

        public void ResetPool()
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i] != null)
                {
                    tempList[i].SetActive(false);
                }
            }
        }
    }
}
