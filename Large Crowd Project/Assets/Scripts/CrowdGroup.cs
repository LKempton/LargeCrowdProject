﻿using UnityEngine;
using System.Collections.Generic;



namespace CrowdAI
{
    /// <summary>
    /// Group of crowd members abritrarily seperated 
    /// </summary>
    /// 
    [System.Serializable]
    public class CrowdGroup
    {
        [SerializeField]
        private string _groupName;
        [SerializeField]
        private List<GameObject> _crowdMembers;
        private List<ModelWrapper> _models;
       

      

      

        /// <summary>
        /// Constructs a new instance of CrowdGroup
        /// </summary>
        /// <param name="groupName">The name associated with this crowd group</param>

         public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<GameObject>();
            _models = new List<ModelWrapper>();
            _groupName = groupName;
        }

        public CrowdGroup(GroupData data)
        {
            _crowdMembers = new List<GameObject>();
            _models = new List<ModelWrapper>();
            _groupName = data._name;
            if (data._models != null)
            {
                for (int i = 0; i < data._models.Length; i++)
                {
                    var _nextModel = GetModelFromData(data._models[i]);
                    _models.Add(_nextModel);
                }
            }
            
            if (data._crowdMembers != null)
            {
                for (int i = 0; i < data._crowdMembers.Length; i++)
                {
                    var _newCrowdMember = MakeCrowdPlaceholder(data._crowdMembers[i]);
                    _crowdMembers.Add(_newCrowdMember);
                }
            }

        }
        public void DestroyCrowdMembers()
        {
            for (int i = _crowdMembers.Count; i > -1; i--)
            {
                if (_crowdMembers[i] != null)
                {
                    GameObject.Destroy(_crowdMembers[i]);
                }
                _crowdMembers.RemoveAt(i);
            }
        }

        public void OverwriteModelData(ModelData[] data)
        {
            if (data.Length < 1 | !Application.isEditor)
            {
                return;
            }
            _models.Clear();

            for (int i = 0; i <data.Length ; i++)
            {
                _models.Add(GetModelFromData(data[i]));
            }

        }
        private GameObject MakeCrowdPlaceholder(TransFormData data)
        {
            var _outGO = GameObject.Instantiate(new GameObject());
            // add components go here for any scripts that need to be on the objects
            _outGO.transform.position = new Vector3(data._posX, data._posY, data._posZ);
            _outGO.transform.rotation = new Quaternion(data._rotX, data._rotY, data._rotZ, data._rotW);

            return _outGO;
            
        }
        private ModelWrapper GetModelFromData(ModelData data)
        {
            int _length = data._modelNames.Length;

            var _outModel = new ModelWrapper();

            _outModel._LODLevel = new GameObject[_length];
            _outModel.sizes = new int[_length];

            for (int i = 0; i < _length; i++)
            {
                _outModel._LODLevel[i] = (GameObject)Resources.Load(data._modelNames[i], typeof (GameObject));
                _outModel.sizes[i] = data._sizes[i];
            }

            return _outModel;
        }

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMember"> the crowd member game object</param>
        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMembers.Add(crowdMember);
        }
       
        public void AddCrowdMember(GameObject[] crowdMembers)
        {
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                _crowdMembers.Add(crowdMembers[i]);
            }
        }
        
      
        public GroupData GetData()
        {
            GroupData _outData = new GroupData();

            _outData._crowdMembers = new TransFormData[_crowdMembers.Count];
             
           

            _outData._name = _groupName;
            if (_crowdMembers != null)
            {
                if (_crowdMembers.Count > 0)
                {
                    for (int i = 0; i < _crowdMembers.Count; i++)
                    {
                        var _currentTransform = _crowdMembers[i].transform;

                        _outData._crowdMembers[i]._posX = _currentTransform.position.x;
                        _outData._crowdMembers[i]._posY = _currentTransform.position.y;
                        _outData._crowdMembers[i]._posZ = _currentTransform.position.z;

                        _outData._crowdMembers[i]._rotW = _currentTransform.rotation.w;
                        _outData._crowdMembers[i]._rotX = _currentTransform.rotation.x;
                        _outData._crowdMembers[i]._rotY = _currentTransform.rotation.y;
                        _outData._crowdMembers[i]._rotZ = _currentTransform.rotation.z;

                    }
                }
            }
           

            if (_models !=null)
            {
                if (_models.Count > 0)
                {
                    _outData._models = new ModelData[_models.Count];

                    for (int i = 0; i < _models.Count; i++)
                    {
                        var _currentModel = _models[i];
                        int _LODCount = _currentModel.sizes.Length;

                        _outData._models[i]._modelNames = new string[_LODCount];
                        _outData._models[i]._sizes = _models[i].sizes;

                        for (int j = 0; j < _LODCount; j++)
                        {
                            _outData._models[i]._modelNames[j] = _models[i]._LODLevel[j].name;

                        }
                    }
                }
                
            }
           

            return _outData;
        }

        public bool AddModelGroup(ModelWrapper model)
        {
            if (!Application.isEditor)
            {
                return false;
            }

            _models.Add(model);
            return true;

        }

        /// <summary>
        /// Sets the state of all crowd members in the gruop
        /// </summary>
        /// <param name="state">Name of the new state</param>
        /// <param name="useRandDelay"> whether there should be a delay before the animation starts </param>
        public void SetState(string state, bool useRandDelay)
        {
            
        }

        public void ToggleAnimations()
        {

        }

       /// <summary>
       /// Removes all crowd Members from the group
       /// Does not dispose of them !
       /// </summary>

        public GameObject[] ClearAllForDeletion()
        {
            if (_crowdMembers == null)
            {
                return null;
            }

            var _groupMembers = _crowdMembers.ToArray();

            _crowdMembers.Clear();

            var _outGOs = new GameObject[_groupMembers.Length];
            for (int i = 0; i < _groupMembers.Length; i++)
            {
                _outGOs[i] = _groupMembers[i];
            }

            return _outGOs;
        }
        
       

        public bool Remove(GameObject crowdMember)
        {
            if (crowdMember == null)
            {
                return false;
            }

            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                if (_crowdMembers[i] == crowdMember)
                {
                    _crowdMembers.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }


        public bool Contains(GameObject crowdMember)
        {
            return _crowdMembers.Contains(crowdMember);
        }

       

        /// <summary>
        /// The name of the group
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                if (_groupName != "Unassigned")
                {
                    _groupName = value;
                }
            }
        }

        public int Size
        {
            get
            {
                if (_crowdMembers == null)
                {
                    return 0;
                }
                else
                {
                    return _crowdMembers.Count;
                }
            }
        }

      
    }

    
}
