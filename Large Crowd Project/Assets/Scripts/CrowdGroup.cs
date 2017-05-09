using UnityEngine;
using System.Collections.Generic;
using System;



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
            
            if (data._groupMembers != null)
            {
                var _parentGameObject = new GameObject();
                _parentGameObject.name = GroupName + "_Source";
                for (int i = 0; i < data._groupMembers.Length; i++)
                {

                    var _newCrowdMember = MakeCrowdPosition(data._groupMembers[i]._position);
                    _newCrowdMember.transform.parent = _parentGameObject.transform;
                    _crowdMembers.Add(_newCrowdMember);
                }
            }

        }

       
        public void DestroyCrowdMembers()
        {
            if (Application.isEditor)
            {
                for (int i = _crowdMembers.Count - 1; i > -1; i--)
                {
                    if (_crowdMembers[i] != null)
                    {
                        GameObject.DestroyImmediate(_crowdMembers[i]);
                    }
                    _crowdMembers.RemoveAt(i);
                }
            }
            else
            {
                
                for (int i = _crowdMembers.Count - 1; i > -1; i--)
                {
                    if (_crowdMembers[i] != null)
                    {
                        GameObject.Destroy(_crowdMembers[i]);
                    }
                    _crowdMembers.RemoveAt(i);
                }
            }
            _crowdMembers.Clear();
            
        }

        public void OverwriteModelData(ModelData[] data)
        {

            if (data == null)
            {
                return;
            }

            if (data.Length < 1)
            {
                return;
            }

            for (int i = 0; i <data.Length ; i++)
            {
                _models.Add(GetModelFromData(data[i]));
            }

        }
        private GameObject MakeCrowdPosition(TransFormData data)
        {
            var _outGO = new GameObject();
            _outGO.name = GroupName+"_Crowd Member_"+_crowdMembers.Count;

            // add components go here for any scripts that need to be on the objects
            _outGO.transform.position = new Vector3(data._posX, data._posY, data._posZ);
            _outGO.transform.rotation = new Quaternion(data._rotX, data._rotY, data._rotZ, data._rotW);

            return _outGO;
            
        }

        private void MakeCrowdPlaceholder(TransFormData data, GameObject placeholder)
        {
            var _crowdMember = GameObject.Instantiate(placeholder);

            _crowdMember.transform.position = new Vector3(data._posX, data._posY, data._posZ);
            _crowdMember.transform.rotation = new Quaternion(data._rotX, data._rotY, data._rotZ, data._rotW);
            _crowdMembers.Add(_crowdMember);

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
        
       
      private int GetParentIndex(GameObject value, List<GameObject> parents)
        {
            if (value == null || parents == null)
            {
                return -1;
            }

            for (int i = 0; i < parents.Count; i++)
            {
                if (value == parents[i])
                {
                    return i;
                }
            }

            return -1;
        }

        public GroupData GetData(List<GameObject> parents)
        {
            GroupData _outData = new GroupData();

            _outData._groupMembers = new MemberData[_crowdMembers.Count];
             
            
         
           

           

            _outData._name = _groupName;
            if (_crowdMembers != null)
            {
                if (_crowdMembers.Count > 0)
                {
                    for (int i = 0; i < _crowdMembers.Count; i++)
                    {
                        var _currentTransform = _crowdMembers[i].transform;

                        _outData._groupMembers[i]._position._posX = _currentTransform.position.x;
                        _outData._groupMembers[i]._position._posY = _currentTransform.position.y;
                        _outData._groupMembers[i]._position._posZ = _currentTransform.position.z;

                        _outData._groupMembers[i]._position._rotW = _currentTransform.rotation.w;
                        _outData._groupMembers[i]._position._rotX = _currentTransform.rotation.x;
                        _outData._groupMembers[i]._position._rotY = _currentTransform.rotation.y;
                        _outData._groupMembers[i]._position._rotZ = _currentTransform.rotation.z;

                        _outData._groupMembers[i].source = GetParentIndex(_currentTransform.parent.gameObject, parents);
                       
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

        public void AddModelGroup(ModelWrapper model)
        {
            if (!Application.isEditor)
            {
                return;
            }

            _models.Add(model);
            

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
                if (_groupName == "")
                {
                    return "Unassigned";
                }
                return _groupName;
            }
            set
            {
                if (_groupName != "Unassigned" )
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
