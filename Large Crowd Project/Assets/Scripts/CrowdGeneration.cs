using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CrowdAI
{
    public class CrowdGeneration 
    {

       
        private int _rows, _columns;

        
        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;

        float _objWidth = 0, _objHeight = 0;



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

            var prefabRend = crowdObject.GetComponent<Renderer>();

            // works with any renderer - may want to consider getting mutiple renderer components
            //May want to consider getting mutiple rendering components and finding the largest value to allow users to have mutiple sprite objects for example.
            

            if (!prefabRend)
            {
                prefabRend = crowdObject.GetComponentInChildren<Renderer>();

            }

            if (prefabRend)
            {
                _objWidth = prefabRend.bounds.extents.x * crowdObject.transform.localScale.x;
                _objHeight = prefabRend.bounds.extents.y * crowdObject.transform.localScale.y;
            }
            else
            {
                Debug.LogError("Could not find Renderer in the prefab object : " + crowdObject.name + ". You must attach a sprite or mesh renderer on the object or in a direct chlid of the object");
            }
            
        }


            /// <summary>
            /// Generates a crowd in a given formation
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

        private ICrowd[] GenerateCrowdCircle(GameObject gameObject, CrowdGroup[] groups, bool randomGroupDist)
        {
            var _crowdOutMembers = new List<ICrowd>();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var _transform = gameObject.transform;
            int _objCount = 0;

            //var _objColliderHeight = _crowdObject.GetComponent<MeshRenderer>().bounds.size.y * _transform.localScale.y;
            //var _objColliderWidth = _crowdObject.GetComponent<MeshRenderer>().bounds.size.x * _transform.localScale.x;

            for (int i = 0; i < _rows; i++)
            {
                //radius is number of layers multiplied by rough distance between objs
                var _radius = (i + 1) * (_objWidth * 2);
                var _circumference = 2 * Mathf.PI * _radius;

                //number of objs around the circumference of the layer
                var _objPerLayer = _circumference / (_objWidth * 2);

                for (int j = 0; j < _objPerLayer - (_objWidth / 2); j++)
                {
                    var _posX = _radius * Mathf.Cos(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));
                    var _posZ = _radius * Mathf.Sin(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));

                    var _objPos = new Vector3(_transform.position.x + _posX, _transform.position.y + (_objHeight / 2), _transform.position.z + _posZ);

                    var _obj = GameObject.Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

                    var _crowdMemberInfo = _obj.GetComponent<ICrowd>();
                    if (_crowdMemberInfo == null)
                    {
                        Debug.LogError("Crowd prefab object doesnt contain ICrowd interface");
                    }
                    else
                    {
                        _crowdOutMembers.Add(_crowdMemberInfo);
                    }

                    _objCount++;
                }

            }

            return _crowdOutMembers.ToArray();
        }

       private ICrowd[] GenerateCrowdSquare(GameObject gameObject, CrowdGroup[] groups, bool randomGroupDist)
        {
            

            var _crowdOutMembers = new List<ICrowd>();

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

                    var _crowdMemberInfo = _obj.GetComponent<ICrowd>();
                    if (_crowdMemberInfo == null)
                    {
                        Debug.LogError("Crowd prefab object doesnt contain ICrowd interface");
                    }
                    else
                    {
                        _crowdOutMembers.Add(_crowdMemberInfo);
                    }

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
            return _crowdOutMembers.ToArray();

        }

        private ICrowd[] GenerateCrowdRing(GameObject gameObject, CrowdGroup[]groups, bool randomGroupDist)
        {
            throw new System.NotImplementedException();
        }

    }

    
}
