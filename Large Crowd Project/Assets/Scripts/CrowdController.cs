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

        float animationStagger = 0.25f;

        // crowd gen parameters
        [SerializeField]
        private int _rows, _columns;
        [SerializeField]
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;
        [SerializeField]
        private GameObject _crowdObject;
        [SerializeField]
        bool _randomGroupDist = true;

        

        // Update is called once per frame
        void Start()
        {

            var _generator = new CrowdGeneration(_rows,_columns,_minOffset,_maxOffset,_tiltAmount,_startHeight,_crowdObject);


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

            var _crowdMembers = _generator.GenerateCrowd(_crowdFormation, gameObject, _crowdGroups, _randomGroupDist);

        }



    }
}