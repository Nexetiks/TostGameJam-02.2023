using System;
using General;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class Crank : MonoBehaviour
    {
        [Serializable]
        public class UnityEventFloat : UnityEvent<float>
        {
        }

        public UnityEventFloat OnCrankDelta = new UnityEventFloat();

        [SerializeField]
        private CrankHandle _handle;
        [SerializeField]
        private Transform _arm;
        [SerializeField]
        private Transform _pole;
        [SerializeField]
        private float _dampen = 1;

        private void OnEnable()
        {
            _handle.OnHandleMoved += Handle_OnHandleMoved;
        }

        private void OnDisable()
        {
            _handle.OnHandleMoved -= Handle_OnHandleMoved;
        }

        private void Handle_OnHandleMoved(Vector2 delta)
        {
            Vector3 euler = _pole.transform.rotation.eulerAngles;

            float v = Vector3.Dot(Quaternion.Inverse(GameManager.Instance.CameraController.transform.rotation) * _pole.forward, new Vector3(-delta.x, 0, -delta.y));
            euler.y += v * _dampen;
            _pole.transform.eulerAngles = euler;

            OnCrankDelta.Invoke(_pole.transform.eulerAngles.y);
        }
    }
}