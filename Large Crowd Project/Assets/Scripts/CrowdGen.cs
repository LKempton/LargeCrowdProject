using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{

    /// <summary>
    /// Static helper class for generating crowd members
    /// </summary>
    public static class CrowdGen
    {
        /// <summary>
        /// Creates a crowd as a child of a crowd source in a circle formation
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="rotDir">The direction the crowd members are rotated to face</param>
        ///  <param name="parent">The parent GameObject that the crowdmembers will be attached to (and position)</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <param name="prefab">The crowd member to spawn a crowd of</param>
        /// <returns>The list of crowd member gameobjects generated</returns>
        public static GameObject[] GenCrowdCircle(float crowdDensity, float rotDir, GameObject parent, Vector3 bounds,  GameObject prefab)
        {
            var _outList = new List<GameObject>(); //used list since the number of game objects generated isn't determined in a linear manner


            prefab.transform.Rotate(0, rotDir, 0);

            float _radius = (bounds.x + bounds.z) / 4;


            // average size of the bounds = circumference

            for (float i = 0; i <=_radius; i+= 1/crowdDensity)
            {
               float  _tilt = (i / _radius) * bounds.y;
                GenerateRing(i, crowdDensity, _tilt, parent,  prefab,ref _outList);
                

            }

            prefab.transform.Rotate(0, -rotDir, 0);
            return _outList.ToArray() ;
        }

        /// <summary>
        /// Creates a crowd as a child of a crowd source in a square formation
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="rotDir">The direction the crowd members are rotated to face</param>
        ///  <param name="parent">The parent GameObject that the crowdmembers will be attached to</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <param name="prefab">The crowd member to spawn a crowd of</param>
        /// <returns>The array of crowd member gameobjects generated</returns>
        public static GameObject[] GenCrowdSquare(float crowdDensity, float rotDir, GameObject parent,  Vector3 bounds,   GameObject prefab)
        {


            var _parentTrans = parent.transform;
           

            int _rows = Mathf.RoundToInt(crowdDensity * bounds.x);
            int _columns = Mathf.RoundToInt(crowdDensity * bounds.z);

            _parentTrans.position += new Vector3(0.5f * bounds.x, 0, 0.5f * bounds.z);
            var _startPos = new Vector3(parent.transform.position.x - 0.5f * bounds.x, 0, parent.transform.position.z - 0.5f * bounds.z);

            prefab.transform.Rotate(0, rotDir, 0);

            if (_rows <0)
            {
                _rows *= -1;
            }
            if (_columns < 0)
            {
                _columns *= -1;
            }

            int _arrDiv = (_columns > _rows) ? _rows : _columns;

            float _tilt = bounds.y /  _columns;
          

            var _crowdMembers = new GameObject[_rows*_columns];

            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    var _newPos = _startPos;
                    _newPos += new Vector3(j / crowdDensity,i*_tilt, i / crowdDensity);

                    var _newObj = GameObject.Instantiate(prefab, _parentTrans);
                    _newObj.transform.position = _newPos; ;


                    _crowdMembers[i * _arrDiv+ j] = _newObj;
                }
            }

            prefab.transform.Rotate(0, -rotDir, 0);

            return _crowdMembers;
            
          
         }

        /// <summary>
        /// Creates a crowd as a child of a crowd source in a ring formation
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="rotDir">The direction the crowd members are rotated to face</param>
        ///  <param name="parent">The parent GameObject that the crowdmembers will be attached to</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <param name="prefab">The crowd member to spawn a crowd of</param>
        /// <param name="innerRadius">The radius of the hole in the middle of the ring</param>
        /// <returns>An array of crowd member gameobjects generateds</returns>
        public static GameObject[] GenCrowdRing(float crowdDensity,float rotDir, GameObject parent, Vector3 bounds, GameObject prefab, float innerRadius )
        {
            float _radius = (bounds.x + bounds.z) / 4;

            var _outList = new List<GameObject>();
            

            for (float i = _radius; i >innerRadius ; i-= 1/crowdDensity)
            {
                float _tilt = (i / _radius) * bounds.y;

                GenRingCentreFacing(i, rotDir, crowdDensity, _tilt, parent, prefab, ref _outList);
            }
            
            return _outList.ToArray();
        }

        /// <summary>
        /// Gets the boundaries of the size of a game object
        /// </summary>
        /// <param name="gO">The gameobject to get the boundaries of the size of</param>
        /// <returns>The boundaries of the size of a gameobject</returns>
        public static Vector3 GetObjectBounds(GameObject gO)
        {
            var _outBounds = new Vector3();

            var _rend = gO.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < _rend.Length; i++)
            {
                var _cRend = _rend[i];

                if ( _cRend is  MeshRenderer || _cRend is SpriteRenderer)
                {
                    var _cBounds = _cRend.bounds.extents;

                    if (_cBounds.x > _outBounds.x)
                    {
                        _outBounds.x = _cBounds.x;
                    }

                    if (_cBounds.y > _outBounds.y)
                    {
                        _outBounds.y = _cBounds.y;
                    }

                    if (_cBounds.z > _outBounds.z)
                    {
                        _outBounds.z = _cBounds.z;
                    }
                }
            }

            _outBounds.x *= gO.transform.localScale.x*2;
            _outBounds.y *= gO.transform.localScale.y*2;
            _outBounds.z *= gO.transform.localScale.z*2;

            return _outBounds;

        }

        /// <summary>
        /// Estimate the number of crowd members that will be generated in a ring formation with the current settings
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <param name="innerRadius">The radius of the hole in the middle of the ring</param>
        /// <returns>The estimated number of crowd members that will be generated in a ring formation with the current settings</returns>
        public static int EstimateRing(float crowdDensity, Vector3 bounds, float innerRadius)
        {
           
            

            float _radius = (Mathf.Abs(bounds.x) + Mathf.Abs(bounds.z)) / 4;

            if (innerRadius >= _radius)
            {
                return 0;
            }

            float _count = 0;

           for (float i  =_radius; i>innerRadius; i -= 1 / crowdDensity)
            {
               _count+= EstRing(crowdDensity, i);
            }

            return Mathf.RoundToInt(_count)+1;
        }

        /// <summary>
        /// Estimate the number of crowd members that will be generated in a square formation with the current settings
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <returns>The number of crowd members that will be generated in a square formation with the current settings</returns>
        public static int EstimateSquare(float crowdDensity, Vector3 bounds)
        {
            return Mathf.RoundToInt(Mathf.Abs(bounds.x)* Mathf.Abs(bounds.z) * crowdDensity * crowdDensity);
        }

        /// <summary>
        /// Estimate the number of crowd members that will be generated in a circle formation with the current settings
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="bounds">The size of the area the members will be generated in </param>
        /// <returns>The number of crowd members that will be generated in a circle formation with the current settings</returns>
        public static int EstimateCircle(float crowdDensity, Vector3 bounds)
        {
            float _count = 0;

           
            float _radius = (Mathf.Abs(bounds.x) + Mathf.Abs(bounds.z)) / 4;

            for (float i = 0; i <= _radius; i += 1 / crowdDensity)
            {
                _count += EstRing(crowdDensity, i);
            }

            return Mathf.RoundToInt(_count)+1;
        }

        /// <summary>
        /// Estimate the number of crowd members in a single loop of a ring or circle formation crowd
        /// </summary>
        /// <param name="crowdDensity">How densely packed the crowd members are in the crowd</param>
        /// <param name="radius">The radius of the ring being estimated</param>
        /// <returns>The number of crowd members in a single loop of a ring or circle formation crowd</returns>
        private static float EstRing(float crowdDensity, float radius)
        {
            return(float)2 * Mathf.PI * radius * crowdDensity;
        }

        /// <summary>
        /// Generate the crowd members for a single loop of a ring or circle formation crowd
        /// </summary>
        /// <param name="radius">The radius of the loop being generated</param>
        /// <param name="density">How densely packed the crowd members are in the crowd</param>
        /// <param name="yPos">The height of the loop being generated</param>
        /// <param name="parent">The parent GameObject that the crowdmembers will be attached to</param>
        /// <param name="prefab">The crowd member to spawn a crowd of</param>
        /// <param name="list">The list of crowd member gameobjects generated in the crowd so far</param>
        private static void GenerateRing(float radius, float density, float yPos, GameObject parent, GameObject prefab, ref List<GameObject> list)
        {
            float _objCount = 2 * Mathf.PI * radius * density;
         


            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            for (int i = 0; i < _objCount; i++)
            {
                float _posX = radius * Mathf.Cos(Mathf.Deg2Rad * (i * (360 / _objCount)));
                float _posZ = radius * Mathf.Sin(Mathf.Deg2Rad * (i * (360 / _objCount)));

                var _newObj = GameObject.Instantiate(prefab, _parentTrans);
                _newObj.transform.position = new Vector3(_parentPos.x + _posX, yPos, _parentPos.z + _posZ);
                list.Add(_newObj);

               
            }


        }
        /// <summary>
        /// Generate the crowd members for a single loop of a ring or circle formation crowd
        /// </summary>
        /// <param name="radius">The radius of the loop being generated</param>
        /// <param name="rotation">The direction the crowd members in the loop are generated facing</param>
        /// <param name="density">How densely packed the crowd members are in the crowd</param>
        /// <param name="_yPos">The height of the loop being generated</param>
        /// <param name="parent">The gameobject representing the first corner of the crowd</param>
        /// <param name="prefab">The crowd member to spawn a crowd of</param>
        /// <param name="list">The list of crowd member gameobjects generated in the crowd so far</param>
        private static void GenRingCentreFacing(float radius, float rotation, float density, float _yPos, GameObject parent, GameObject prefab, ref List<GameObject> list)
        {
            float _objCount = 2 * Mathf.PI * radius * density;



            var _parentTrans = parent.transform;
            var _parentPos = _parentTrans.position;

            var _lookDir = _parentPos ;
            _lookDir.y = 0;

            for (int i = 0; i < _objCount; i++)
            {
                float _posX = radius * Mathf.Cos(Mathf.Deg2Rad * (i * (360 / _objCount)));
                float _posZ = radius * Mathf.Sin(Mathf.Deg2Rad * (i * (360 / _objCount)));

                var _newObj = GameObject.Instantiate(prefab, _parentTrans);

                float _yRotz = Mathf.Sin((i * (360 / _objCount)));

                _newObj.transform.position = new Vector3(_parentPos.x + _posX, _yPos, _parentPos.z + _posZ);
                _newObj.transform.LookAt(_lookDir, Vector3.up);
                _newObj.transform.Rotate(0, 180+rotation, 0);
                
                list.Add(_newObj);


            }
        }

    }

}
