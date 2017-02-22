using UnityEngine;
using System.Collections;
namespace CrowdAI
{
    public class CrowdGeneration : MonoBehaviour
    {

        [SerializeField]
        private int _rows, _columns;

        [SerializeField]
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;

        [SerializeField]
        private GameObject _crowdObject;
        // Use this for initialization
        void Start()
        {
            GenerateCrowd();
        }

        void GenerateCrowd()
        {
            // Diagnostic tool to test how long a method takes to run.
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var _transform = gameObject.transform;
            int _objCount = 0;

            // Removed this for now as it's too specific of a use case. USE _startHeight INSTEAD!
            //var _objCollider = _crowdObject.GetComponent<MeshRenderer>().bounds.size.y * _transform.localScale.y;

            // Run through rows and columns and generate objects as needed. 
            // Make them a child of the source object.


            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _offset = Random.Range(_minOffset, _maxOffset);
                    var _objPos = new Vector3(_transform.position.x + i * _offset, transform.position.y + _startHeight, _transform.position.z + j * _offset);

                    var _obj = Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

                    _objCount++;
                }

                if (_tiltAmount != 0)
                {
                    _startHeight = _startHeight + _tiltAmount;
                }

            }

            watch.Stop();
            var _elapsedTime = watch.ElapsedMilliseconds;

            Debug.Log(System.String.Format("Generated {0} objects in {1} milliseconds.", _objCount, _elapsedTime));


        }
    }
}
