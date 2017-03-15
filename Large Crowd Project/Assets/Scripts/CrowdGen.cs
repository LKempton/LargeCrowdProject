using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdAI
{
    public static class CrowdGen
    {

        public static void GenCrowdCicle()
        {

        }

        public static GameObject[,] GenCrowdSquare(int columns, int rows, GameObject parent, Vector3 size,  float yOffset, float minGapSize, float maxGapSize)
        {
            var _placeHolder = new GameObject();
            var _parentPos = parent.transform.position;

            var _crowdMembers = new GameObject[columns, rows];
            
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var offset = new Vector3();
                    offset.x += Random.Range(minGapSize, maxGapSize);
                    
                    offset.z = Random.Range(minGapSize, maxGapSize);
                    var _newPos = _parentPos + new Vector3(i*(offset.x+size.x),yOffset, (offset.z+size.z)*j );

                    _crowdMembers[i, j] = GameObject.Instantiate(_placeHolder, parent.transform);
                    _crowdMembers[i, j].transform.position = _newPos;
                }
            }

            return _crowdMembers;

        }


        public static GameObject[,] GenCrowdSquare(int columns, int rows, GameObject parent, Vector3 size, float yOffset, float minGapSize, float maxGapSize, GameObject prefab)
        {
         
            var _parentPos = parent.transform.position;

            var _crowdMembers = new GameObject[columns, rows];

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var offset = new Vector3();
                    offset.x += Random.Range(minGapSize, maxGapSize);

                    offset.z = Random.Range(minGapSize, maxGapSize);
                    var _newPos = _parentPos + new Vector3(i * (offset.x +size.x), yOffset, (offset.z + size.z) * j);

                    _crowdMembers[i, j] = GameObject.Instantiate(prefab, parent.transform);
                    _crowdMembers[i, j].transform.position = _newPos;
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
