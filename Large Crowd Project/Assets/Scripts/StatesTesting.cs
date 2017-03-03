using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    /// <summary>
    /// This class is used to test the state system of crowds
    /// Left Mouse and right mouse change the state in a linear fashion
    /// </summary>
    public class StatesTesting : MonoBehaviour
    {
        private CrowdController _crowdController;
        private string[] states;
        int _cState = 0;

        // Use this for initialization
        void Start()
        {
            _crowdController = GetComponent<CrowdController>();
            states = _crowdController.GetCrowdStates();
        }

        // Update is called once per frame
        void Update()
        {
           

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _cState++;
                if (_cState >= states.Length)
                    _cState = 0;

                _crowdController.SetState(states[_cState],true);
                print("Changed to : "+states[_cState]+" state");
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _cState--;
                if (_cState < 0)
                    _cState = states.Length - 1;

                _crowdController.SetState(states[_cState],true);
                print("Changed to : "+states[_cState]+" state");

            }
        }
    }
}
