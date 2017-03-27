using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public static class CrowdGen
    {

        public static GameObject[,] GenCrowdCicle(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset )
        {
          // Not currently functional

            var _placeholder = new GameObject();

            _placeholder.name = "CrowdMemberPosition";

            // average size of the bounds = circumference
            float _radius = (bounds.x + bounds.z) / 4;

            int _xMax = Mathf.RoundToInt(_radius * crowdDensity);

            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;
           


            for (int i = 0; i < _xMax; i++)
            {
                int _yMax = Mathf.RoundToInt(Mathf.PI * 2 * (i + 1) * crowdDensity);

                for (int j = 0; j < _yMax; j++)
                {
                    float _newPosX = _radius * Mathf.Cos(Mathf.Deg2Rad * (j * (360 * crowdDensity)));
                    float _newPosZ = _radius * Mathf.Sin(Mathf.Deg2Rad * (j * (360 * crowdDensity)));

                    var _objPos = new Vector3(_parentPos.x + _newPosX, yOffset, _parentPos.z + _newPosZ);

                    var _crowdInstance = GameObject.Instantiate(_placeholder,_parentTrans);

                    _crowdInstance.transform.position = _objPos;
                }
            }

            return null;
        }

        public static GameObject[,] GenCrowdCicle(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, GameObject prefab)
        {

            float _radius = (bounds.x + bounds.z) / 4;

            // average size of the bounds = circumference
           

           

            do
            {
                GenerateRing(_radius, crowdDensity, parent, yOffset, prefab);
                _radius -= 1/crowdDensity;

            }
            while (_radius > 0);

            return null;
        }

        private static GameObject[] GenerateRing(float radius, float density, GameObject parent, float yOffset, GameObject prefab)
        {

            float _objCountUnround = 2 * Mathf.PI * radius * density;

            int _objCount = Mathf.RoundToInt(2 * Mathf.PI * radius*density);
            
            var _outRing = new GameObject[_objCount];
            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            for (int i = 0; i < _objCount; i++)
            {
                float _posX = radius * Mathf.Cos(Mathf.Deg2Rad * (i*density *(360 / _objCount)));
                float _posZ = radius * Mathf.Sin(Mathf.Deg2Rad * (i *density* (360 / _objCount)));

                var _newObj = GameObject.Instantiate(prefab,_parentTrans);
                _newObj.transform.position = new Vector3(_parentPos.x + _posX, yOffset, _parentPos.z + _posZ);
                _outRing[i] = _newObj;

             //   Debug.Log("Loop iteration: " + i + " |posX: " + _posX + " |posZ: " + _posZ);
            }

            float _gapDist = Vector3.Distance(_outRing[0].transform.position, _outRing[_objCount - 1].transform.position);
            int _objectsToBePlaced = Mathf.RoundToInt(_gapDist * density);

            Debug.Log("linear distance: " + _gapDist);
            _gapDist =radius * Mathf.Acos(1.0f - ((_gapDist * _gapDist) / (2 * radius * radius)));


                            

           // Debug.Log("Loop Complete");
            Debug.Log("Radius: "+ radius+ " |Density: " +density+ " |Distance: " + _gapDist + " |_actual count: " +_objCountUnround +" |rounded:" +_objCount+ " |Missing:" +_objectsToBePlaced);

            return _outRing;
        }


        public static GameObject[,] GenCrowdSquare(float crowdDensity, GameObject parent,  Vector3 bounds,  float yOffset)
        {

            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);
            


            var _placeholder = new GameObject();
            _placeholder.name = "CrowdMemberPosition";

            var _crowdMembers = new GameObject[_rows,_columns];

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _parentPos;
                    _newPos += new Vector3(j / crowdDensity, yOffset, i / crowdDensity);


                    _crowdMembers[i, j] = GameObject.Instantiate(_placeholder, _parentTrans);

                    _crowdMembers[i, j].transform.position = _newPos; ;
                }
            }
           
            return _crowdMembers;
            
          
         }

        public static GameObject[,] GenCrowdSquare(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, float densityRange)
        {
           
            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);



            var _placeholder = new GameObject();
            _placeholder.name = "CrowdMemberPosition";

            var _crowdMembers = new GameObject[_rows, _columns];

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _parentPos;

                    float _newDensity = crowdDensity + Random.Range(-densityRange, densityRange);
                    _newPos += new Vector3(j / _newDensity, yOffset, i / _newDensity);


                    _crowdMembers[i, j] = GameObject.Instantiate(_placeholder, _parentTrans);

                    _crowdMembers[i, j].transform.position = _newPos; ;
                }
            }

            return _crowdMembers;

        }

        public static GameObject[,] GenCrowdSquare(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, float densityRange,GameObject prefab)
        { // e = m/v, e = crowdDensity, m = n of people , v = bounds. Therefore n of people  =  CrowdDensity * bounds

            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);
            
           

            

            var _crowdMembers = new GameObject[_columns, _rows];
         

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _parentPos;
                    float _newDensity = crowdDensity + Random.Range(-densityRange, densityRange);
                    _newPos += new Vector3(j / _newDensity, yOffset, i / _newDensity);
                        

                    _crowdMembers[i, j] = GameObject.Instantiate(prefab, _parentTrans);

                    _crowdMembers[i, j].transform.position =_newPos;
                }
            }
            
            return _crowdMembers;



        }

        public static Vector3 GetObjectBounds(GameObject gO, bool is3D)
        {
            var _outBounds = new Vector3();

            if (is3D)
            {
                var _rend = gO.GetComponents<MeshRenderer>();
               

                if (_rend.Length < 1)
                {

                    _rend = gO.GetComponentsInChildren<MeshRenderer>();
                }

                _outBounds = _rend[0].bounds.extents;

                for (int i = 1; i < _rend.Length; i++)
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

                _outBounds = _spriteRend[0].bounds.extents;

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
    }

}
