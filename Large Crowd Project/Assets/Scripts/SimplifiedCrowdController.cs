using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public class SimplifiedCrowdController : MonoBehaviour
    {
        [SerializeField]
        private CrowdFormation _crowdFormation;

        [SerializeField]
        private Team _team;

        [SerializeField]
        private GameObject _startingPrefab;

        private int _crowdCount = 0;

        [SerializeField]
        private float _density, _startHeight, _innerRadius, _rotDir = 0;

        [SerializeField]
        private int _crowdMemberLayer;

        public void GenerateCrowd(Vector3 bounds)
        {
            var parent = new GameObject();
            parent.name = "Crowd Source";

            bounds = transform.GetChild(0).transform.localPosition;
            parent.transform.position = transform.position;

            var posModifier = Vector3.zero;

            if (bounds.x < 0)
            {
                posModifier.x += bounds.x;
                bounds.x *= -1;
            }

            if (bounds.z < 0)
            {
                posModifier.z += bounds.z;
                bounds.z *= -1;
            }

            parent.transform.position += posModifier;

            GameObject[] newCrowd;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:

                    parent.transform.position += .5f * bounds;
                    newCrowd = CrowdGen.GenCrowdCircle(_density, _rotDir, parent, bounds, _startingPrefab);
                    break;


                case CrowdFormation.SQUARE:

                    parent.transform.position += .5f * CrowdGen.GetObjectBounds(_startingPrefab);
                    newCrowd = CrowdGen.GenCrowdSquare(_density, _rotDir, parent, bounds, _startingPrefab);
                    break;

                default:
                    parent.transform.position += .5f * bounds;

                    newCrowd = CrowdGen.GenCrowdRing(_density, _rotDir, parent, bounds, _startingPrefab, _innerRadius);
                    break;
            }

            if (newCrowd.Length > 0)
            {
                _crowdCount += newCrowd.Length;
            }
            else
            {
                Destroy(parent);
            }
        }

        public int GetPrediction()
        {
            int prediction = 0;

            var bounds = transform.GetChild(0).transform.localPosition;

            switch (_crowdFormation)
            {
                case CrowdFormation.CIRCLE:
                    prediction = CrowdGen.EstimateCircle(_density, bounds);
                    break;

                case CrowdFormation.RING:
                    prediction = CrowdGen.EstimateRing(_density, bounds, _innerRadius);

                    break;

                case CrowdFormation.SQUARE:
                    prediction = CrowdGen.EstimateSquare(_density, bounds);
                    break;
            }

            return prediction;
        }

        private void CountCrowdMembers()
        {
            int total = 0;

            GameObject[] gameObjectsInScene = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            for (int i = 0; i < gameObjectsInScene.Length; i++)
            {
                if (gameObjectsInScene[i].layer == _crowdMemberLayer)
                {
                    total++;
                }
            }

            _crowdCount = total;
        }

        public int CrowdCount
        {
            get
            {
                return _crowdCount;
            }
        }
    }
}
