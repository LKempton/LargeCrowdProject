using UnityEngine;
using System.Collections.Generic;
using System;

namespace CrowdAI
{
    public class CrowdGroup
    {
        private string _groupName;
        private List<ICrowd> _crowdMemebers;

        

         public CrowdGroup(string groupName)
        {
            _crowdMemebers = new List<ICrowd>();

            _groupName = groupName;
        }

        public CrowdGroup(string groupName, GameObject[] crowdMemebers)
        {
            _groupName = groupName;

            _crowdMemebers = new List<ICrowd>();

            for (int i = 0; i < crowdMemebers.Length; i++)
            {
                _crowdMemebers.Add(crowdMemebers[i].GetComponent<ICrowd>());
            }

        }

        public void AddCrowdMember(GameObject crowdMember)
        {
            _crowdMemebers.Add(crowdMember.GetComponent<ICrowd>());
        }

        public void AddCrowdMember(ICrowd crowdMeber)
        {
            _crowdMemebers.Add(crowdMeber);
        }

        public void AddCrowdMember(object crowdMember)
        {
            //overloaded because Instansiate function returns an object

            var _goCrowdMember = (GameObject)crowdMember;

                _crowdMemebers.Add(_goCrowdMember.GetComponent<ICrowd>());
            
        }

        public void ClearAll()
        {
            _crowdMemebers.Clear();
        }


    }

    
}
