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
        private float _yOffset = 0.5f, _minGapSize = 0, _maxGapSize = 3;
        [SerializeField]
        private GameObject _prefab;

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
                    if(!_prefab)
                    CrowdGen.GenCrowdSquare(_xActors, _yActors, gameObject, Vector3.one, _yOffset, _minGapSize, _maxGapSize);
                    else
                        CrowdGen.GenCrowdSquare(_xActors, _yActors, gameObject, Vector3.one, _yOffset, _minGapSize, _maxGapSize,_prefab);
                    break;
            }

            
        }


    }

}