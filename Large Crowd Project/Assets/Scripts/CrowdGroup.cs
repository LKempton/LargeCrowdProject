using UnityEngine;
using System.Collections.Generic;
using System;

namespace CrowdAI
{
    public class CrowdGroup
    {
        private string _groupName;
        private List<ICrowd> _crowdMembers;

        

         public CrowdGroup(string groupName)
        {
            _crowdMembers = new List<ICrowd>();

            _groupName = groupName;
        }

        public CrowdGroup(string groupName, GameObject[] crowdMemebers)
        {
            _groupName = groupName;

            _crowdMembers = new List<ICrowd>();

            for (int i = 0; i < crowdMemebers.Length; i++)
            {
                _crowdMembers.Add(crowdMemebers[i].GetComponent<ICrowd>());
            }

        }

        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMembers.Add(crowdMember.GetComponent<ICrowd>());
        }

        public void AddCrowdMember(ICrowd crowdMeber)
        {
            _crowdMembers.Add(crowdMeber);
        }

        public void AddCrowdMember(object crowdMember)
        {
            //overloaded because Instansiate function returns an object

            var _goCrowdMember = (GameObject)crowdMember;

                _crowdMembers.Add(_goCrowdMember.GetComponent<ICrowd>());
            
        }

        public void SetState(string state)
        {
            for (int i = 0; i < _crowdMembers.Count; i++)
            {
                _crowdMembers[i].SetState(state, false);
            }
        }

        public void ClearAll()
        {
            _crowdMembers.Clear();
        }


    }

    
}
