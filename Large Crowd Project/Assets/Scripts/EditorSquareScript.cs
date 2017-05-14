using UnityEngine;

namespace CrowdAI
{


    /// <summary>
    ///  Draws guidelines to show where crowd members will be created
    /// </summary>
    public class EditorSquareScript : MonoBehaviour
    {
      
        private Transform source;
        private Transform bounds;

        private Vector3 corner1;
        private Vector3 corner2;

        [SerializeField]
        private Color selectedColour;

        [HideInInspector]
        public bool isCircle;

        private CrowdController cc;

        /// <summary>
        /// Draws the crowd creation outline in the scene in either the circle or square shape
        /// </summary>
        void OnDrawGizmosSelected()
        {
            if (!isCircle)
            {
                if (transform.parent == null)
                { //uses 2 corners to draw a square
                    source = transform;
                    bounds = transform.GetChild(0).transform;

                    corner1 = new Vector3(bounds.position.x, source.position.y, source.position.z);
                    corner2 = new Vector3(source.position.x, bounds.position.y, bounds.position.z);

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

                    corner1 = new Vector3(bounds.position.x, source.position.y, source.position.z);
                    corner2 = new Vector3(source.position.x, bounds.position.y, bounds.position.z);

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
                        UnityEditor.Handles.DrawWireDisc(midPoint, Vector3.up, bounds.localPosition.magnitude / 2.25f);
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
                        UnityEditor.Handles.DrawWireDisc(midPoint, Vector3.up, bounds.localPosition.magnitude / 2.25f);
                    }
                }
            }
        }
    }
}
