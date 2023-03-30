using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        public void UpdateRotation(float rotation)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y = rotation;
            transform.eulerAngles = euler;
        }
    }
}
