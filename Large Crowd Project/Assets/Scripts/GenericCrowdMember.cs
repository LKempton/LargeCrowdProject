using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class GenericCrowdMember : MonoBehaviour,ICrowd
    {
        private Dictionary<string, AnimationClip> _animDict;
        private Renderer _rend;
        private Animator _animator;

        [SerializeField]
        private AnimationClip[] _stateAnimClips;


        private void Start()
        {
            

            _animDict = new Dictionary<string, AnimationClip>();
           

            _rend = gameObject.GetComponent<Renderer>();
            _animator = GetComponent<Animator>();

            if (!_animator)
                _animator = GetComponentInChildren<Animator>();

            if (!_rend)            
                _rend = GetComponentInChildren<Renderer>();

            string[] states = GetComponentInParent<CrowdController>().GetCrowdStates();

            for (int i = 0; i < states.Length; i++)
            {

            }
        }
        
      

        public bool LoopAnimation
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void ToggleRenderer()
        {
            
        }

        public string GetState()
        {
            throw new NotImplementedException();
        }

        public void SetState(string State)
        {
            throw new NotImplementedException();
        }

        public void StartAnimation()
        {
            throw new NotImplementedException();
        }

        public void StartAnimations(float delay)
        {
            throw new NotImplementedException();
        }

        public void StopAnimation()
        {
            throw new NotImplementedException();
        }

        public void DisableRenderer()
        {
            throw new NotImplementedException();
        }
    }

}
