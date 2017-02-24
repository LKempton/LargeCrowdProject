using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class CrowdController : MonoBehaviour
    {

        [SerializeField]
        private  string[] _crowdStates;
        [SerializeField]
        private string[] _groupNames;

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
        void Awake()
        {

            var _generator = new CrowdGeneration(_rows,_columns,_minOffset,_maxOffset,_tiltAmount,_startHeight,_crowdObject);

       
            
            if (_groupNames == null)
            {
                _crowdGroups = new CrowdGroup[1];
                _crowdGroups[0] = new CrowdGroup("default");
            }
            else
            {
                _crowdGroups = new CrowdGroup[_groupNames.Length];

                for (int i = 0; i < _crowdStates.Length; i++)
                {
                    _crowdGroups[i] = new CrowdGroup(_groupNames[i]);
                    // can add optimisation by sorting and using binary search
                }
            }

            var _crowdMembers = _generator.GenerateCrowd(_crowdFormation, gameObject, _crowdGroups, _randomGroupDist);

            if (_randomGroupDist)
            {
               // Adds crowd members to the groups randomly
               
                for (int i = 0; i < _crowdMembers.Length; i++)
                {
                    _crowdGroups[Random.Range(0, _crowdMembers.Length - 1)].AddCrowdMember(_crowdMembers[i]);
                }

            }
            else
            {

                AddCrowdUniformly(_crowdMembers);
            }

        }

        private void AddCrowdUniformly(ICrowd[] _crowdMembers)
        { // add crowd members to the groups in a uniform manner 

            float _remainder = _crowdMembers.Length / _crowdGroups.Length;
            int _groupDiv = (int)_remainder;
            _remainder -= _groupDiv;
            float _cRemainder = 0;


            int _currentCrowdMember = 0;

            for (int i = 0; i < _crowdGroups.Length; i++)
            {
                for (int j = 0; j < _groupDiv; j++)
                {
                    _crowdGroups[i].AddCrowdMember(_crowdMembers[_currentCrowdMember]);
                    _currentCrowdMember++;
                    _cRemainder += _remainder;

                    if (_cRemainder >= 1)
                    {// helps keep each group even
                        _cRemainder -= 1;
                        _crowdGroups[i].AddCrowdMember(_crowdMembers[_currentCrowdMember]);
                        _currentCrowdMember++;
                    }
                }
            }


            while (_currentCrowdMember < _crowdMembers.Length)
            {//adds any remaining crowd members to the last group abritrarily
                _crowdGroups[_crowdGroups.Length - 1].AddCrowdMember(_crowdMembers[_currentCrowdMember]);
                _currentCrowdMember++;
            }

        }

        public string[] GetCrowdStates()
        {
            //copies array since they are passed by reference
            var _crowdStatesCopy = new string[_crowdStates.Length];

            for (int i = 0; i < _crowdStates.Length; i++)
            {
                _crowdStatesCopy[i] = _crowdStates[i];
            }
            return _crowdStatesCopy;
        }

      public  bool StateExists(string stateName)
        {
            for (int i = 0; i < _crowdStates.Length; i++)
            {
                if (stateName == _crowdStates[i])
                    return true;
            }
            return false;
        }



    }
}