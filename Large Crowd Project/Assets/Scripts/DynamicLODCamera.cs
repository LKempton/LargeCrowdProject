using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class DynamicLODCamera : MonoBehaviour {
        
        [SerializeField] //the distance between the camera and the object at which the object changes level of detail
        private float highDetailModelDistance, lowDetailModelDistance, spriteDistance;

        [SerializeField] //interval at which the levels of detail of the objects are updated in milliseconds
        private float updateInterval = 0.5f;

        private Collider[] hitColliders;
        private int currentColliderIndex = 0;
        [SerializeField]
        private int maxCalculationsPerFrame = 100;

        void Start()
        {
            //repeatedly get the crowdmembers to update on an interval
            InvokeRepeating("GetCrowdMembers", 0, updateInterval);
        }

        void Update()
        {
            if (hitColliders != null)
            {
                int _maxIndex = (currentColliderIndex + maxCalculationsPerFrame > hitColliders.Length) ? hitColliders.Length - 1 : currentColliderIndex + maxCalculationsPerFrame;
                for ( ; currentColliderIndex < _maxIndex; currentColliderIndex++)
                {
                    var hitCollider = hitColliders[currentColliderIndex];

                    float distance = Mathf.Abs((hitCollider.gameObject.transform.position - transform.position).sqrMagnitude);

                    //depending on distance to the camera, set the level of detail of the crowd member
                    //distance is squared to save performance on square root calculations
                    if (distance <= (highDetailModelDistance * highDetailModelDistance))
                    {
                        if (hitCollider.gameObject.name.Substring(hitCollider.gameObject.name.Length - 1, 1) != "3")
                        {
                            SetLOD(2, hitCollider.gameObject);
                        }
                    }
                    else if (distance <= (lowDetailModelDistance * lowDetailModelDistance) && distance > (highDetailModelDistance * highDetailModelDistance))
                    {
                        if (hitCollider.gameObject.name.Substring(hitCollider.gameObject.name.Length - 1, 1) != "2")
                        {
                            SetLOD(1, hitCollider.gameObject);
                        }
                    }
                    else if (distance > (lowDetailModelDistance * lowDetailModelDistance))
                    {
                        if (hitCollider.gameObject.name.Substring(hitCollider.gameObject.name.Length - 1, 1) != "1")
                        {
                            SetLOD(0, hitCollider.gameObject);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Get the distance from the camera of all the crowd members in a radius
        /// </summary>
        private void GetCrowdMembers()
        {
            float detectionRadius = spriteDistance*2;

            //get an array of every crowd member within a radius and depending on distance set a level of detail
            hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, (1 << 8), QueryTriggerInteraction.Collide);

            currentColliderIndex = 0;
        }

        private void SetLOD(int LOD, GameObject currentObj)
        {
            var info = currentObj.GetComponent<CrowdMemberInfo>();

            if (info == null)
            {
                return;
            }

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
            var newObj = SimplifiedLODPooler.instance.GetPooledObject(newObjName);

            //set position and rotation of new model
            newObj.transform.position = currentObj.transform.position;
            newObj.transform.rotation = currentObj.transform.rotation;
            newObj.SetActive(true);

            //Disable current model
            currentObj.SetActive(false);
        }
    }
}
