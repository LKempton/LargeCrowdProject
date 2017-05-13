using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class CrowdMemberInfo : MonoBehaviour
    {
        Collider collider;

        [SerializeField]
        private GameObject _sprite, _lowDetailModel, _highDetailModel;

        [SerializeField]
        private bool _isDynamicCrowdMember = true;

        
       
        private Animation _animation;
        [SerializeField]
        private string[] _clipNames;


        [SerializeField]
        private Team _team;

        void Start()
        {
            collider = GetComponent<Collider>();
            _animation = GetComponent<Animation>();

            if (_animation != null)
            {
                AnimationManager._instance.AddTeamMember(this);
            }
            

            

          
            
        }

        void OnBecameVisible()
        {
            collider.enabled = true;
        }

        void OnBecameInvisible()
        {
            collider.enabled = false;
        }

        public void TryChangeState(int stateIndex)
        {
            if (_clipNames.Length < stateIndex)
            {
                _animation.Play(_clipNames[stateIndex], PlayMode.StopAll);
            }
        }

       
        

        public GameObject Sprite
        {
            get
            {
                return _sprite;
            }
        }

        public GameObject LowDetailModel
        {
            get
            {
                return _lowDetailModel;
            }
        }

        public GameObject HighDetailModel
        {
            get
            {
                return _highDetailModel;
            }
        }

        public bool DynamicCrowdMemberToggle
        {
            get
            {
                return _isDynamicCrowdMember;
            }
            set
            {
                _isDynamicCrowdMember = value;
            }
        }

        public Team Team
        {
            get
            {
                return _team;
            }
            set
            {
                _team = value;
            }
        }
    }
}