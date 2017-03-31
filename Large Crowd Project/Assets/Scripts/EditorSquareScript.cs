using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSquareScript : MonoBehaviour
{
    private Transform source;
    private Transform bounds;

    private Vector3 corner1;
    private Vector3 corner2;

    [SerializeField]
    private Color selectedColour;

    public bool isCircle;

    private CrowdAI.CrowdController cc;


    void OnDrawGizmosSelected()
    {
        if (!isCircle)
        {
            if (transform.parent == null)
            {
                source = transform;
                bounds = transform.GetChild(0).transform;

                corner1 = new Vector3(bounds.position.x, bounds.position.y, source.position.z);
                corner2 = new Vector3(source.position.x, source.position.y, bounds.position.z);

                if (bounds != null && source != null)
                {
                    Gizmos.color = selectedColour;

                    Gizmos.DrawLine(source.position, corner1);
                    Gizmos.DrawLine(corner2, bounds.position);

                    Gizmos.DrawLine(source.position, corner2);
                    Gizmos.DrawLine(corner1, bounds.position);

                    Gizmos.DrawLine(source.position, bounds.position);
                }
            }
            else
            {
                bounds = transform;
                source = transform.parent;

                corner1 = new Vector3(bounds.position.x, bounds.position.y, source.position.z);
                corner2 = new Vector3(source.position.x, source.position.y, bounds.position.z);

                if (bounds != null && source != null)
                {
                    Gizmos.color = selectedColour;

                    Gizmos.DrawLine(source.position, corner1);
                    Gizmos.DrawLine(corner2, bounds.position);

                    Gizmos.DrawLine(source.position, corner2);
                    Gizmos.DrawLine(corner1, bounds.position);

                    Gizmos.DrawLine(source.position, bounds.position);
                }
            }
        }
        else if (isCircle)
        {
            if (transform.parent == null)
            {
                source = transform;
                bounds = transform.GetChild(0).transform;

                Vector3 midPoint = (source.position + bounds.position) / 2;

                if (bounds != null && source != null)
                {
                    UnityEditor.Handles.color = selectedColour;
                    UnityEditor.Handles.DrawWireDisc(midPoint, Vector3.up, bounds.position.magnitude / 3);
                }
            }
            else
            {
                bounds = transform;
                source = transform.parent;

                Vector3 midPoint = (source.position + bounds.position) / 2;

                if (bounds != null && source != null)
                {
                    UnityEditor.Handles.color = selectedColour;
                    UnityEditor.Handles.DrawWireDisc(midPoint, Vector3.up, bounds.position.magnitude / 3);
                }
            }
        }
    }
}
