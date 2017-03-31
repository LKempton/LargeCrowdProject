using UnityEngine;

namespace CrowdAI
{
    [ExecuteInEditMode]
    public class CrowdCleaner : MonoBehaviour
    {
        CrowdController _controller;

        

        void OnDestroy()
        {
            try
            {
                _controller.RemoveMembers(gameObject);
            }
            catch (System.Exception)
            {

                Debug.LogError("Reference lost from undoing ");
            }

            
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
