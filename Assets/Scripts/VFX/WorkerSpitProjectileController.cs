using System;
using General;
using Interactions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VFX
{
    public class WorkerSpitProjectileController : MonoBehaviour
    {
        private float _speed = 1f;
        [SerializeField]
        private float _curveForce = 1f;
        [SerializeField]
        private AnimationCurve _flightCurve;
        [SerializeField]
        private TrailRenderer trailRenderer;
        
        private Vector3 _start;
        private Vector3 _end;
        private float _normalizedProgress = 0;

        private bool wasCollisionTriggered;

        public void Fire(Vector3 start, Vector3 end, float speed)
        {
            _speed = speed * 10f;
            _start = start;
            _end = end;

            Vector3 position = _start;
            // Add trajectory height described by curve
            position.y += _flightCurve.Evaluate(0) * _curveForce;
            transform.position = position;
        }

        private void FixedUpdate()
        {
            UpdateMovement(Time.fixedDeltaTime);
            CheckHit();
        }

        private void OnTriggerEnter(Collider other)
        {
            IInteractable otherInteractable = other.GetComponent<IInteractable>();
            
            if (otherInteractable == null)
            {
                otherInteractable = other.GetComponentInParent<IInteractable>();
            }

            if (otherInteractable == null)
            {
                return;
            }
            wasCollisionTriggered = true;
            trailRenderer.transform.parent = null;
            Destroy(trailRenderer.transform.gameObject, 3f);
            Destroy(gameObject);

            GameManager.ClickData clickData = new GameManager.ClickData()
            {
                interactable = otherInteractable,
                eventData = new MultiMousePointerEventData(EventSystem.current, 0)
                {
                    pointerPressRaycast = new RaycastResult()
                    {
                        worldNormal = (other.transform.position - other.ClosestPoint(transform.position)).normalized,
                        worldPosition = other.ClosestPoint(transform.position),
                    }
                }
            };
            
            GameManager.Instance.OnAfterInteractableClicked?.Invoke(clickData);
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
            if (wasCollisionTriggered) return;
            
            // Target reached - hit target and destroy itself
            if (_normalizedProgress >= 1 )
            {
                trailRenderer.transform.parent = null;
                Destroy(trailRenderer.transform.gameObject, 3f);
                Destroy(gameObject);
            }
        }
    }
}
