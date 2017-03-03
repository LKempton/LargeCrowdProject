﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    /// <summary>
    /// This Class is placed on Each crowd Member
    /// Allows Renderer to be switched off and to modify animation states
    /// </summary>
    public class GenericCrowdMember : MonoBehaviour, ICrowd
    {
        private Dictionary<string, string> _animDict; // links animation clip names to animation states
        private Renderer _rend;
        private Animation _animator;
       
        [SerializeField]
        [Range(0, 2.2f)] // range that delays the animation switch
        private float _minStartDelay = 0, _maxStartDelay = 0.4f;
        
        [SerializeField] // clips associated with the crowd member
        private AnimationClip[] _stateAnimClips;
        [SerializeField] // corresponding names and states
        private string[] _animStateNames;

        //current animation state of the model
        private string currentState;
       
        private void Start()
        {
            _animDict = new Dictionary<string,string>();

            
            
            _rend = gameObject.GetComponent<Renderer>();
            _animator = GetComponent<Animation>();
            
            // if the animator or the Rendere can't be found in the parent it must be in the child
            if (!_animator)
                _animator = GetComponentInChildren<Animation>();

            if (!_rend)
                _rend = GetComponentInChildren<Renderer>();

            var _crowdController = GetComponentInParent<CrowdController>();

           // Only links animations to states not vice versa
            for (int i = 0; i < _animStateNames.Length; i++)
            {
                // if the state name corresponds to the animation then add it to lookup

                if (_crowdController.StateExists(_animStateNames[i]))
                {
                    if (_animator.GetClip(_stateAnimClips[i].name) == null) 
                    {// adds the animation to the componenets list
                        _animator.AddClip(_stateAnimClips[i], _stateAnimClips[i].name);
                    }
                    //adds the key/value pair to a dictionary
                    _animDict.Add(_animStateNames[i], _stateAnimClips[i].name);
                }
                else
                {// if the state does not exist then it is an error
                    Debug.LogError("State: " + _animStateNames[i] + " Does not exist in CrowdController Class");
                }
            }
            // start playing the first animation 
            SetState(_animStateNames[0], true);
            
        }

        public void ToggleRenderer()
        {
            _rend.enabled = !_rend.enabled;
        }

        /// <returns> The name of the animation state playing</returns>
        public string GetCurrentState()
        {
            return currentState;
        }

        /// <summary>
        /// Attempts to find the animtation state then plays it if it exists
        /// </summary>
        /// <param name="state"> The name of the state that will be looked for</param>
        /// <param name="useRandDelay">Whether the object should use a random delay</param>
        /// <returns>True if the state has been changed</returns>
        public bool SetState(string state, bool useRandDelay)
        {
            string _animState;

            bool _isStateSet = _animDict.TryGetValue(state, out _animState);

            if (_isStateSet)
            {
               
                if (useRandDelay)
                {
                    float _delay = Random.Range(_minStartDelay, _maxStartDelay);
                    StartCoroutine(StartNextAnimation(_delay, _animState));
                }
                else
                {

                    StartCoroutine(StartNextAnimation(0, _animState));
                }
                currentState = state;
            }

            return _isStateSet;
        }

        /// <summary>
        /// Attempts to find the animation state then plays it if it exists
        /// </summary>
        /// <param name="state">  The name of the state that will be looked for</param>
        /// <param name="delay"> Time in seconds before the animation starts playing</param>
        /// <returns>True if the state has been changed</returns>
        public bool SetState(string state, float delay)
        {
            string _animState;

            bool _isStateSet = _animDict.TryGetValue(state, out _animState);

            if (_isStateSet)
            {
              
                StartCoroutine(StartNextAnimation(delay, _animState));
                currentState = state;
            }

            return _isStateSet;
        }

        /// <summary>
        /// Transistions to next animation after a delay
        /// </summary>
        /// <param name="delay"> time in seconds before start transistion</param>
        /// <param name="animName">assumed correct name of the animation clip to be played</param>
        /// <returns></returns>
        IEnumerator StartNextAnimation(float delay, string animName)
        {
           

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            if (_animator.clip == null)
            {
                _animator.Play(animName);
                yield break;
            }

            _animator.clip.wrapMode = WrapMode.Once;

         
            _animator.GetClip(animName).wrapMode = WrapMode.Loop;

            _animator.Play();

            do
            {
                yield return null;
            }
            while (_animator.isPlaying);

            _animator.CrossFade(animName);

        }

        /// <summary>
        /// Toggles the animation of this crowd member
        /// </summary>
        public void ToggleAnimation()
        {
            if (_animator.isPlaying)
            {
                _animator.Stop();
            }
            else
            {
                _animator.Play();
            }
        }

       


    }

}
