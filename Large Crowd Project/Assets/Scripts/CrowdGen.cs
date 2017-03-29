using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public static class CrowdGen
    {

       

        public static GameObject[] GenCrowdCircle(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, GameObject prefab)
        {
            var _outList = new List<GameObject>(); 
            //used list since the number of game objects generated isn't determined in a linear manner

            float _radius = (bounds.x + bounds.z) / 4;

            // average size of the bounds = circumference
           

           

            do
            {
                GenerateRing(_radius, crowdDensity, parent, yOffset, prefab,ref _outList);
                _radius -= 1/crowdDensity;

            }
            while (_radius > 0);

            return _outList.ToArray() ;
        }

        public static GameObject[] GenCrowdSquare(float crowdDensity, GameObject parent,  Vector3 bounds,  float yOffset, GameObject prefab)
        {

            

            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);
            


           
            var _crowdMembers = new GameObject[_rows*_columns];

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _parentPos;
                    _newPos += new Vector3(j / crowdDensity, yOffset, i / crowdDensity);

                    var _newObj = GameObject.Instantiate(prefab, _parentTrans);
                    _newObj.transform.position = _newPos; ;


                    _crowdMembers[i * _columns + j] = _newObj;
                }
            }
           
            return _crowdMembers;
            
          
         }

        public static GameObject[] GenCrowdSquare(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, float densityRange,GameObject prefab, float tilt)
        { // e = m/v, e = crowdDensity, m = n of people , v = bounds. Therefore n of people  =  CrowdDensity * bounds

            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);

            var _crowdMembers = new GameObject[_columns* _rows];

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _parentPos;
                    float _newDensity = crowdDensity + Random.Range(-densityRange, densityRange);
                    _newPos += new Vector3(j / _newDensity, yOffset +i*tilt, i / _newDensity);
                        

                    var _newObj = GameObject.Instantiate(prefab, _parentTrans);
                    _newObj.transform.position = _newPos;

                    _crowdMembers[i * _columns + j] = _newObj;
                   
                }
            }
            
            return _crowdMembers;

        }

       public static GameObject[] GenCrowdRing(float crowdDensity, GameObject parent, Vector3 bounds, float yOffset, GameObject prefab, float innerRadius, float tiltAmount)
        {
            float _radius = (bounds.x + bounds.z) / 4;

           

            var _outList = new List<GameObject>();

            for (float i = _radius; i >innerRadius ; i-= 1/crowdDensity)
            {
                Debug.Log(i);
                GenerateRing(i, crowdDensity, parent, yOffset +i*tiltAmount, prefab, ref _outList);
            }
            
            return _outList.ToArray();
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

        private static void GenerateRing(float radius, float density, GameObject parent, float yOffset, GameObject prefab, ref List<GameObject> list)
        {
            float _objCount = 2 * Mathf.PI * radius * density;
            int index = 0;


            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            for (float i = 0; i <= _objCount; i++)
            {
                float _posX = radius * Mathf.Cos(Mathf.Deg2Rad * (i * (360 / _objCount)));
                float _posZ = radius * Mathf.Sin(Mathf.Deg2Rad * (i * (360 / _objCount)));

                var _newObj = GameObject.Instantiate(prefab, _parentTrans);
                _newObj.transform.position = new Vector3(_parentPos.x + _posX, yOffset, _parentPos.z + _posZ);
                list.Add(_newObj);

                index++;
            }


        }
    }

}
