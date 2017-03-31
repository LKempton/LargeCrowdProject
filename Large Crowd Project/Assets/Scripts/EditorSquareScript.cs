using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSquareScript : MonoBehaviour {

    private Transform source;
    private Transform bounds;

    [SerializeField]
    private Color selectedColour;

    void OnDrawGizmosSelected()
    {
        if (transform.parent == null)
        {
            source = transform;
            bounds = transform.GetChild(0).transform;

            if (bounds != null && source != null)
            {
                Gizmos.color = selectedColour;

                Gizmos.DrawLine(source.position, new Vector3(source.position.x, source.position.y, bounds.position.z));
                Gizmos.DrawLine(source.position, new Vector3(bounds.position.x, source.position.y, source.position.z));

                Gizmos.DrawLine(bounds.position, new Vector3(bounds.position.x, bounds.position.y, source.position.z));
                Gizmos.DrawLine(bounds.position, new Vector3(source.position.x, bounds.position.y, bounds.position.z));
            }
        }
        else
        {
            bounds = transform;
            source = transform.parent;

                        if (bounds != null && source != null)
            {
                Gizmos.color = selectedColour;

                Gizmos.DrawLine(source.position, new Vector3(source.position.x, source.position.y, bounds.position.z));
                Gizmos.DrawLine(source.position, new Vector3(bounds.position.x, source.position.y, source.position.z));

                Gizmos.DrawLine(bounds.position, new Vector3(bounds.position.x, bounds.position.y, source.position.z));
                Gizmos.DrawLine(bounds.position, new Vector3(source.position.x, bounds.position.y, bounds.position.z));
            }
        }

    }
}
