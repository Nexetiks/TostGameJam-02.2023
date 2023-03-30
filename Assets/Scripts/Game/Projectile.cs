using UnityEngine;

namespace Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _curveForce = 1f;
        [SerializeField]
        private AnimationCurve _flightCurve;
        [SerializeField]
        private Transform trailObject;

        private ITarget _target;
        private Vector3 _start;
        private Vector3 _end;
        private float _normalizedProgress = 0;
        private float _damage;

        public void Fire(Vector3 start, ITarget target, float damage = 10f)
        {
            _target = target;
            _start = start;
            _end = _target.GetPosition();
            _damage = damage;
        }

        private void FixedUpdate()
        {
            UpdateMovement(Time.fixedDeltaTime);
            CheckHit();
        }

        private void UpdateMovement(float deltaTime)
        {
            Vector3 previousPosition = transform.position;

            // Move projectile forward
            _normalizedProgress = Mathf.Min(deltaTime * _speed + _normalizedProgress, 1);
            // Calculate position on between start and end
            Vector3 position = Vector3.Lerp(_start, _end, _normalizedProgress);
            // Add trajectory height described by curve
            position.y += _flightCurve.Evaluate(_normalizedProgress) * _curveForce;
            transform.position = position;

            // Face the movement direction
            transform.LookAt(position + (position - previousPosition).normalized);
        }

        private void CheckHit()
        {
            // Target reached - hit target and destroy itself
            if (_normalizedProgress >= 1)
            {
                _target.Hit(_damage);
                trailObject.parent = null;
                Destroy(trailObject.gameObject, 3f);
                Destroy(gameObject);
            }
        }
    }
}