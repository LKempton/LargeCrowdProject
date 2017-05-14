using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Newtonsoft.Json;

namespace CrowdAI
{
    /// <summary>
    ///Monobehaviour that Creates Crowds in edit mode and manages them in play mode
    /// </summary>
    /// 

    public class CrowdController : MonoBehaviour
    {
        ///ctrl + m + o = collapse all, ctrl + m + l = expand all 

        /// <summary>
        /// The _instance of the Controller in this scene, if it exists
        /// </summary>
        private static CrowdController _instance;

        /// <summary>
        /// Pools the models and sprites at runtime
        /// </summary>
        private LODPoolManager _poolManager;

        /// <summary>
        /// Used to manage animation states
        /// </summary>
        [SerializeField]
        private string[] _crowdStates;

        /// <summary>
        /// Tracks the number of crowd members on the Scene
        /// </summary>
        private int _crowdCount = 0;

        /// <summary>
        ///  whether the controller has been set up or not
        /// </summary>
        bool _setUp = false;

        private string _scene;

        /// <summary>
        /// All the crowd groups in the scene
        /// </summary>
        private List<CrowdGroup> _crowdGroups;

        /// <summary>
        /// The parent game objects for each corwd formation generated
        /// </summary>
        private List<GameObject> _crowdSources;

        /// <summary>
        /// All crowd members are put in the group when generated
        /// </summary>
        private CrowdGroup _groupUnassigned;

        /// <summary>
        /// (Not Implemented) 
        /// Whether the generated crowds should stagger or not
        /// </summary>
        [SerializeField]
        private bool _randomStagger;

        /// <summary>
        /// The formation the crowd will generate int
        /// </summary>
        [SerializeField]
        private CrowdFormation _crowdFormation;

        /// <summary>
        /// Range for which it takes for a group to transistion
        /// </summary>
        [SerializeField]
        private float _animationStagger = 0.25f;

        /// <summary>
        /// How many members per unit space are generated
        /// </summary>
        [SerializeField]
        private float _density;

        /// <summary>
        /// The radius inside of a hollow ring formation
        /// </summary>
        [SerializeField]
        private float _innerRadius;

        /// <summary>
        /// The y axis rotation for the crowd members generated 
        /// </summary>
        [SerializeField]
        private float _rotDir = 0;

        /// <summary>
        /// The prefab used to show where crowd members will be generated
        /// </summary>
        [SerializeField]
        private GameObject _placeholderPrefab;

        

        #region Initialisation 
        /// <summary>
        /// Called when play mode starts
        /// </summary>
        void Awake()
        {
            int _groupLength = (_crowdGroups == null) ? 0 : _crowdGroups.Count;

            int _totalElements = 0;
            int _currentIndex = 0;

        }

        /// <summary>
        /// Instantiates classes inside of the controller
        /// This will cause the controller to lose all references to objects
        /// </summary>
        public void SetUp()
        {

            _scene = SceneManager.GetActiveScene().name;


            if (_instance != null)
            {
                if (_instance != this && _instance.ControllerScene == _scene)
                {


                    Debug.LogWarning("Can Only Have one Crowd Controller per scene");
                    Destroy(gameObject);
                    return;
                }
            }
            print("set up");
            _instance = this;

            _crowdGroups = new List<CrowdGroup>();
            _groupUnassigned = new CrowdGroup("Unassigned");
            _crowdSources = new List<GameObject>();

            _setUp = true;
        }

        #endregion

        #region GroupManagement
        /// <summary>
        /// Gets all the groups in the controller
        /// </summary>
        /// <returns> All crowd groups in the controller </returns>
        public CrowdGroup[] GetGroups()
        {
            if (_crowdGroups == null)
            {
                return null;
            }
            else
            {
                return _crowdGroups.ToArray();
            }

        }

        /// <summary>
        /// Gets the group used to store unassigned crowd members
        /// </summary>
        /// <returns> The unassigned group if it has been intialised</returns>
        public CrowdGroup GetUnassignedGroup
        {
            get
            {
                if (_groupUnassigned == null)
                {
                    return null;
                }
                else
                {
                    return _groupUnassigned;
                }
            }
        }

        /// <summary>
        /// Creates a new group and stores it privately
        /// </summary>
        /// <param name="groupName"> The name of the group created</param>
        public void AddGroup(string groupName)
        {
            if (_crowdGroups == null)
            {
                _crowdGroups = new List<CrowdGroup>();
            }
            _crowdGroups.Add(new CrowdGroup(groupName));


        }

