using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    public class CrowdMemberOptimizer : MonoBehaviour, ICrowdPosition
    {

        //private float distanceToCam;

        [SerializeField]
        private bool _dynamicCrowdModel;

        [SerializeField]
        private float _updateFrequency = 1f;

        [SerializeField]
        private Collider _collider;

        //[SerializeField]
        //private float _veryHighLODDistance;
        //[SerializeField]
        //private float _highLODDistance;
        //[SerializeField]
        //private float _midLODDistance;
        //[SerializeField]
        //private float _lowLODDistance;
        //[SerializeField]
        //private float _veryLowLODDistance;

        //[SerializeField]
        //private GameObject[] LODModels;

        //private int currentLODNumber;

        void OnBecameVisible()
        {
            _collider.enabled = true;
        }

        void OnBecameInvisible()
        {
            _collider.enabled = false;
        }

        /// <summary>
        /// Gets the distance of the crowdmember from the camera
        /// </summary>
        //private void UpdateLOD()
        //{
        //    distanceToCam = CalculateDistance();
        //    Debug.Log(distanceToCam);

        //    int newLODNumber = 0;

        //    if (distanceToCam <= _veryHighLODDistance)
        //    {
        //        newLODNumber = 4;
        //    }
        //    else if (distanceToCam > _veryHighLODDistance && distanceToCam <= _highLODDistance)
        //    {
        //        newLODNumber = 3;
        //    }
        //    else if (distanceToCam > _highLODDistance && distanceToCam <= _midLODDistance)
        //    {
        //        newLODNumber = 2;
        //    }
        //    else if (distanceToCam > _midLODDistance && distanceToCam <= _lowLODDistance)
        //    {
        //        newLODNumber = 1;
        //    }
        //    else if (distanceToCam > _lowLODDistance)
        //    {
        //        newLODNumber = 0;
        //    }

        //    if (newLODNumber != currentLODNumber)
        //    {
        //        SetNewLOD(newLODNumber);
        //    }
        //}

        //void OnBecameVisible()
        //{
        //    if (_dynamicCrowdModel)
        //    {
        //        InvokeRepeating("UpdateLOD", 0, _updateFrequency);
        //    }
        //}

        //void OnBecameInvisible()
        //{
        //    if (_dynamicCrowdModel)
        //    {
        //        CancelInvoke("UpdateLOD");
        //    }
        //}

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