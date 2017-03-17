using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CrowdAI
{
    /// <summary>C:\Users\pvitalis\Documents\LargeCrowdProject\Large Crowd Project\Assets\Scripts\CrowdCircleGen.cs
    /// Class that generates crowd members in a formation
    /// </summary>
    public class CrowdGeneration
    {


        private int _rows, _columns;


        private float _minOffset, _maxOffset, _tiltAmount, _startHeight;

        float _spacing = 0, _objHeight = 0;

        private bool _is3D = true;

        private GameObject[] _crowdObjects;


        public CrowdGeneration(int rows, int columns, float minOffset, float maxOffset, float tiltAmount, float startHeight, GameObject[] crowdObjects)
        {

            _rows = rows;
            _columns = columns;
            _minOffset = minOffset;
            _maxOffset = maxOffset;
            _tiltAmount = tiltAmount;
            _startHeight = startHeight;
            _crowdObjects = crowdObjects;



        }

        public CrowdGeneration(int rows, int columns, float minOffset, float maxOffset, float tiltAmount, float startHeight, GameObject[] crowdObjects, bool is3D)
        {

            _rows = rows;
            _columns = columns;
            _minOffset = minOffset;
            _maxOffset = maxOffset;
            _tiltAmount = tiltAmount;
            _startHeight = startHeight;
            _crowdObjects = crowdObjects;


            _is3D = is3D;
        }



        private Vector3 GetObjectBounds(GameObject gO)
        {
            var _outBounds = new Vector3();

            if (_is3D)
            {
                var _rend = gO.GetComponents<MeshRenderer>();

                if (_rend.Length<1)
                {
                    _rend = gO.GetComponentsInChildren<MeshRenderer>();
                }

                for (int i = 0; i < _rend.Length; i++)
                {
                    var _newBounds = _rend[i].bounds.extents;

                    if (_outBounds.x < _newBounds.x)
                    {
                        _outBounds.x = _newBounds.x;
                    }

                    if (_outBounds.y < _newBounds.y)
                    {
                        _outBounds.y = _newBounds.y;
                    }

                    if (_outBounds.z < _newBounds.z)
                    {
                        _outBounds.z = _newBounds.z;
                    }
                }
            }
            else
            {
                var _spriteRend = gO.GetComponents<SpriteRenderer>();

                if (_spriteRend == null)
                {
                    _spriteRend = gO.GetComponentsInChildren<SpriteRenderer>();
                }

                for (int i = 0; i < _spriteRend.Length; i++)
                {
                    var _newBounds = _spriteRend[i].bounds.extents;

                    if (_outBounds.x < _newBounds.x)
                    {
                        _outBounds.x = _newBounds.x;
                    }

                    if (_outBounds.y < _newBounds.y)
                    {
                        _outBounds.y = _newBounds.y;
                    }

                    if (_outBounds.z < _newBounds.z)
                    {
                        _outBounds.z = _newBounds.z;
                    }


                }
            }

            _outBounds.x *= gO.transform.localScale.x * 2;
            _outBounds.y *= gO.transform.localScale.y * 2;
            _outBounds.z *= gO.transform.localScale.z * 2;

            return _outBounds;

        }



        /// <summary>
        /// Generates a crowd in a given formation
        /// </summary>
        /// <param name="formation"> The formation of the crowd desired</param>
        /// <param name="parent">the object to which all objects are parented to </param>
        /// <param name="groups"> the crowd groups that the crowd members are part of</param>
        /// <param name="randomGroupDist"> if true the different groups are randomly distributed in the crowd, otherwise it is uniform</param>
        public void GenerateCrowd(CrowdFormation formation, GameObject parent, ref CrowdGroup[] groups, bool randomGroupDist)
        {
            switch (formation)
            {
                case CrowdFormation.CIRCLE:
                   
                    GenerateCrowdCircle(parent, ref groups, randomGroupDist);
                    break;

                case CrowdFormation.RING:
                    GenerateCrowdRing(parent, ref groups, randomGroupDist);
                    break;

                default:
                    GenerateCrowdSquare(parent, ref groups, randomGroupDist);
                    break;


            }
        }

        private void GenerateCrowdCircle(GameObject gameObject, ref CrowdGroup[] groups, bool randomGroupDist)
        {
            // crowd groups are passed because they are to be able to have their own models, still implementing.


            var watch = System.Diagnostics.Stopwatch.StartNew();
            var _transform = gameObject.transform;
           

            bool[] hasModels = new bool[groups.Length];
            for (int i = 0; i < hasModels.Length; i++)
            {
                hasModels[i] = groups[i].GetCrowdModels != null;
            }


            var _bounds = GetObjectBounds(_crowdObjects[0]);
            
            if (randomGroupDist)
            {
                for (int i = 0; i < _rows; i++)
                {
                    //radius is number of layers multiplied by rough distance between objs
                    var _radius = (i + 1) * ((_bounds.z + _spacing) );
                    var _circumference = 2 * Mathf.PI * _radius;

                    //number of objs around the circumference of the layer
                    var _objPerLayer = _circumference / (_bounds.x + _spacing);



                    for (int j = 0; j < _objPerLayer - (_spacing / 2); j++)
                    {
                        var _posX = _radius * Mathf.Cos(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));
                        var _posZ = _radius * Mathf.Sin(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));

                        var _objPos = new Vector3(_transform.position.x + _posX, _transform.position.y + (_bounds.y / 2) + _startHeight, _transform.position.z + _posZ);

                        int _nextGroupIndex = Random.Range(0, groups.Length);


                       
                        var _nextPrefab = (GameObject)null;

                        int _modelIndex;

                        if (hasModels[_nextGroupIndex])
                        {
                            _modelIndex = Random.Range(0, groups[_nextGroupIndex].GetCrowdModels.Length);

                            _nextPrefab = groups[_nextGroupIndex].GetCrowdModels[_modelIndex];

                        }
                        else
                        {
                           
                            _modelIndex = Random.Range(0, _crowdObjects.Length );
                            _nextPrefab = _crowdObjects[_modelIndex];
                        }

                        var _newCrowdInstance = GameObject.Instantiate(_nextPrefab, _objPos, Quaternion.identity, _transform);

                        groups[_nextGroupIndex].AddCrowdMember(_newCrowdInstance);

                       
                    }

                }

            }
            else
            {
                // the total number of models that will be generated below (it's correct)

                float _maxRadius = _rows * (_bounds.z + _spacing);

                float _maxCircum = 2 * Mathf.PI * _maxRadius;
                float _maxObjLayer = _maxCircum / (_bounds.x + _spacing);
                
                // NEEDS THE TOTAL NUMBER OF MODELS THAT WILL BE CREATED TO FUNCTION this is when layers =50 & spacing  = 0
                int _totalModels = 8036;

                int _nextGroupIndex = 0;
              
                int _objCount = 0;
                int _membersPGroup = _totalModels / groups.Length;

                Debug.Log("Total Models: " + _totalModels);

                for (int i = 0; i < _rows; i++)
                {
                    //radius is number of layers multiplied by rough distance between objs
                    var _radius = (i + 1) * ((_bounds.z + _spacing));
                    var _circumference = 2 * Mathf.PI * _radius;

                    //number of objs around the circumference of the layer
                    var _objPerLayer = _circumference / (_bounds.x + _spacing);



                    for (int j = 0; j < _objPerLayer - (_spacing / 2); j++)
                    {
                        var _posX = _radius * Mathf.Cos(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));
                        var _posZ = _radius * Mathf.Sin(Mathf.Deg2Rad * (j * (360 / _objPerLayer)));

                        var _objPos = new Vector3(_transform.position.x + _posX, _transform.position.y + (_bounds.y / 2) + _startHeight, _transform.position.z + _posZ);

                       


                        
                        var _nextPrefab = (GameObject) null;

                        int _modelIndex;

                        _nextGroupIndex = _objCount / _membersPGroup;

                        if (hasModels[_nextGroupIndex])
                        {
                            _modelIndex = Random.Range(0, groups[_nextGroupIndex].GetCrowdModels.Length);

                            _nextPrefab = groups[_nextGroupIndex].GetCrowdModels[_modelIndex];

                        }
                        else
                        {
                            _modelIndex = Random.Range(0, _crowdObjects.Length);
                            _nextPrefab = _crowdObjects[_modelIndex];
                        }

                        var _newCrowdInstance = GameObject.Instantiate(_nextPrefab, _objPos, Quaternion.identity, _transform);

                        groups[_nextGroupIndex].AddCrowdMember(_newCrowdInstance);

                        _objCount++;
                    }

                }

                Debug.Log("total models: " + _totalModels + " object count: " + _objCount);
            }




        }

        private void GenerateCrowdSquare(GameObject gameObject, ref CrowdGroup[] groups, bool randomGroupDist)
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
                    var _objPos = new Vector3(_transform.position.x + i * _offset, gameObject.transform.position.y + _startHeight, _transform.position.z + j * _offset);

                    var _obj = GameObject.Instantiate(_crowdObjects[0], _objPos, _transform.rotation, _transform);

                    var _crowdMemberInfo = _obj.GetComponent<ICrowd>();


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
            ;

        }

        private void GenerateCrowdRing(GameObject gameObject, ref CrowdGroup[] groups, bool randomGroupDist)
        {
            throw new System.NotImplementedException();
        }

    }


}
