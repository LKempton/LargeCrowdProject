using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class DynamicLODCamera : MonoBehaviour {

        private SimplifiedCrowdController scc;

        [SerializeField] //the distance between the camera and the object at which the object changes level of detail
        private float highDetailModelDistance, lowDetailModelDistance, spriteDistance;

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
            float detectionRadius = spriteDistance*2;

            //get an array of every crowd member within a radius and depending on distance set a level of detail
            Collider[] hitColliders = Physics.OverlapSphere(centre, detectionRadius, scc.CrowdMemberLayer, QueryTriggerInteraction.Collide);
            if (hitColliders == null)
            {
                return;
            }

            for (int i = 0; i < hitColliders.Length; i++)
            {
                float distance = Mathf.Abs((hitColliders[i].gameObject.transform.position - centre).sqrMagnitude);

                //depending on distance to the camera, set the level of detail of the crowd member
                //distance is squared to save performance on square root calculations
                if (distance <= (highDetailModelDistance * highDetailModelDistance))
                {
                    SetLOD(2, hitColliders[i].gameObject);
                }
                else if (distance <= (lowDetailModelDistance * lowDetailModelDistance) && distance > (highDetailModelDistance * highDetailModelDistance))
                {
                    SetLOD(1, hitColliders[i].gameObject);
                }
                else //if (distance <= (spriteDistance * spriteDistance) && distance > (lowDetailModelDistance * lowDetailModelDistance))
                {
                    SetLOD(0, hitColliders[i].gameObject);
                }
            }
        }

        private void SetLOD(int LOD, GameObject currentObj)
        {
            var info = currentObj.GetComponent<CrowdMemberInfo>();

            string newObjName;

            //get the model to switch to
            switch (LOD)
            {
                case 0:
                    newObjName = info.Team + "_1_1";

                    break;
                case 1:
                    newObjName = info.Team + "_1_2";
                    break;
                case 2:
                    newObjName = info.Team + "_1_3";
                    break;
                default:
                    newObjName = null;
                    Debug.LogError("LOD game object set to null");
                    break;

            }

            var newObj = scc.PoolManager.GetPooledObject(newObjName);

            //set position and rotation of new model
            newObj.transform.position = currentObj.transform.position;
            newObj.transform.rotation = currentObj.transform.rotation;
            newObj.SetActive(true);

            //Disable current model
            currentObj.SetActive(false);
        }
    }
}
