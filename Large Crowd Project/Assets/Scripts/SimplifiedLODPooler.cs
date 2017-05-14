using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    /// <summary>
    /// Monobehaviour that pools all crowd objects on scene
    /// </summary>
    public class SimplifiedLODPooler : MonoBehaviour
    {
        public static SimplifiedLODPooler instance;

        private Hashtable _mainPool = new Hashtable();

        private List<GameObject> _tempList = new List<GameObject>();
        
        /// <summary>
        /// Goes through each crowd member sprite game object in the scene and instantiates two higher detail game objects
        /// Puts all of the gameobjects in hashtable indexed by the name of that detail of gameobject
        /// </summary>
        void Awake()
        {
            instance = this;

            List<GameObject> tempRedSpriteList = new List<GameObject>();
            List<GameObject> tempRedLowDetailModelList = new List<GameObject>();
            List<GameObject> tempRedHighDetailModelList = new List<GameObject>();
            List<GameObject> tempBlueSpriteList = new List<GameObject>();
            List<GameObject> tempBlueLowDetailModelList = new List<GameObject>();
            List<GameObject> tempBlueHighDetailModelList = new List<GameObject>();

            var parentObj = new GameObject();
            parentObj.name = "Pooled Objects";

            GameObject[] gameObjectsInScene = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            for (int i = 0; i < gameObjectsInScene.Length; i++)
            {
                if (gameObjectsInScene[i].layer == LayerMask.NameToLayer("CrowdMembers"))
                {
                    CrowdMemberInfo info = gameObjectsInScene[i].GetComponent<CrowdMemberInfo>();

                    GameObject lowDetailGameObject = Instantiate(info.LowDetailModel);
                    lowDetailGameObject.transform.parent = parentObj.transform;
                    lowDetailGameObject.name = info.Team + "_1_2";

                    GameObject highDetailGameObject = Instantiate(info.HighDetailModel);
                    highDetailGameObject.transform.parent = parentObj.transform;
                    highDetailGameObject.name = info.Team + "_1_3";

                    if (info.Team == Team.RED)
                    {
                        tempRedSpriteList.Add(gameObjectsInScene[i]);
                        tempRedLowDetailModelList.Add(lowDetailGameObject);
                        tempRedHighDetailModelList.Add(highDetailGameObject);
                    }
                    else if (info.Team == Team.BLUE)
                    {
                        tempBlueSpriteList.Add(gameObjectsInScene[i]);
                        tempBlueLowDetailModelList.Add(lowDetailGameObject);
                        tempBlueHighDetailModelList.Add(highDetailGameObject);
                    }
                }
            }

            _mainPool.Add("RED_1_1", tempRedSpriteList);
            _mainPool.Add("RED_1_2", tempRedLowDetailModelList);
            _mainPool.Add("RED_1_3", tempRedHighDetailModelList);
            _mainPool.Add("BLUE_1_1", tempBlueSpriteList);
            _mainPool.Add("BLUE_1_2", tempBlueLowDetailModelList);
            _mainPool.Add("BLUE_1_3", tempBlueHighDetailModelList);
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