        /// <summary>
        /// Removes a group and puts its members into the unassigned group
        /// </summary>
        /// <param name="groupName"> The name of the group to be removed</param>
        /// <returns>True if the group existed (and therefore has been removed)</returns>
        public bool RemoveGroup(string groupName)
        {


            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                if (_crowdGroups[i].GroupName == groupName)
                {
                    var _group = _crowdGroups[i];

                    var _previousMembers = _group.ClearAllForDeletion();

                    if (_previousMembers != null)
                    {
                        if (_previousMembers.Length > 0)
                        {
                            _groupUnassigned.AddCrowdMember(_previousMembers);
                        }

                    }

                    _crowdGroups.RemoveAt(i);


                    return true;
                }
            }


            return false;
        }

        /// <summary>
        /// Gets the name of the group names
        /// </summary>
        /// <returns> An array of all group names </returns>
        public string[] GetGroupNames()
        {
            if (_crowdGroups == null)
            {
                return null;
            }

            var _names = new string[_crowdGroups.Count];

            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                _names[i] = _crowdGroups[i].GroupName;
            }

            return _names;
        }

        /// <summary>
        /// Attempts to add crowd members to the group given by name
        /// </summary>
        /// <param name="groupName">The name of the group to add crowd members to</param>
        /// <param name="group"> The array of crowd members to be added</param>
        /// <returns>True if the group has been found (and the crowd members were added)</returns>
        public bool AddCrowdMembers(string groupName, GameObject[] group)
        {
            if (_crowdGroups == null)
            {
                return false;
            }

            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                var _currentGroup = _crowdGroups[i];

                if (_currentGroup.GroupName == groupName)
                {
                    _currentGroup.AddCrowdMember(group);
                    return true;
                }
            }


            return false;
        }

        #endregion

        #region AnimationStates
        /// <summary>
        /// Gets all the States that any crowd member may have
        /// </summary>
        /// <returns>An array of strings that are the names of the states</returns>
        public string[] GetCrowdStates()
        {
            //copies array since they are passed by reference
            var _crowdStatesCopy = new string[_crowdStates.Length];

            for (int i = 0; i < _crowdStates.Length; i++)
            {
                _crowdStatesCopy[i] = _crowdStates[i];
            }
            return _crowdStatesCopy;
        }

        /// <summary>
        /// Searches for a state
        /// </summary>
        /// <param name="stateName">The name of the state to be searched for</param>
        /// <returns> True if the state exists</returns>
        public bool StateExists(string stateName)
        {
            for (int i = 0; i < _crowdStates.Length; i++)
            {
                if (stateName == _crowdStates[i])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the animation state of all crowd members
        /// </summary>
        /// <param name="state"> The name of the animation state to be set</param>
        /// <param name="useRandDelay">whether there is a random delay between a state transistion</param>
        public void SetState(string state, bool useRandDelay)
        {

            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                _crowdGroups[i].SetState(state, useRandDelay);
            }

        }

        /// <summary>
        /// Sets the animation state of a crowd group
        /// </summary>
        /// <param name="state">Name of the animation state to be set</param>
        /// <param name="groupName">Name of the group who's animation state is changing</param>
        /// <param name="useRandDelay">whether there is a random delay between a state transistion</param>
        /// <returns> True if the state has been set sucessfully</returns>
        public bool SetState(string state, string groupName, bool useRandDelay)
        {
            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                if (groupName == _crowdGroups[i].GroupName)
                {
                    _crowdGroups[i].SetState(state, useRandDelay);

                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Enables and disable all animations
        /// </summary>
        public void ToggleAnimations()
        {
            for (int i = 0; i < _crowdGroups.Count; i++)
            {
                _crowdGroups[i].ToggleAnimations();
            }
        }

        #endregion


        #region Generation
        /// <summary>
        /// Generates a crowd based on the current values in the controller
        /// </summary>
        public void GenerateCrowd()
        {
            if (!_setUp)
            {
                SetUp();
            }

            var _parent = new GameObject();
            _parent.name = "Crowd Source";

            var _cleaner = _parent.AddComponent<CrowdSourceCleaner>();


            var _bounds = transform.GetChild(0).transform.localPosition;

            _parent.transform.position = transform.position;

            var _posModifier = Vector3.zero;

            if (_bounds.x < 0)
            {
                _posModifier.x += _bounds.x;
                _bounds.x *= -1;
            }

            if (_bounds.z < 0)
            {
                _posModifier.z += _bounds.z;
                _bounds.z *= -1;
            }

            _parent.transform.position += _posModifier;


            GameObject[] _newCrowd;

            _cleaner.Controller = this;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:

                    _parent.transform.position += .5f * _bounds;
                    _newCrowd = CrowdGen.GenCrowdCircle(_density, _rotDir, _parent, _bounds, _placeholderPrefab);
                    break;


                case CrowdFormation.SQUARE:

                    _parent.transform.position += .5f * CrowdGen.GetObjectBounds(_placeholderPrefab);
                    _newCrowd = CrowdGen.GenCrowdSquare(_density, _rotDir, _parent, _bounds, _placeholderPrefab);
                    break;

                default: // for ring formation
                    _parent.transform.position += .5f * _bounds;

                    _newCrowd = CrowdGen.GenCrowdRing(_density, _rotDir, _parent, _bounds, _placeholderPrefab, _innerRadius);
                    break;
            }

            if (_newCrowd.Length > 0)
            {
                _groupUnassigned.AddCrowdMember(_newCrowd);
                _crowdSources.Add(_parent);
                RecalculateCount();
            }
            else
            {
                Destroy(_parent);
            }
        }


        /// <summary>
        /// Approximates the number of crowd members that will be generated ,
        /// based on the current values in this object
        /// </summary>
        /// <returns> The predicted value for the crowd members generated</returns>
        public int GetPrediction()
        {
            int _prediction = 0;

            var _bounds = transform.GetChild(0).transform.localPosition;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:
                    _prediction = CrowdGen.EstimateCircle(_density, _bounds);
                    break;

                case CrowdFormation.RING:
                    _prediction = CrowdGen.EstimateRing(_density, _bounds, _innerRadius);

                    break;

                case CrowdFormation.SQUARE:
                    _prediction = CrowdGen.EstimateSquare(_density, _bounds);
                    break;
            }


            return _prediction;
        }

        /// <summary>
        /// Recounts the number of models in the scene and stores it
        /// </summary>
        public void RecalculateCount()
        {
            if (_groupUnassigned == null)
            {
                return;
            }

            _crowdCount = _groupUnassigned.Size;

            if (_crowdGroups != null)
            {
                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _crowdCount += _crowdGroups[i].Size;

                }
            }
        }

        /// <summary>
        /// Returns the number of crowd members in the current scene
        /// </summary>
        public int Size
        {
            get
            {
                return _crowdCount;
            }
        }

        /// <summary>
        ///Used to destroy a crowd formation
        /// </summary>
        /// <param name="crowdMembers"> The crowd members </param>
        public void RemoveSourceChildren(GameObject[] crowdMembers)
        {
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                var _currentChild = crowdMembers[i];

                bool _childRemoved = _groupUnassigned.Remove(_currentChild);

                if (_childRemoved)
                {
                    continue;
                }
                else
                {
                    if (_crowdGroups == null)
                    {


                        for (int j = 0; j < _crowdGroups.Count; j++)
                        {
                            _childRemoved = _crowdGroups[j].Remove(_currentChild);

                            if (_childRemoved)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            RecalculateCount();

        }

        #endregion
        

        #region InstanceManagement


        /// <summary>
        /// Gets the current instance of the crowd controller
        /// </summary>
        /// <returns>the current instance of the controller on scene</returns>
        public static CrowdController GetCrowdController()
        {
            return _instance;
        }

        /// <summary>
        /// The scene that this controller was created on
        /// </summary>
        public string ControllerScene
        {
            get
            {
                return _scene;
            }
        }

        #endregion

        /// <summary>
        /// Prints out current object values to the console 
        /// </summary>
        public void ShowDebugInfo()
        {
            string _outInfo = "Current Crowd Count: " + _crowdCount;
            _outInfo += "\n Current Groups and the number of their members:\n";

            if (_groupUnassigned != null)
            {
                _outInfo += _groupUnassigned.GroupName + ", Size: " + _groupUnassigned.Size + "\n";
            }

            if (_crowdGroups != null)
            {
                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _outInfo += _crowdGroups[i].GroupName + ", Size: " + _crowdGroups[i].Size + "\n";
                }
            }
            else
            {
                _outInfo += "Their are no groups \n";
            }


            _outInfo += "N. Of Crowd States:";
            if (_crowdStates != null)
            {
                _outInfo += _crowdStates.Length + "\n The States Are:";

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _outInfo += "\n -" + _crowdStates[i];
                }
                _outInfo += "\n";
            }
            else
            {
                _outInfo += "0 \n";
            }




            Debug.Log(_outInfo);
        }

        /// <summary>
        /// Returns a list of all the source parent gameobjects
        /// </summary>
        public List<GameObject> Sources
        {
            get
            {
                return _crowdSources;
            }
        }

        /// <summary>
        /// Overwrites the current controller with data passed in
        /// </summary>
        /// <param name="data"> crowd controller data</param>
        public void OverWriteData(CrowdData data)
        {
            if (Application.isPlaying)
            {
                // load the empty placeholders

            }
            else
            {
                // load the placeholders
            }

        }

        /// <summary>
        /// (PlayMode Only)
        /// Gets a pooled Game Object
        /// By default the naming is GroupName_ModelNumberInGroup_LODLevel
        /// </summary>
        /// <param name="name"> The name of the object in the pooler </param>
        /// <returns>The game object  if it exists, otherwise null</returns>
        public GameObject GetPooled(string name)
        {
            if (Application.isPlaying)
            {
                return _poolManager.GetPooledObject(name);
            }
            else
            {
                return null;
            }

        }

    }

}





