using UnityEngine;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class CrowdCleaner : MonoBehaviour
    {
        CrowdController _controller;

        public CrowdCleaner(CrowdController controller)
        {
            _controller = controller;
        }

        void OnDestroy()
        {
            _controller.RemoveMembers(gameObject);
        }

    }

}
