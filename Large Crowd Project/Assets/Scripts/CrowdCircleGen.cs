using UnityEngine;
using System.Collections;
namespace CrowdAI
{
    public class CrowdCircleGen : MonoBehaviour
    {

        [SerializeField]
        private int _layers;

        [SerializeField]
        private float _minOffset, _maxOffset, _objHeight, _objWidth;

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

            //var _objColliderHeight = _crowdObject.GetComponent<MeshRenderer>().bounds.size.y * _transform.localScale.y;
            //var _objColliderWidth = _crowdObject.GetComponent<MeshRenderer>().bounds.size.x * _transform.localScale.x;

            for (int i = 0; i < _layers; i++)
            {
                var _radius = (i + 1) * _objWidth * 2;
                var _circumference = 2 * Mathf.PI * _radius;
                int _objPerLayer = (int)(_circumference / (_objWidth * 2));
                

                for (int j = 0; j < _objPerLayer; j++)
                {
                    var _offset = Random.Range(_minOffset, _maxOffset);

                    var _posX = _radius * Mathf.Cos(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));
                    var _posZ = _radius * Mathf.Sin(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));

                    var _objPos = new Vector3(_transform.position.x + _posX + (1 * _offset), _transform.position.y + (_objHeight / 2), _transform.position.z + _posZ + (1 * _offset));

                    var _obj = Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

                    _objCount++;
                }
            }

            //// Run through rows and columns and generate objects as needed. 
            //// Make them a child of the source object.
            //for (int i = 0; i < _columns; i++)
            //{
            //    for (int j = 0; j < _rows; j++)
            //    {
            //        var _offset = Random.Range(_minOffset, _maxOffset);
            //        var _objPos = new Vector3(_transform.position.x + i + (1 * _offset), transform.position.y + (_objColliderHeight / 2), _transform.position.z + j);

            //        var _obj = Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

            //        _objCount++;
            //    }
            //}

            watch.Stop();
            var _elapsedTime = watch.ElapsedMilliseconds;

            Debug.Log(System.String.Format("Generated {0} objects in {1} milliseconds.", _objCount, _elapsedTime));


        }
    }
}
