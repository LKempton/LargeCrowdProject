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
        [SerializeField]
        CrowdFormation _crowdFormation;

        // crowd gen parameters
        [SerializeField]
        private int _rows, _columns;
        [SerializeField]
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;
        [SerializeField]
        private GameObject _crowdObject;


        // Update is called once per frame
        void Start()
        {
            CrowdGeneration generator = new CrowdGeneration();
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