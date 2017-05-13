using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    /// <summary>
    /// (Legacy)
    /// This class is used to test the state system of crowds
    /// Left Mouse and right mouse change the state in a linear fashion
    /// </summary>
    public class StatesTesting : MonoBehaviour
    {
        private CrowdController _crowdController;
        private string[] _states;
        int _cState = 0;
        int _cGroup = 0;
        private string[] _groupNames;


        // Use this for initialization
        void Start()
        {
            _crowdController = GetComponent<CrowdController>();
            _states = _crowdController.GetCrowdStates();
            _groupNames = _crowdController.GetGroupNames();

        }

        // Update is called once per frame
        void Update()
        {
           

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _cState++;
                if (_cState >= _states.Length)
                    _cState = 0;

                _crowdController.SetState(_states[_cState],true);
                print("Changed to : "+_states[_cState]+" state");
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _cGroup++;

                if(_cGroup>= _groupNames.Length)
                {
                    _cGroup = 0;
                }

                print("selected: " + _groupNames[_cGroup] + " group");

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _crowdController.ToggleAnimations();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                _cState++;
                if (_cState >= _states.Length)
                {
                    _cState = 0;
                }
                _crowdController.SetState(_states[_cState], _groupNames[_cGroup], true);
            }
          
        }
    }
}
