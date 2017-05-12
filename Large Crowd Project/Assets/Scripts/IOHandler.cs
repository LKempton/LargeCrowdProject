using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrowdAI
{


    public static class IOHandler
    {
        
        public static void SaveController(CrowdController controller)
        {
            if (controller == null)
            {
                return;
            }

            var _controllerData = new CrowdData();
            var _sources = controller.Sources;

            _controllerData._stateNames = controller.GetCrowdStates();

            var _groups = controller.GetGroups();

            if (_groups != null)
            {
                _controllerData._groups = new GroupData[_groups.Length];
                for (int i = 0; i < _groups.Length; i++)
                {

                    _controllerData._groups[i] = _groups[i].GetData(_sources);
                }

            }


            _controllerData._unassignedGroup = controller.GetUnassignedGroup.GetData(_sources);

            
            if (_sources.Count > 0)
            {
                _controllerData._parents = new TransformData[_sources.Count];

                for (int i = 0; i < _sources.Count; i++)
                {
                    _controllerData._parents[i] = GetTransformData(_sources[i].transform);
                }
            }


            string _path = Application.streamingAssetsPath + @"/CrowdAIData";
            Debug.Log(_path);
           
            string _fileName = @"/CrowdData - " + SceneManager.GetActiveScene().name + ".data.json";
            Debug.Log(_fileName);
            Directory.CreateDirectory(_path);

            var _serializedData = JsonConvert.SerializeObject(_controllerData);

            File.WriteAllText(_path + _fileName, _serializedData);


        }

        public static void SaveController()
        {
            var _controller = CrowdController.GetCrowdController();

            if (_controller != null)
            {
                SaveController(_controller);
            }
            else
            {
                Debug.LogError("The static instance of the controller is null, can't save");
            }
        }

        


        public static TransformData GetTransformData(Transform transform)
        {
            var _tranformData = new TransformData();
            var _position = transform.position;
            var _rotation = transform.rotation;

            _tranformData._posX = _position.x;
            _tranformData._posY = _position.y;
            _tranformData._posZ = _position.z;

            _tranformData._rotW = _rotation.w;
            _tranformData._rotX = _rotation.x;
            _tranformData._rotY = _rotation.y;
            _tranformData._rotZ = _rotation.z;

            return _tranformData;
        }

        public static Vector3 GetPosition(TransformData data)
        {
            return new Vector3(data._posX, data._posY, data._posZ);
        }

        public static Quaternion GetRotation(TransformData data)
        {
            return new Quaternion(data._rotX, data._rotY, data._rotZ, data._rotW);
        }
    }

}
