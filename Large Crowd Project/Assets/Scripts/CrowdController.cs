using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Newtonsoft.Json;

namespace CrowdAI
{
    /// <summary>
    ///Creates Crowds in edit mode and manages them in play mode
    /// </summary>
    /// 
    [RequireComponent(typeof(ControllerDelegator))]
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
        /// The name of the current scene the controller is in
        /// </summary>
        private string _scene;

        /// <summary>
        /// The path where the controller saves data
        /// </summary>
        private string _savePath;
        /// <summary>
        /// The name of the file that the controller saves
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Tracks the number of crowd members on the Scene
        /// </summary>
        private int _crowdCount = 0;


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

        /// <summary>
        /// (PlayMode Only)
        /// Gets a pooled Game Object
        /// By default the naming is GroupName_ModelNumberInGroup_LODLevel
        /// </summary>
        /// <param name="name"> The name of the object pooled</param>
        /// <returns>The game object  if it exists, otherwise null</returns>
        public GameObject GetPooled(string name)
        {
            return _poolManager.GetPooledObject(name);
        }

        /// <summary>
        /// Gets the name of the group names
        /// </summary>
        /// <returns> </returns>
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
        /// Called when play mode starts
        /// </summary>
        void Awake()
        {
            ReadAll();

            int _groupLength = (_crowdGroups == null) ? 0 : _crowdGroups.Count;

            int _totalElements = 0;
            int _currentIndex = 0;




        }

        /// <summary>
        /// Puts all variables in a structure so that they can be serialized by a JSON writer
        /// </summary>
        /// <returns>All values required for saving in order to reconstruct this object</returns>
        public CrowdData GetData()
        {

            CrowdData _outData = new CrowdData();

            _outData._animationStagger = _animationStagger;

            _outData._position._posX = transform.position.x;
            _outData._position._posY = transform.position.y;
            _outData._position._posZ = transform.position.z;

            _outData._position._rotW = transform.rotation.w;
            _outData._position._rotX = transform.rotation.x;
            _outData._position._rotY = transform.rotation.y;
            _outData._position._rotZ = transform.rotation.z;

            if (_crowdStates != null)
            {
                _outData._stateNameSize = _crowdStates.Length;
                _outData._stateNames = _crowdStates;
            }
            if (_crowdGroups != null)
            {
                _outData._groupCount = _crowdGroups.Count;
                _outData._groups = new GroupData[_crowdGroups.Count];

                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _outData._groups[i] = _crowdGroups[i].GetData(_crowdSources);
                }
            }

            if (_groupUnassigned != null)
            {
                _outData._unassignedGroup = _groupUnassigned.GetData(_crowdSources);
            }

            if (_crowdSources != null)
            {
                if (_crowdSources.Count > 0)
                {
                    _outData._parents = new TransFormData[_crowdSources.Count];

                    for (int i = 0; i < _crowdSources.Count; i++)
                    {
                        var _transformData = new TransFormData();
                        var _currentParent = _crowdSources[i].transform;

                        _transformData._posX = _currentParent.position.x;
                        _transformData._posY = _currentParent.position.y;
                        _transformData._posZ = _currentParent.position.z;

                        _transformData._rotX = _currentParent.rotation.x;
                        _transformData._rotY = _currentParent.rotation.y;
                        _transformData._rotZ = _currentParent.rotation.z;
                        _transformData._rotW = _currentParent.rotation.w;
                    }
                }
            }

