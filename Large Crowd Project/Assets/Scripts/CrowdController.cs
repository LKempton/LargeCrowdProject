using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class CrowdController : MonoBehaviour
    {

        [SerializeField]
        private string[] _crowdStates;
        private CrowdGroup[] _crowdGroups;


        // Update is called once per frame
        void Start()
        {

            if (_crowdStates == null)
            {
                _crowdGroups = new CrowdGroup[1];
                _crowdGroups[0] = new CrowdGroup("default");
            }
            else
            {
                _crowdGroups = new CrowdGroup[_crowdStates.Length];

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _crowdGroups[i] = new CrowdGroup(_crowdStates[i]);
                }
            }
        }



    }
}