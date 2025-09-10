using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace DoDoDoIt
{
    public class CameraManager : MonoBehaviour
    {
        private static List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();
        public static CinemachineVirtualCamera ActiveCamera = null;

        
        private const int ActivePriority = 10;
        private const int InActivePriority = 0;
        
        public static bool IsActivatedCamera(CinemachineVirtualCamera camera)
        {
            return camera == ActiveCamera;
        }

        public static void SwitchCamera(CinemachineVirtualCamera newCamera)
        {
            if (newCamera == ActiveCamera)
            {
                return;
            }
            
            newCamera.Priority = ActivePriority;
            ActiveCamera = newCamera;

            foreach (CinemachineVirtualCamera cam in _cameras)
            {
                if (cam != newCamera)
                {
                    cam.Priority = InActivePriority;
                }
            }
        }

        public static void Register(CinemachineVirtualCamera camera)
        {
            if (!_cameras.Contains(camera))
            {
                _cameras.Add(camera);
            }
        } 

        public static void UnRegister(CinemachineVirtualCamera camera)
        {
            if (_cameras.Contains(camera))
            {
                _cameras.Remove(camera);
            }
        }
    }
}
