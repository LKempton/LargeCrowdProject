using UnityEngine;
using System.Collections;
namespace CrowdAI
{
    public class CrowdMaterialGeneration : MonoBehaviour
    {

        [SerializeField]
        private Material[] _material = new Material[1];

        void Start()
        {
            var rng = Random.Range(0, _material.Length - 1);
            gameObject.transform.GetComponent<MeshRenderer>().material = _material[rng];
        }
    }
}
