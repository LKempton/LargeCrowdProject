using UnityEngine;
using System.Collections.Generic;




namespace CrowdAI
{
    /// <summary>
    /// Group of crowd members abritrarily seperated 
    /// </summary>
    public class CrowdGroup
    {
        [SerializeField]
        private string _groupName;
        [SerializeField]
        private List<GameObject> _crowdMembers;
        private List<ModelWrapper> _models;
      
        /// <summary>
        /// Constructs a new crowd group
        /// </summary>
        /// <param name="groupName">The name of the group</param>

        public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<GameObject>();
            _models = new List<ModelWrapper>();
            _groupName = groupName;
        }

        /// <summary>
        /// Constructs a new crowd group
        /// </summary>
        /// <param name="data">group object data</param>
        /// <param name="sources">list of crowd sources in the scene</param>
        /// <param name="prefab">crowd member to generate group for</param>
        public CrowdGroup(GroupData data, List<GameObject> sources, GameObject prefab)
        {
            _groupName = data._name;

            _crowdMembers = new List<GameObject>();
            _models = new List<ModelWrapper>();

            //instantiate and initialise crowd members inside new group
            if (data._groupMembers != null)
            {
                for (int i = 0; i < data._groupMembers.Length; i++)
                {
                    var _newMember = GameObject.Instantiate(prefab);
                    var _memberData = data._groupMembers[i];

                    _newMember.transform.position = IOHandler.GetPosition(_memberData._transform);
                    _newMember.transform.rotation = IOHandler.GetRotation(_memberData._transform);

                    if (_memberData.source > 0 && _memberData.source < sources.Count)
                    {
                        _newMember.transform.parent = sources[_memberData.source].transform;
                    }

                    _crowdMembers.Add(_newMember);
                }
            }
            
            if (data._models != null)
            {
                // NEEDS TO BE TESTED
                for (int i = 0; i < data._models.Length; i++)
                {
                    int _length = data._models[i]._modelNames.Length;

                    var _models = new GameObject[_length];

                    for (int j = 0; j < _length; j++)
                    {
                        var _modelName = data._models[i]._modelNames[j];
                        //_models[i] = Resources.Load()
                    }
                }
            }
        }

        /// <summary>
        /// Removes null crowd members from list
        /// </summary>
        public void CheckForNullMembers()
        {
            for (int i = _crowdMembers.Count; i > -1; i--)
            {
                var _member = _crowdMembers[i];

                if (_member == null)
                {
                    _crowdMembers.Remove(_member);
                }
            }
        }

        /// <summary>
        /// Destroys crowd members in list
        /// </summary>
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

        /// <summary>
        /// Adds a crowd member to the group
        /// </summary>
        /// <param name="crowdMember"> the crowd member game object</param>
        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMembers.Add(crowdMember);
        }

        /// <summary>
        /// Adds crowd members objects to crowd member list
        /// </summary>
        /// <param name="crowdMembers">array of crowd members to add to list</param>
        public void AddCrowdMember(GameObject[] crowdMembers)
        {
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                _crowdMembers.Add(crowdMembers[i]);
            }
        }

        /// <summary>
        /// Get all the data in this instance of the class
        /// </summary>
        /// <param name="parents">A list of the crowd source objects in the scene</param>
        /// <returns>The data in this instance of the class</returns>
        public GroupData GetData(List<GameObject> parents)
        {
            var _outData = new GroupData();

            _outData._name = _groupName;
            // save crowd members
            if (_crowdMembers.Count > 0)
            {
                _outData._groupMembers = new MemberData[_crowdMembers.Count];

                for (int i = 0; i < _crowdMembers.Count; i++)
                {
                    var _member = _crowdMembers[i];

                    var _memberData = new MemberData();

                    _memberData.source = -1;

                    for (int j = 0; j < parents.Count; j++)
                    {
                        if (_member.transform.parent.gameObject == parents[j])
                        {
                            Debug.Log("found the source");
                            _memberData.source = j;
                            break;
                        }
                    }

                    _memberData._transform = IOHandler.GetTransformData(_member.transform);

                    
                }

            }

            if (_models.Count > 0)
            {
                _outData._models = new ModelData[_models.Count];

                for (int i = 0; i < _models.Count; i++)
                {
                    for (int j = 0; j <_models[i]._LODLevel.Length ; j++)
                    {
                        //_outData._models[i]._modelNames[j] = _models[i]._LODLevel[j];
                       // Resources.Lo
                    }
                }

            }

            return _outData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
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
        /// <returns>An array of all the members in the group</returns>
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
