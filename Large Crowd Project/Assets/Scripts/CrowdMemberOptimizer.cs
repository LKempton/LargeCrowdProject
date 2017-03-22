using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberOptimizer : MonoBehaviour {

    private Renderer render;

    private float distanceToCam;

    void Start()
    {
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        distanceToCam = CalculateDistance();
        Debug.Log(distanceToCam);

        //!distance boundaries to change!
        if (distanceToCam <= 15f)
        {
            //set high detail model
        }
        else if (distanceToCam > 15 && distanceToCam <= 50)
        {
            //set medium detail model
        }
        else if (distanceToCam > 50)
        {
            //set far away model
        }
    }

	void OnBecameVisible()
    {
        //render.enabled = true;
        //Debug.Log("MeshOn");
    }

    void OnBecameInvisible()
    {
        //render.enabled = false;
        //Debug.Log("Mesh gone");
    }

    private float CalculateDistance()
    {
        var camera = Camera.main;
        var heading = transform.position - camera.transform.position;
        float distance = Vector3.Dot(heading, camera.transform.forward);

        return distance;
    }
}
