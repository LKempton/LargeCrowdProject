using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class CrowdMemberOptimizer : MonoBehaviour
    {

        [SerializeField]
        private bool _dynamicCrowdModel;

        [SerializeField]
        private float _updateFrequency = 1f;

        [SerializeField]
        private Collider _collider;

        void OnBecameVisible()
        {
            if (_dynamicCrowdModel)
            {
                _collider.enabled = true;
            }
        }

        void OnBecameInvisible()
        {
            _collider.enabled = false;
        }

        //private float CalculateDistance()
        //{
        //    var camera = Camera.main;
        //    var heading = transform.position - camera.transform.position;
        //    float distance = Vector3.Dot(heading, camera.transform.forward);

        //    return distance;
        //}

        public GameObject PlaceholderObject()
        {
            return gameObject;
        }

        public bool IsStatic()
        {
            return _dynamicCrowdModel;
        }
    }
}