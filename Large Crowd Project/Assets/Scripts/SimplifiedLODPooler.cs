using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class SimplifiedLODPooler : MonoBehaviour
    {
        private SimplifiedCrowdController scc;

        private Hashtable _mainPool = new Hashtable();

        private List<GameObject> _tempList = new List<GameObject>();

        /// <summary>
        /// Goes through each crowd member sprite game object in the scene and instantiates two higher detail game objects
        /// Puts all of the gameobjects in hashtable indexed by the name of that detail of gameobject
        /// </summary>
        void Start()
        {
            List<GameObject> tempSpriteList = new List<GameObject>();
            List<GameObject> tempLowDetailModelList = new List<GameObject>();
            List<GameObject> tempHighDetailModelList = new List<GameObject>();

            GameObject[] gameObjectsInScene = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            for (int i = 0; i < gameObjectsInScene.Length; i++)
            {
                if (gameObjectsInScene[i].layer == scc.CrowdMemberLayer)
                {
                    tempSpriteList.Add(gameObjectsInScene[i]);

                    CrowdMemberInfo info = gameObjectsInScene[i].GetComponent<CrowdMemberInfo>();

                    GameObject lowDetailGameObject = Instantiate(info.LowDetailModel);
                    lowDetailGameObject.name = info.Team + "_1_2";
                    tempLowDetailModelList.Add(lowDetailGameObject);

                    GameObject highDetailGameObject = Instantiate(info.HighDetailModel);
                    highDetailGameObject.name = info.Team + "_1_3";
                    tempHighDetailModelList.Add(highDetailGameObject);
                    
                }
            }

            _mainPool.Add(tempSpriteList[0].name, tempSpriteList);
            _mainPool.Add(tempLowDetailModelList[0].name, tempLowDetailModelList);
            _mainPool.Add(tempHighDetailModelList[0].name, tempHighDetailModelList);
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