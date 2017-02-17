using UnityEngine;
using System.Collections;

public class CrowdGeneration : MonoBehaviour {

    [SerializeField]
    private int _rows, _columns;

    [SerializeField]
    private float _offset = 0.5f;

    [SerializeField]
    private GameObject _crowdObject;
	// Use this for initialization
	void Start () 
    {
        GenerateCrowd();
	}

    void GenerateCrowd()
    {
        var _transform = gameObject.transform;
        int _objCount = 0;

        var _objCollider = _crowdObject.GetComponent<CapsuleCollider>().height;

        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                var _objPos = new Vector3(_transform.position.x + i * _offset, transform.position.y + (_objCollider / 2), _transform.position.z + j * _offset);

                var _obj = Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

                _objCount++;
                print(_objCount);
                
            }
        }

    }
}
