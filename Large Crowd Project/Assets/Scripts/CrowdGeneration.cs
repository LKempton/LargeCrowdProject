using UnityEngine;
using System.Collections;

public class CrowdGeneration : MonoBehaviour {

    [SerializeField]
    private int _rows, _columns;

    [SerializeField]
    private float _minOffset, _maxOffset;

    [SerializeField]
    private GameObject _crowdObject;
	// Use this for initialization
	void Start () 
    {
        GenerateCrowd();
	}

    void GenerateCrowd()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var _transform = gameObject.transform;
        int _objCount = 0;

        var _objCollider = _crowdObject.GetComponent<MeshRenderer>().bounds.size.y * _transform.localScale.y ;

        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                var _offset = Random.Range(_minOffset, _maxOffset);
                var _objPos = new Vector3(_transform.position.x + i * _offset, transform.position.y + (_objCollider / 2), _transform.position.z + j * _offset);

                var _obj = Instantiate(_crowdObject, _objPos, _transform.rotation, _transform);

                _objCount++;
            }
        }

        watch.Stop();
        var _elapsedTime = watch.ElapsedMilliseconds / 100;

        Debug.Log(System.String.Format("Generated {0} objects in {1} seconds.", _objCount, _elapsedTime));
       

    }
}
