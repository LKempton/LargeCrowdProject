using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class DynamicLODCamera : MonoBehaviour {

        [SerializeField] //the distance between the camera and the object at which the object changes level of detail
        private float veryHighLODDistance, highLODDistance, midLODDistance, lowLODDistance, veryLowLODDistance;

        [SerializeField] //interval at which the levels of detail of the objects are updated in milliseconds
        private float updateInterval = 0.5f;

        void Start()
        {
            //repeatedly get the crowdmembers to update on an interval
            InvokeRepeating("GetCrowdMembers", 0, updateInterval);
        }

        /// <summary>
        /// Get the distance from the camera of all the crowd members in a radius
        /// </summary>
        private void GetCrowdMembers()
        {
            Vector3 centre = transform.position;
            float detectionRadius = veryLowLODDistance;

            //get an array of every crowd member within a radius and depending on distance set a level of detail
            Collider[] hitColliders = Physics.OverlapSphere(centre, detectionRadius, 9, QueryTriggerInteraction.Collide);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                float distance = Mathf.Abs((hitColliders[i].gameObject.transform.position - centre).sqrMagnitude);

                //depending on distance to the camera, set the level of detail of the crowd member
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
            //Disable previous model

            //Get new model
            

            //place model at position
        }
    }
}
