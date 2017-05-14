using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrowdAI
{

    /// <summary>
    /// Class for Handling input and output
    /// </summary>
    public static class IOHandler
    {
        /// <summary>
        ///  Writes the data about the controller to disk in the streaming assets folder
        /// </summary>
        /// <param name="controller">The controller being saveds </param>
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
        /// <summary>
        /// Attempts to find the static instance of the controller and 
        ///  Writes the data about the controller to disk in the streaming assets folder
        /// </summary>
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

        

        /// <summary>
        /// Puts the position and rotation of a transform into the TransformData strcut then returns it
        /// </summary>
        /// <param name="transform"> The transform to get data from</param>
        /// <returns>The positions and rotation in a structure</returns>
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

        /// <summary>
        /// Makes a Vector3 from the position in TransformData
        /// </summary>
        /// <param name="data">The Transform to get the position from</param>
        /// <returns>A Vector3 that has the same position as given in the data</returns>
        public static Vector3 GetPosition(TransformData data)
        {
            return new Vector3(data._posX, data._posY, data._posZ);
        }

        /// <summary>
        /// Makes a Quaternioin from the rotations in TransformData
        /// </summary>
        /// <param name="data">The Transform to get the rotations from</param>
        /// <returns>A Quaternion that has the same rotations as given in the data</returns>
        public static Quaternion GetRotation(TransformData data)
        {
            return new Quaternion(data._rotX, data._rotY, data._rotZ, data._rotW);
        }
    }

}
