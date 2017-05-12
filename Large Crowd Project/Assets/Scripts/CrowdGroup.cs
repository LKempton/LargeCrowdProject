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

        public void AddCrowdMember(GameObject[] crowdMembers)
        {
            for (int i = 0; i < crowdMembers.Length; i++)
            {
                _crowdMembers.Add(crowdMembers[i]);
            }
        }

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
                        _outData._models[i]._modelNames[j] = _models[i]._LODLevel[j].name;
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
