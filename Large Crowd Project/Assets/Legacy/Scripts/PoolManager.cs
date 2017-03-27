using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CrowdAI
{

    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private int crowdNumber;
        [SerializeField]
        private int lodLayers;

        [SerializeField]
        private string[] objectNames;
        [SerializeField]
        private GameObject[] crowdLODObjects;
        [SerializeField]
        private int[] objectAmounts;

        private Hashtable mainPool = new Hashtable();

        private List<GameObject> tempList;

        void Start()
        {
            tempList = new List<GameObject>();

            objectNames = new string[lodLayers];
            crowdLODObjects = new GameObject[lodLayers];
            objectAmounts = new int[lodLayers];

            for (int i = 0; i < objectAmounts.Length; i++ )
            {
                objectAmounts[i] = crowdNumber;
            }

            for (int i = 0; i < objectNames.Length; i++)
            {
                List<GameObject> objList = new List<GameObject>();

                for (int j = 0; j < objectAmounts[i]; j++)
                {
                    GameObject obj = Instantiate(crowdLODObjects[i]);
                    obj.transform.parent = transform;
                    objList.Add(obj);
                }

                mainPool.Add(objectNames[i], objList);
            }
        }

        public GameObject GetPooledObject(string name)
        {
            if (mainPool.ContainsKey(name))
            {
                tempList = mainPool[name] as List<GameObject>;

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