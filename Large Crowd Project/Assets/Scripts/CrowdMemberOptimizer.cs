using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberOptimizer : MonoBehaviour {

    private Renderer render;

    private float distanceToCam;

    [SerializeField]
    private float highLODDistance;
    [SerializeField]
    private float midLODDistance;
    [SerializeField]
    private float lowLODDistance;


    void Start()
    {
        render = GetComponent<Renderer>();
    }

    private void UpdateLOD()
    {
        distanceToCam = CalculateDistance();
        Debug.Log(distanceToCam);

        //!distance boundaries to change!
        if (distanceToCam <= highLODDistance)
        {
            //set high detail model
        }
        else if (distanceToCam > highLODDistance && distanceToCam <= lowLODDistance)
        {
            //set medium detail model
        }
        else if (distanceToCam > lowLODDistance)
        {
            //set far away model
        }
    }

	void OnBecameVisible()
    {
        InvokeRepeating("UpdateLOD", 0, 0.1f);
    }

    void OnBecameInvisible()
    {
        CancelInvoke();
    }

    private float CalculateDistance()
    {
        var camera = Camera.main;
        var heading = transform.position - camera.transform.position;
        float distance = Vector3.Dot(heading, camera.transform.forward);

        return distance;
    }
}
