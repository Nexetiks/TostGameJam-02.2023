using General;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
    public enum ClickableMode
    {
        ResetOnMaxValue,
        StayOnMaxValue,
        NoMaxValue,
    }

    public interface IClickable : IInteractable, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        /// <summary>
        /// Need BackField, in setter check if its bigger then MaxClickCunt or its less then zero, set TimeOfLastClick;
        /// </summary>
        public int CurrentClickCount { get; set; }
        public int MaxClickCunt { get; set; }
        public float TimeOfLastClick { get; set; }
        public ClickableMode ClickableMode { get; set; }
        public float SecondToWaitBeforeClickCountDecreases { get; set; }
        public float SecondsToDecreasesOnePoint { get; set; }

        public Action OnCounterCountChange { get; set; }
        public Action OnCounterMaxValue { get; set; }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;

            GameManager.ClickData clickData = new GameManager.ClickData()
            {
                interactable = this,
                eventData = data
            };

            TimeOfLastClick = Time.time;
            CurrentClickCount++;
            GameManager.Instance.OnBeforeInteractableClicked?.Invoke(clickData);
            OnCursorClick(data);
            GameManager.Instance.OnAfterInteractableClicked?.Invoke(clickData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;
            OnCursorUp(data);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;
            OnCursorDown(data);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;
            OnCursorDrag(data);
        }

        void OnCursorClick(MultiMousePointerEventData eventData);

        void OnCursorUp(MultiMousePointerEventData eventData);
        void OnCursorDown(MultiMousePointerEventData eventData);
        void OnCursorDrag(MultiMousePointerEventData eventData);
    }
}