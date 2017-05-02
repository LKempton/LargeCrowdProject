using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class CrowdMemberOptimizer : MonoBehaviour,ICrowdPosition
    {

        private float distanceToCam;

        [SerializeField]
        private bool _dynamicCrowdModel;

        [SerializeField]
        private float _highLODDistance;
        [SerializeField]
        private float _midLODDistance;
        [SerializeField]
        private float _lowLODDistance, _updateFrequency = 0.3f;

        private void UpdateLOD()
        {
            distanceToCam = CalculateDistance();
            Debug.Log(distanceToCam);

            //!distance boundaries to change!
            if (distanceToCam <= _highLODDistance)
            {
                //set high detail model
            }
            else if (distanceToCam > _highLODDistance && distanceToCam <= _lowLODDistance)
            {
                //set medium detail model
            }
            else if (distanceToCam > _lowLODDistance)
            {
                //set far away model
            }
        }

        void OnBecameVisible()
        {
            if (_dynamicCrowdModel)
            {
                
                InvokeRepeating("UpdateLOD", 0, _updateFrequency);
            }
        }

        void OnBecameInvisible()
        {
            if (_dynamicCrowdModel)
            {
                CancelInvoke("UpdateLOD");
            }
        }

        private float CalculateDistance()
        {
            var camera = Camera.main;
            var heading = transform.position - camera.transform.position;
            float distance = Vector3.Dot(heading, camera.transform.forward);

            return distance;
        }

        public GameObject PlaceholderObject()
        {
            throw new NotImplementedException();
        }

        public bool IsStatic()
        {
            throw new NotImplementedException();
        }
    }
}