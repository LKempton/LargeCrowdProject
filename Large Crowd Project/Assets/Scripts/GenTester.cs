using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class GenTester : MonoBehaviour
    {
        [SerializeField]
        CrowdFormation _formation = 0;
        [SerializeField]
        private float _yOffset = 0.5f, _randomRange,_crowdDensity=0.8f;
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private Vector3 _bounds;

        // Use this for initialization
        void Start()
        {

            var _size = CrowdGen.GetObjectBounds(_prefab, true);


           
            switch (_formation)
            {
                case CrowdFormation.CIRCLE:
                    CrowdGen.GenCrowdCicle(_crowdDensity, gameObject, _bounds, _yOffset,_prefab);
                    break;

                case CrowdFormation.SQUARE:
                    CrowdGen.GenCrowdSquare(_crowdDensity, gameObject, _bounds, _yOffset, _randomRange, _prefab);
                    break;
            }

            
        }

            
    }

}