            return _outData;
        }

        /// <summary>
        /// Gets all the groups in the controller
        /// </summary>
        /// <returns> All crowd groups in </returns>
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
        /// <returns> true if the state exists</returns>
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

        /// <summary>
        /// Creates a new group and stores it privately
        /// </summary>
        /// <param name="groupName"> the name of the group created</param>
        public void AddGroup(string groupName)
        {
            if (_crowdGroups == null)
            {
                _crowdGroups = new List<CrowdGroup>();
            }
            _crowdGroups.Add(new CrowdGroup(groupName));


        }

        /// <summary>
        ///Used to destory a crowd formation
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


        }

        /// <summary>
        /// Removes a group and puts it's members into unassigned
        /// </summary>
        /// <param name="groupName"> The name of the group to be removed</param>
        /// <returns>true if the group existed (and therefore has been removed)</returns>
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

        public void GenerateCrowd()
        {
            if (_groupUnassigned == null)
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

                default:
                    _parent.transform.position += .5f * _bounds;

                    _newCrowd = CrowdGen.GenCrowdRing(_density, _rotDir, _parent, _bounds, _placeholderPrefab, _innerRadius);
                    break;
            }

            if (_newCrowd.Length > 0)
            {

                _groupUnassigned.AddCrowdMember(_newCrowd);
                _crowdCount = RecalculateCount();
                _crowdSources.Add(_parent);


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
        /// <returns> the predicted value for the crowd members generated</returns>
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
        /// Recounts the number of models in the scene
        /// </summary>
        /// <returns>The number of models in the scene</returns>
        private int RecalculateCount()
        {
            if (_groupUnassigned == null)
            {
                return 0;
            }

            int size = _groupUnassigned.Size;

            if (_crowdGroups != null)
            {
                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    size += _crowdGroups[i].Size;

                }
            }
            return size;
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
        /// Attempts to add crowd members to the group given by name
        /// </summary>
        /// <param name="groupName">The name of the group to add crowd members to</param>
        /// <param name="group"> The array of crowd members to be added</param>
        /// <returns>true if the group has been found (and therefore the crowd member were added)</returns>
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
        /// Instansiates classes inside of the controller to do with 
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
            _instance = this;

            _crowdGroups = new List<CrowdGroup>();
            _groupUnassigned = new CrowdGroup("Unassigned");
            _crowdSources = new List<GameObject>();

        }

        /// <summary>
        /// Saves data for the controller to disk
        /// </summary>
        /// <param name="destroyPlacholders">Whether the objects in the controller should be cleared
        /// useful for editor as references don't persist in playmode
        /// </param>
        public void SaveAll(bool destroyPlacholders)
        {

            if (!Application.isEditor)
            {
                return;
            }

            var _data = GetData();


            _savePath = Application.dataPath + @"/CrowdAssetData";
            _fileName = @"/CrowdData - " + SceneManager.GetActiveScene().name + ".data.json";


            if (!Directory.Exists(_savePath))
            {

                Directory.CreateDirectory(_savePath);
            }



            string _serializedData = JsonConvert.SerializeObject(_data);

            File.WriteAllText(_savePath + _fileName, _serializedData);

            if (destroyPlacholders)
            {
                RemovePlaceholderReferences();
                _crowdCount = RecalculateCount();
            }

        }

        /// <summary>
        /// Overwrite the current object's values 
        /// </summary>
        /// <param name="data"> Value to be overwritten to</param>
        private void OverWriteData(CrowdData data)
        {
            if (Application.isEditor)
            {
                RemovePlaceholderReferences();
            }
            SetUp();
            if (data._path != null)
            {
                _savePath = data._path;
            }
            gameObject.transform.position = new Vector3(data._position._posX, data._position._posY, data._position._posZ);
            gameObject.transform.rotation = new Quaternion(data._position._rotX, data._position._rotY, data._position._rotZ, data._position._rotW);
            _animationStagger = data._animationStagger;

            if (data._stateNameSize > 0)
            {
                _crowdStates = new string[data._stateNameSize];

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _crowdStates[i] = data._stateNames[i];

                }
            }



            if (Application.isEditor && !EditorApplication.isPlaying)
            {
                _crowdSources = new List<GameObject>();
                if (data._parents != null)
                {
                    for (int i = 0; i < data._parents.Length; i++)
                    {
                        var _newSource = new GameObject();
                        var _newParentData = data._parents[i];

                        _newSource.name = "Crowd Source";
                        _newSource.transform.position = new Vector3(_newParentData._posX, _newParentData._posY, _newParentData._posZ);
                        _newSource.transform.rotation = new Quaternion(_newParentData._rotX, _newParentData._rotY, _newParentData._rotZ, _newParentData._rotW);

                        _crowdSources.Add(_newSource);
                    }
                }


                if (data._unassignedGroup._groupMembers != null)
                {
                    _groupUnassigned = GenerateGroupAndPlaceholders(data._unassignedGroup);
                }
                if (data._groupCount > 0)
                {
                    _crowdGroups.Clear();
                    int _currentGroupIndex = 0;

                    while (_currentGroupIndex < data._groupCount)
                    {

                        _crowdGroups.Add(GenerateGroupAndPlaceholders(data._groups[_currentGroupIndex]));
                        _currentGroupIndex++;
                    }
                }
            }
            else
            {
                if (data._groupCount > 0)
                {
                    for (int i = 0; i < data._groupCount; i++)
                    {
                        _crowdGroups.Add(new CrowdGroup(data._groups[i]));
                    }
                }
                if (data._unassignedGroup._groupMembers != null)
                {
                    _groupUnassigned = new CrowdGroup(data._unassignedGroup);
                }


            }

        }

        /// <summary>
        /// Removes all crowd members in the controller
        /// </summary>
        private void RemovePlaceholderReferences()
        {
            if (_groupUnassigned == null)
            {
                return;
            }
            if (_groupUnassigned.Size > 0)
            {
                _groupUnassigned.DestroyCrowdMembers();

            }

            if (_crowdGroups.Count > 0)
            {


                for (int i = 0; i < _crowdGroups.Count; i++)
                {
                    _crowdGroups[i].DestroyCrowdMembers();
                }
                _crowdGroups.Clear();
            }


            if (_crowdSources.Count > 0)
            {

                if (!Application.isPlaying)
                {
                    for (int i = 0; i < _crowdSources.Count; i++)
                    {
                        var _crowdSource = _crowdSources[i];

                        if (_crowdSource != null)
                        {
                            DestroyImmediate(_crowdSource);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < _crowdSources.Count; i++)
                    {
                        var _crowdSource = _crowdSources[i];

                        if (_crowdSource != null)
                        {
                            Destroy(_crowdSource);
                        }
                    }
                }

            }


        }

        /// <summary>
        /// Generates the placeholders back in, for editor
        /// </summary>
        /// <param name="groupData"> data about the gruop being generated</param>
        /// <returns></returns>
        private CrowdGroup GenerateGroupAndPlaceholders(GroupData groupData)
        {

            var _newGroup = new CrowdGroup(groupData._name);
            _newGroup.OverwriteModelData(groupData._models);

            int _memberIndex = 0;

            while (_memberIndex < groupData._groupMembers.Length)
            {
                var _newMember = GameObject.Instantiate(_placeholderPrefab);
                var _transformData = groupData._groupMembers[_memberIndex]._position;
                int _source = groupData._groupMembers[_memberIndex].source;

                _newMember.transform.position = new Vector3(_transformData._posX, _transformData._posY, _transformData._posZ);
                _newMember.transform.rotation = new Quaternion(_transformData._rotX, _transformData._rotY, _transformData._rotZ, _transformData._rotW);

                _newGroup.AddCrowdMember(_newMember);
                _crowdSources.Add(_newMember.transform.parent.gameObject);

                _memberIndex++;
            }

            return _newGroup;
        }

        /// <summary>
        /// Reads all data from the disk then overwrites 
        /// </summary>
        public void ReadAll()
        {

            if (_savePath == null)
            {
                _savePath = Application.dataPath + @"/CrowdAssetData/CrowdData - " + SceneManager.GetActiveScene().name + ".data.json";

            }

            if (File.Exists(_savePath))
            {
                var _reader = new StreamReader(_savePath);


                var _jsonFile = _reader.ReadToEnd();

                var data = JsonConvert.DeserializeObject<CrowdData>(_jsonFile);

                _reader.Dispose();

                OverWriteData(data);

                _crowdCount = RecalculateCount();
            }
        }

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
    }

}





