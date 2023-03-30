﻿using MultiMouse;
using System;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
    [RequireComponent(typeof(MultiMouseManager))]
    [AddComponentMenu("Event/Multi Mouse Input Module")]
    public class MultiMouseInputModule : MultiMousePointerInputModule
    {
        private float m_PrevActionTime;
        private Vector2 m_LastMoveVector;
        private int m_ConsecutiveMoveCount;
        private GameObject m_CurrentFocusedGameObject;
        private PointerEventData m_InputPointerEvent;
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";
        [SerializeField]
        private string m_VerticalAxis = "Vertical";
        [SerializeField]
        private string m_SubmitButton = "Submit";
        [SerializeField]
        private string m_CancelButton = "Cancel";
        [SerializeField]
        private float m_InputActionsPerSecond = 10f;
        [SerializeField]
        private float m_RepeatDelay = 0.5f;
        [SerializeField]
        [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
        private bool m_ForceModuleActive;

        protected MultiMouseInputModule()
        {
        }

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public MultiMouseInputModule.InputMode inputMode => MultiMouseInputModule.InputMode.Mouse;

        [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
        public bool allowActivationOnMobileDevice
        {
            get => this.m_ForceModuleActive;
            set => this.m_ForceModuleActive = value;
        }

        public bool forceModuleActive
        {
            get => this.m_ForceModuleActive;
            set => this.m_ForceModuleActive = value;
        }

        public float inputActionsPerSecond
        {
            get => this.m_InputActionsPerSecond;
            set => this.m_InputActionsPerSecond = value;
        }

        public float repeatDelay
        {
            get => this.m_RepeatDelay;
            set => this.m_RepeatDelay = value;
        }

        public string horizontalAxis
        {
            get => this.m_HorizontalAxis;
            set => this.m_HorizontalAxis = value;
        }

        public string verticalAxis
        {
            get => this.m_VerticalAxis;
            set => this.m_VerticalAxis = value;
        }

        public string submitButton
        {
            get => this.m_SubmitButton;
            set => this.m_SubmitButton = value;
        }

        public string cancelButton
        {
            get => this.m_CancelButton;
            set => this.m_CancelButton = value;
        }

        private bool ShouldIgnoreEventsOnNoFocus()
        {
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.MacOSX:
                case OperatingSystemFamily.Windows:
                case OperatingSystemFamily.Linux:
                    return true;
                default:
                    return false;
            }
        }

        public override void UpdateModule()
        {
            if (this.eventSystem.isFocused || !this.ShouldIgnoreEventsOnNoFocus())
                return;
            if (this.m_InputPointerEvent != null && (UnityEngine.Object)this.m_InputPointerEvent.pointerDrag != (UnityEngine.Object)null && this.m_InputPointerEvent.dragging)
                this.ReleaseMouse(this.m_InputPointerEvent, this.m_InputPointerEvent.pointerCurrentRaycast.gameObject);
            this.m_InputPointerEvent = (PointerEventData)null;
        }

        private void ReleaseMouse(PointerEventData pointerEvent, GameObject currentOverGo)
        {
            ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerUpHandler);
            GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);
            if ((UnityEngine.Object)pointerEvent.pointerPress == (UnityEngine.Object)eventHandler && pointerEvent.eligibleForClick)
                ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerClickHandler);
            else if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.ExecuteHierarchy<IDropHandler>(currentOverGo, (BaseEventData)pointerEvent, ExecuteEvents.dropHandler);
            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = (GameObject)null;
            pointerEvent.rawPointerPress = (GameObject)null;
            if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.endDragHandler);
            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = (GameObject)null;
            if ((UnityEngine.Object)currentOverGo != (UnityEngine.Object)pointerEvent.pointerEnter)
            {
                this.HandlePointerExitAndEnter(pointerEvent, (GameObject)null);
                this.HandlePointerExitAndEnter(pointerEvent, currentOverGo);
            }
            this.m_InputPointerEvent = pointerEvent;
        }

        public override bool IsModuleSupported() => this.m_ForceModuleActive || this.input.mousePresent || this.input.touchSupported;

        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;
            bool flag = this.m_ForceModuleActive | this.input.GetButtonDown(this.m_SubmitButton) | this.input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(this.input.GetAxisRaw(this.m_HorizontalAxis), 0.0f) | !Mathf.Approximately(this.input.GetAxisRaw(this.m_VerticalAxis), 0.0f);
            foreach (MultiMouseDevice mouse in MultiMouseManager.Instance.Mice)
            {
                flag |= (double)mouse.Delta.sqrMagnitude > 0.0;
                flag |= mouse.GetMouseButtonDown(0);
            }
            if (this.input.touchCount > 0)
                flag = true;
            return flag;
        }

        public override void ActivateModule()
        {
            if (!this.eventSystem.isFocused && this.ShouldIgnoreEventsOnNoFocus())
                return;
            base.ActivateModule();
            GameObject selectedGameObject = this.eventSystem.currentSelectedGameObject;
            if ((UnityEngine.Object)selectedGameObject == (UnityEngine.Object)null)
                selectedGameObject = this.eventSystem.firstSelectedGameObject;
            this.eventSystem.SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
        }

        public override void DeactivateModule()
        {
            base.DeactivateModule();
            this.ClearSelection();
        }

        public override void Process()
        {
            if (!this.eventSystem.isFocused && this.ShouldIgnoreEventsOnNoFocus())
                return;
            bool selectedObject = this.SendUpdateEventToSelectedObject();
            if (!this.ProcessTouchEvents() && this.input.mousePresent)
                this.ProcessMouseEvent();
            if (!this.eventSystem.sendNavigationEvents)
                return;
            if (!selectedObject)
                selectedObject |= this.SendMoveEventToSelectedObject();
            if (selectedObject)
                return;
            this.SendSubmitEventToSelectedObject();
        }

        private bool ProcessTouchEvents()
        {
            for (int index = 0; index < this.input.touchCount; ++index)
            {
                Touch touch = this.input.GetTouch(index);
                if (touch.type != TouchType.Indirect)
                {
                    bool pressed;
                    bool released;
                    MultiMousePointerEventData pointerEventData = this.GetTouchPointerEventData(touch, out pressed, out released);
                    this.ProcessTouchPress((PointerEventData)pointerEventData, pressed, released);
                    if (!released)
                    {
                        this.ProcessMove(pointerEventData);
                        this.ProcessDrag(pointerEventData);
                    }
                    else
                        this.RemovePointerData(pointerEventData);
                }
            }
            return this.input.touchCount > 0;
        }

        protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
        {
            GameObject gameObject1 = pointerEvent.pointerCurrentRaycast.gameObject;
            if (pressed)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
                this.DeselectIfSelectionChanged(gameObject1, (BaseEventData)pointerEvent);
                if ((UnityEngine.Object)pointerEvent.pointerEnter != (UnityEngine.Object)gameObject1)
                {
                    this.HandlePointerExitAndEnter(pointerEvent, gameObject1);
                    pointerEvent.pointerEnter = gameObject1;
                }
                GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData)pointerEvent, ExecuteEvents.pointerDownHandler);
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)null)
                    gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
                float unscaledTime = Time.unscaledTime;
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)pointerEvent.lastPress)
                {
                    if ((double)unscaledTime - (double)pointerEvent.clickTime < 0.300000011920929)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;
                    pointerEvent.clickTime = unscaledTime;
                }
                else
                    pointerEvent.clickCount = 1;
                pointerEvent.pointerPress = gameObject2;
                pointerEvent.rawPointerPress = gameObject1;
                pointerEvent.clickTime = unscaledTime;
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
                if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null)
                    ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.initializePotentialDrag);
                this.m_InputPointerEvent = pointerEvent;
            }
            if (!released)
                return;
            ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerUpHandler);
            GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
            if ((UnityEngine.Object)pointerEvent.pointerPress == (UnityEngine.Object)eventHandler && pointerEvent.eligibleForClick)
                ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerClickHandler);
            else if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData)pointerEvent, ExecuteEvents.dropHandler);
            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = (GameObject)null;
            pointerEvent.rawPointerPress = (GameObject)null;
            if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.endDragHandler);
            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = (GameObject)null;
            ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, (BaseEventData)pointerEvent, ExecuteEvents.pointerExitHandler);
            pointerEvent.pointerEnter = (GameObject)null;
            this.m_InputPointerEvent = pointerEvent;
        }

        protected bool SendSubmitEventToSelectedObject()
        {
            if ((UnityEngine.Object)this.eventSystem.currentSelectedGameObject == (UnityEngine.Object)null)
                return false;
            BaseEventData baseEventData = this.GetBaseEventData();
            if (this.input.GetButtonDown(this.m_SubmitButton))
                ExecuteEvents.Execute<ISubmitHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
            if (this.input.GetButtonDown(this.m_CancelButton))
                ExecuteEvents.Execute<ICancelHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
            return baseEventData.used;
        }

        private Vector2 GetRawMoveVector()
        {
            Vector2 zero = Vector2.zero;
            zero.x = this.input.GetAxisRaw(this.m_HorizontalAxis);
            zero.y = this.input.GetAxisRaw(this.m_VerticalAxis);
            if (this.input.GetButtonDown(this.m_HorizontalAxis))
            {
                if ((double)zero.x < 0.0)
                    zero.x = -1f;
                if ((double)zero.x > 0.0)
                    zero.x = 1f;
            }
            if (this.input.GetButtonDown(this.m_VerticalAxis))
            {
                if ((double)zero.y < 0.0)
                    zero.y = -1f;
                if ((double)zero.y > 0.0)
                    zero.y = 1f;
            }
            return zero;
        }

        protected bool SendMoveEventToSelectedObject()
        {
            float unscaledTime = Time.unscaledTime;
            Vector2 rawMoveVector = this.GetRawMoveVector();
            if (Mathf.Approximately(rawMoveVector.x, 0.0f) && Mathf.Approximately(rawMoveVector.y, 0.0f))
            {
                this.m_ConsecutiveMoveCount = 0;
                return false;
            }
            bool flag = (double)Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0.0;
            if (flag && this.m_ConsecutiveMoveCount == 1)
            {
                if ((double)unscaledTime <= (double)this.m_PrevActionTime + (double)this.m_RepeatDelay)
                    return false;
            }
            else if ((double)unscaledTime <= (double)this.m_PrevActionTime + 1.0 / (double)this.m_InputActionsPerSecond)
                return false;
            AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
            if (axisEventData.moveDir != MoveDirection.None)
            {
                ExecuteEvents.Execute<IMoveHandler>(this.eventSystem.currentSelectedGameObject, (BaseEventData)axisEventData, ExecuteEvents.moveHandler);
                if (!flag)
                    this.m_ConsecutiveMoveCount = 0;
                ++this.m_ConsecutiveMoveCount;
                this.m_PrevActionTime = unscaledTime;
                this.m_LastMoveVector = rawMoveVector;
            }
            else
                this.m_ConsecutiveMoveCount = 0;
            return axisEventData.used;
        }

        protected virtual void ProcessMouseEvent()
        {
            for (int id = 0; id < MultiMouseManager.Instance.Mice.Count; ++id)
                this.ProcessMouseEvent(id);
        }

        [Obsolete("This method is no longer checked, overriding it with return true does nothing!")]
        protected virtual bool ForceAutoSelect() => false;

        protected virtual void ProcessMouseEvent(int id)
        {
            MultiMousePointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(id);
            MultiMousePointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
            this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
            this.ProcessMousePress(eventData);
            this.ProcessMove(eventData.buttonData);
            this.ProcessDrag(eventData.buttonData);
            this.ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
            this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
            this.ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
            this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
            if (Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
                return;
            ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData)eventData.buttonData, ExecuteEvents.scrollHandler);
        }

        protected bool SendUpdateEventToSelectedObject()
        {
            if ((UnityEngine.Object)this.eventSystem.currentSelectedGameObject == (UnityEngine.Object)null)
                return false;
            BaseEventData baseEventData = this.GetBaseEventData();
            ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
            return baseEventData.used;
        }

        protected void ProcessMousePress(
          MultiMousePointerInputModule.MouseButtonEventData data)
        {
            MultiMousePointerEventData buttonData = data.buttonData;
            GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
            if (data.PressedThisFrame())
            {
                buttonData.eligibleForClick = true;
                buttonData.delta = Vector2.zero;
                buttonData.dragging = false;
                buttonData.useDragThreshold = true;
                buttonData.pressPosition = buttonData.position;
                buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
                this.DeselectIfSelectionChanged(gameObject1, (BaseEventData)buttonData);
                GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData)buttonData, ExecuteEvents.pointerDownHandler);
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)null)
                    gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
                float unscaledTime = Time.unscaledTime;
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)buttonData.lastPress)
                {
                    if ((double)unscaledTime - (double)buttonData.clickTime < 0.300000011920929)
                        ++buttonData.clickCount;
                    else
                        buttonData.clickCount = 1;
                    buttonData.clickTime = unscaledTime;
                }
                else
                    buttonData.clickCount = 1;
                buttonData.pointerPress = gameObject2;
                buttonData.rawPointerPress = gameObject1;
                buttonData.clickTime = unscaledTime;
                buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
                if ((UnityEngine.Object)buttonData.pointerDrag != (UnityEngine.Object)null)
                    ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData)buttonData, ExecuteEvents.initializePotentialDrag);
                this.m_InputPointerEvent = (PointerEventData)buttonData;
            }
            if (!data.ReleasedThisFrame())
                return;
            this.ReleaseMouse((PointerEventData)buttonData, gameObject1);
        }

        protected GameObject GetCurrentFocusedGameObject() => this.m_CurrentFocusedGameObject;

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public enum InputMode
        {
            Mouse,
            Buttons,
        }
    }
}
