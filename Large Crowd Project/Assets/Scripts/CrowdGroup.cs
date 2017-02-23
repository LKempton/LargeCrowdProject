using UnityEngine;
using System.Collections.Generic;

namespace CrowdAI
{
    public class CrowdGroup
    {
        private string _groupName;
        private List<GameObject> _crowdMemebers;

         public CrowdGroup(string groupName)
        {
            _crowdMemebers = new List<GameObject>();

            _groupName = groupName;
        }

        public CrowdGroup(string groupName, GameObject[] crowdMemebers)
        {
            _groupName = groupName;

            _crowdMemebers = new List<GameObject>();

            for (int i = 0; i < crowdMemebers.Length; i++)
            {
                _crowdMemebers.Add(crowdMemebers[i]);
            }

        }

    }

    
}
