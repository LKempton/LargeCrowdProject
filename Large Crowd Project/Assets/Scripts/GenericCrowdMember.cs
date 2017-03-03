using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class GenericCrowdMember : MonoBehaviour, ICrowd
    {
        private Dictionary<string, string> _animDict;
        private Renderer _rend;
        private Animation _animator;
       
        [SerializeField]
        [Range(0, 2.2f)]
        private float _minStartDelay = 0, _maxStartDelay = 0.4f;

        [SerializeField]
        private AnimationClip[] _stateAnimClips;
        [SerializeField]
        private string[] _animStateNames;

        private string currentState;

        private void Start()
        {
            _animDict = new Dictionary<string,string>();

            
            
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
                {
                    if (_animator.GetClip(_stateAnimClips[i].name) == null)
                    {
                        _animator.AddClip(_stateAnimClips[i], _stateAnimClips[i].name);
                    }

                    _animDict.Add(_animStateNames[i], _stateAnimClips[i].name);
                }
                else
                {
                    Debug.LogError("State: " + _animStateNames[i] + " Does not exist in CrowdController Class");
                }
            }
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
