using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
    public abstract class Clickable : MonoBehaviour, IClickable
    {
        [SerializeField]
        protected int currentClickCount;

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
        public Transform SnapPivot { get; protected set; }
        [field: SerializeField]
        public bool IsZmniejszable { get; set; } = true;

        public Action OnCounterCountChange { get; set; }
        public Action OnCounterMaxValue { get; set; }

        private float lastTimePointDecreased = 0;

        protected virtual void Update()
        {
            if (IsZmniejszable)
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
        }

        public abstract void OnCursorEnter(MultiMousePointerEventData eventData);

        public abstract void OnCursorExit(MultiMousePointerEventData eventData);

        public abstract void OnCursorClick(MultiMousePointerEventData eventData);
        public abstract void OnCursorUp(MultiMousePointerEventData eventData);
        public abstract void OnCursorDown(MultiMousePointerEventData eventData);
        public abstract void OnCursorDrag(MultiMousePointerEventData eventData);
    }
}