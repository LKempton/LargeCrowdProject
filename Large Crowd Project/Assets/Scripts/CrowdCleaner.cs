using UnityEngine;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class CrowdCleaner : MonoBehaviour
    {
        CrowdController _controller;

        

        void OnDestroy()
        {
            _controller.RemoveMembers(gameObject);
        }

        public CrowdController Controller
        {
            set
            {
                _controller = value;
            }
        }

    }

}
