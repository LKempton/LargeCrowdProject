using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class DynamicLODCamera : MonoBehaviour {

        [SerializeField]
        private Collider veryLowLODCollider, lowLODCollider, midLODCollider, highLODCollider, veryHighLODCollider;

        [SerializeField]
        private float veryHighLODDistance;
        [SerializeField]
        private float highLODDistance;
        [SerializeField]
        private float midLODDistance;
        [SerializeField]
        private float lowLODDistance;
        [SerializeField]
        private float veryLowLODDistance;

        [SerializeField]
        private float updateInterval = 0.5f;

        /// <summary>
        /// Start a function that repeats less than every frame
        /// </summary>
        void Start()
        {
            InvokeRepeating("GetCrowdMembers", 0, updateInterval);
        }

        private void GetCrowdMembers()
        {
            Vector3 centre = transform.position;
            float detectionRadius = 0;

            Collider[] hitColliders = Physics.OverlapSphere(centre, detectionRadius, 9, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                float distance = Mathf.Abs((hitColliders[i].gameObject.transform.position - centre).sqrMagnitude);

                if (distance <= (veryHighLODDistance*veryHighLODDistance))
                {
                    SetLOD(4);
                }
                else if (distance <= (highLODDistance*highLODDistance) && distance > (veryHighLODDistance*veryHighLODDistance))
                {
                    SetLOD(3);
                }
                else if (distance <= (midLODDistance*midLODDistance) && distance > (highLODDistance*highLODDistance))
                {
                    SetLOD(2);
                }
                else if (distance <= (lowLODDistance*lowLODDistance) && distance > (midLODDistance*midLODDistance))
                {
                    SetLOD(1);
                }
                else //if (distance <= (veryLowLODDistance*veryLowLODDistance) && distance > (lowLODDistance*lowLODDistance)
                {
                    SetLOD(0);
                }
            }
        }

        private void SetLOD(int LOD)
        {

        }
    }
}
