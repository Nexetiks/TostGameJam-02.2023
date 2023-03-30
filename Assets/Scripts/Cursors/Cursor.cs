using General;
using Interactions;
using MultiMouse;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Cursors
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cursorObject;
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [field: SerializeField]
        public float Damage { get; protected set; } = 5f;

        private MultiMouseDevice _mouse;
        private string DebugTag => $"[{nameof(Cursor)}]";

        private IInteractable _interactableUnderCursor;

        public void Initialize(MultiMouseDevice mouse, string name)
        {
            _mouse = mouse;
            transform.name = name;
        }

        private void Update()
        {
            if (_mouse == null) return;

            UpdateInteractable();
            UpdatePosition();
        }
        
        public void Clean() { }

        public void SetColor(Color color)
        {
            //_image.color = color;
            _meshRenderer.material.color = color;
        }

        private void UpdateInteractable()
        {
            if (GameManager.Instance.Hovers.TryGetValue(_mouse, out var interactable))
            {
                if (interactable != null)
                {
                    _interactableUnderCursor = interactable;
                    return;
                }
            }
            _interactableUnderCursor = null;
        }

        private void UpdatePosition()
        {
            if (_interactableUnderCursor != null && _interactableUnderCursor.SnapPivot != null)
            {
                transform.position = _interactableUnderCursor.SnapPivot.position;
                return;
            }

            // Fallback to physical raycast if Hovers does not contain any data for this mouse
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_mouse.Position);

            int layerMask = ~(1 << gameObject.layer);
            Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                //Debug.Log($"{DebugTag} Hit point {hit.collider.name}");
                transform.position = hit.point;
            }
            else
            {
                //Debug.Log($"{DebugTag} Hit plane");
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                plane.Raycast(ray, out float enter);
                transform.position = ray.GetPoint(enter);
            }
        }
    }
}

