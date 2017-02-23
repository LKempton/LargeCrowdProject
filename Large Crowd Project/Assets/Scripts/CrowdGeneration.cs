using UnityEngine;
using System.Collections;
namespace CrowdAI
{
    public class CrowdGeneration 
    {

       
        private int _rows, _columns;

        
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;

       
        private GameObject _crowdObject;
        
       public CrowdGeneration(int rows, int columns, float minOffset, float maxOffset, float tiltAmount, float startHeight, GameObject crowdObject)
        {
            
            _rows = rows;
            _columns = columns;
            _minOffset = minOffset;
            _maxOffset = maxOffset;
            _tiltAmount = tiltAmount;
            _startHeight = startHeight;
            _crowdObject = crowdObject;
        }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="formation"> The formation of the crowd desired</param>
            /// <param name="parent">the object to which all objects are parented to </param>
            /// <param name="groups"> the crowd groups that the crowd members are part of</param>
            /// <param name="randomGroupDist"> if true the different groups are randomly distributed in the crowd, otherwise it is uniform</param>
       public ICrowd[] GenerateCrowd(CrowdFormation formation, GameObject parent,CrowdGroup[] groups, bool randomGroupDist)
        {
            switch (formation)
            {
                case  CrowdFormation.CIRCLE:
                  return  GenerateCrowdCircle(parent,groups,randomGroupDist);
                    
                case CrowdFormation.RING:
                   return GenerateCrowdRing(parent, groups, randomGroupDist);

                default:
                   return GenerateCrowdSquare(parent, groups, randomGroupDist);


            }
        }

       private ICrowd[] GenerateCrowdCircle(GameObject gameObject,CrowdGroup[] groups, bool randomGroupDist)
        {
            throw new System.NotImplementedException();
        }

       private ICrowd[] GenerateCrowdSquare(GameObject gameObject, CrowdGroup[] groups, bool randomGroupDist)
        {
            throw new System.NotImplementedException();

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
                    var _objPos = new Vector3(_transform.position.x + i * _offset, gameObject.transform.position.y + _startHeight, _transform.position.z + j * _offset);

                    var _obj = GameObject.Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

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

        private ICrowd[] GenerateCrowdRing(GameObject gameObject, CrowdGroup[]groups, bool randomGroupDist)
        {
            throw new System.NotImplementedException();
        }

    }

    
}
