using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class CrowdMemberInfo : MonoBehaviour
    {
        [SerializeField]
        private GameObject _sprite, _lowDetailModel, _highDetailModel;

        [SerializeField]
        private bool _isDynamicCrowdMember = true;

        [SerializeField]
        private Team _team;

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