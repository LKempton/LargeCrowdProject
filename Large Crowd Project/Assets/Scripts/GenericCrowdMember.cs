using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class GenericCrowdMember : MonoBehaviour, ICrowd
    {
        private Dictionary<string, AnimationClip> _animDict;
        private Renderer _rend;
        private Animation _animator;

        [SerializeField]
        [Range(0, 1)]
        private float _minStartDelay = 0, _maxStartDelay = 0.4f;

        [SerializeField]
        private AnimationClip[] _stateAnimClips;
        [SerializeField]
        private string[] _animStateNames;

        private string currentState;

        private void Start()
        {
            _animDict = new Dictionary<string, AnimationClip>();

            
            
            _rend = gameObject.GetComponent<Renderer>();
            _animator = GetComponent<Animation>();

            if (!_animator)
                _animator = GetComponentInChildren<Animation>();

            if (!_rend)
                _rend = GetComponentInChildren<Renderer>();

            var _crowdController = GetComponentInParent<CrowdController>();

           
            for (int i = 0; i < _animStateNames.Length; i++)
            {
                if (_crowdController.StateExists(_animStateNames[i]))
                    _animDict.Add(_animStateNames[i], _stateAnimClips[i]);
                else
                    Debug.LogError("State: " + _animStateNames[i] + " Does not exist in CrowdController Class");
            }
            _animator.playAutomatically = false;
            SetState(_animStateNames[0], true);
        }





        public void ToggleRenderer()
        {
            _rend.enabled = !_rend.enabled;
        }

        public string GetCurrentState()
        {
            return currentState;
        }

        public bool SetState(string state, bool useRandDelay)
        {
            AnimationClip _animState = new AnimationClip();

            bool _isStateSet = _animDict.TryGetValue(state, out _animState);

            if (_isStateSet)
            {
               
                if (useRandDelay)
                {
                    float _delay = Random.Range(_minStartDelay, _maxStartDelay);
                    StartCoroutine(StartStateDelayed(_delay, _animState));
                }
                else
                {
                   
                    _animator.Stop();
                    _animator.clip = _animState;
                    _animator.Play();
                }
                currentState = state;
            }

            return _isStateSet;
        }

        public bool SetState(string state, float delay)
        {
            AnimationClip _animState = new AnimationClip();

            bool _isStateSet = _animDict.TryGetValue(state, out _animState);

            if (_isStateSet)
            {
              
                StartCoroutine(StartStateDelayed(delay, _animState));
                currentState = state;
            }

            return _isStateSet;
        }

        IEnumerator StartStateDelayed(float delay, AnimationClip state)
        {
            yield return new WaitForSeconds(delay);
           
            _animator.Stop();
            _animator.clip = state;
            _animator.Play();
        }
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
