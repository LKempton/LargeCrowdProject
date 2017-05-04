using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLODCamera : MonoBehaviour {

    [SerializeField]
    private Collider veryLowLODCollider, lowLODCollider, midLODCollider, highLODCollider, veryHighLODCollider;

    [SerializeField]
    private float updateInterval = 0.5f;

    void Start()
    {
        InvokeRepeating("GetCrowdMembers", 0, updateInterval);
    }

    private void GetCrowdMembers()
    {

    }
}
