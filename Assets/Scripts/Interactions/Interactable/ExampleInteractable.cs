using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions.Interactable
{
    public class ExampleInteractable : MonoBehaviour, IClickable
    {
        [SerializeField]
        private int currentClickCount;

        public int CurrentClickCount
        {
            get { return currentClickCount; }
            set
            {
                currentClickCount = value;
                OnCounterCountChange?.Invoke();

                if (currentClickCount > MaxClickCunt)
                {
                    if (ClickableMode == ClickableMode.NoMaxValue)
                    {
                        //nothing to do
                    }
                    else if (ClickableMode == ClickableMode.ResetOnMaxValue)
                    {
                        OnCounterMaxValue?.Invoke();

                        currentClickCount = 0;
                    }
                    else if (ClickableMode == ClickableMode.StayOnMaxValue)
                    {
                        currentClickCount = MaxClickCunt;
                    }
                }

                if (currentClickCount < 0)
                {
                    currentClickCount = 0;
                }
            }
        }

        [field: SerializeField]
        public int MaxClickCunt { get; set; } = 10;
        [field: SerializeField]
        public float TimeOfLastClick { get; set; } = 0;
        [field: SerializeField]
        public ClickableMode ClickableMode { get; set; }
        [field: SerializeField]
        public float SecondToWaitBeforeClickCountDecreases { get; set; }
        [field: SerializeField]
        public float SecondsToDecreasesOnePoint { get; set; } = 5;

        [field: SerializeField]
        public Transform SnapPivot { get; }

        public Action OnCounterCountChange { get; set; }
        public Action OnCounterMaxValue { get; set; }

        private float lastTimePointDecreased = 0;

        private void Update()
        {
            if ((Time.time - TimeOfLastClick) > SecondToWaitBeforeClickCountDecreases)
            {
                if ((Time.time - lastTimePointDecreased) > SecondsToDecreasesOnePoint)
                {
                    lastTimePointDecreased = Time.time;
                    CurrentClickCount--;
                }
            }
        }

        public void OnCursorEnter(MultiMousePointerEventData eventData)
        {
            Debug.Log("OnCursorEnter");
        }

        public void OnCursorClick(MultiMousePointerEventData eventData)
        {
            Debug.Log(CurrentClickCount);
            Debug.Log("OnCursorClick");
        }

        public void OnCursorExit(MultiMousePointerEventData eventData)
        {
            Debug.Log("OnCursorExit");
        }

        public void OnCursorUp(MultiMousePointerEventData eventData)
        {
        }

        public void OnCursorDown(MultiMousePointerEventData eventData)
        {
        }

        public void OnCursorDrag(MultiMousePointerEventData eventData)
        {
        }
    }
}