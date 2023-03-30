using System;
using General;
using Interactions;
using MultiMouse;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class CrankHandle : MonoBehaviour, IClickable
    {
        public event Action<Vector2> OnHandleMoved;
        public int CurrentClickCount { get; set; }
        public int MaxClickCunt { get; set; }
        public float TimeOfLastClick { get; set; }
        public float SecondPerOneClickDisappear { get; set; }
        public Action OnCounterCountChange { get; set; }
        public Action OnCounterMaxValue { get; set; }

        public Transform SnapPivot => _crankHandle;
        [SerializeField]
        private Transform _crankHandle;

        private string DebugTag => $"[{nameof(Crank)}]";

        public ClickableMode ClickableMode { get; set; }
        public float SecondToWaitBeforeClickCountDecreases { get; set; }
        public float SecondsToDecreasesOnePoint { get; set; }

        private MultiMouseDevice _attachedMouse;

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            if (_attachedMouse == null) return;

            OnHandleMoved(_attachedMouse.Delta);
        }

        private void LateUpdate()
        {
            if (_attachedMouse == null) return;

            _attachedMouse.Position = Camera.main.WorldToScreenPoint(SnapPivot.position);
        }

        public void OnCursorEnter(MultiMousePointerEventData eventData) => Debug.Log($"{DebugTag} {nameof(OnCursorEnter)}");

        public void OnCursorExit(MultiMousePointerEventData eventData) => Debug.Log($"{DebugTag} {nameof(OnCursorExit)}");

        public void OnCursorClick(MultiMousePointerEventData eventData)
        {
        }

        public void OnCursorDown(MultiMousePointerEventData eventData)
        {
            if (_attachedMouse == null) _attachedMouse = eventData.MouseDevice;
        }

        public void OnCursorDrag(MultiMousePointerEventData eventData)
        {
        }

        public void OnCursorUp(MultiMousePointerEventData eventData)
        {
            eventData.MouseDevice.Position = Camera.main.WorldToScreenPoint(SnapPivot.position);
            if (_attachedMouse == eventData.MouseDevice) _attachedMouse = null;
        }
    }
}