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
        private int _xActors = 4, _yActors = 7;
        [SerializeField]
        private float _yOffset = 0.5f, _minGapSize = 0, _maxGapSize = 3,_crowdDensity=0.8f;
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private Vector3 _bounds;

        // Use this for initialization
        void Start()
        {

            var _size = CrowdGen.GetObjectBounds(_prefab, true);


            print("size: "+_size );
            switch (_formation)
            {
                case CrowdFormation.CIRCLE:

                    break;

                case CrowdFormation.SQUARE:
                    CrowdGen.GenCrowdSquare(_crowdDensity, gameObject, _size, _bounds, _yOffset, 0,_prefab);
                    break;
            }

            
        }

            
    }

}