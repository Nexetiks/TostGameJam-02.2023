using MultiMouse;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.EventSystems
{
    public abstract class MultiMousePointerInputModule : MultiMouseBaseInputModule
    {
        public const int kMouseLeftId = -1;
        public const int kMouseRightId = -2;
        public const int kMouseMiddleId = -3;
        public const int kFakeTouchesId = -4;
        protected Dictionary<int, Dictionary<int, MultiMousePointerEventData>> m_PointerData = new Dictionary<int, Dictionary<int, MultiMousePointerEventData>>();
        private readonly MultiMousePointerInputModule.MouseState m_MouseState = new MultiMousePointerInputModule.MouseState();

        protected bool GetPointerData(
          int id,
          out MultiMousePointerEventData data,
          bool create,
          int mouseId)
        {
            Dictionary<int, MultiMousePointerEventData> dictionary1;
            if (this.m_PointerData.TryGetValue(mouseId, out dictionary1))
            {
                if (dictionary1.TryGetValue(id, out data) || !create)
                    return false;
                ref MultiMousePointerEventData local = ref data;
                MultiMousePointerEventData pointerEventData = new MultiMousePointerEventData(this.eventSystem, mouseId);
                pointerEventData.pointerId = id;
                local = pointerEventData;
                dictionary1.Add(id, data);
                return true;
            }
            if (create)
            {
                Dictionary<int, MultiMousePointerEventData> dictionary2 = new Dictionary<int, MultiMousePointerEventData>();
                MultiMousePointerEventData pointerEventData = new MultiMousePointerEventData(this.eventSystem, mouseId);
                pointerEventData.pointerId = id;
                data = pointerEventData;
                dictionary2.Add(id, data);
                this.m_PointerData.Add(mouseId, dictionary2);
                return true;
            }
            data = (MultiMousePointerEventData)null;
            return false;
        }

        protected void RemovePointerData(MultiMousePointerEventData data) => this.m_PointerData.Remove(data.pointerId);

        protected MultiMousePointerEventData GetTouchPointerEventData(
          Touch input,
          out bool pressed,
          out bool released)
        {
            MultiMousePointerEventData data;
            bool pointerData = this.GetPointerData(input.fingerId, out data, true, 0);
            data.Reset();
            pressed = pointerData || input.phase == TouchPhase.Began;
            released = input.phase == TouchPhase.Canceled || input.phase == TouchPhase.Ended;
            if (pointerData)
                data.position = input.position;
            if (pressed)
                data.delta = Vector2.zero;
            else
                data.delta = input.position - data.position;
            data.position = input.position;
            data.button = PointerEventData.InputButton.Left;
            if (input.phase == TouchPhase.Canceled)
            {
                data.pointerCurrentRaycast = new RaycastResult();
            }
            else
            {
                this.eventSystem.RaycastAll((PointerEventData)data, this.m_RaycastResultCache);
                RaycastResult firstRaycast = MultiMouseBaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
                data.pointerCurrentRaycast = firstRaycast;
                this.m_RaycastResultCache.Clear();
            }
            return data;
        }

        protected void CopyFromTo(MultiMousePointerEventData from, MultiMousePointerEventData to)
        {
            to.position = from.position;
            to.delta = from.delta;
            to.scrollDelta = from.scrollDelta;
            to.pointerCurrentRaycast = from.pointerCurrentRaycast;
            to.pointerEnter = from.pointerEnter;
        }

        protected PointerEventData.FramePressState StateForMouseButton(
          int buttonId,
          int mouseId)
        {
            MultiMouseDevice mouse = MultiMouseManager.Instance.Mice[mouseId];
            bool mouseButtonDown = mouse.GetMouseButtonDown(buttonId);
            bool mouseButtonUp = mouse.GetMouseButtonUp(buttonId);
            if (mouseButtonDown & mouseButtonUp)
                return PointerEventData.FramePressState.PressedAndReleased;
            if (mouseButtonDown)
                return PointerEventData.FramePressState.Pressed;
            return mouseButtonUp ? PointerEventData.FramePressState.Released : PointerEventData.FramePressState.NotChanged;
        }

        protected virtual MultiMousePointerInputModule.MouseState GetMousePointerEventData() => this.GetMousePointerEventData(0);

        protected virtual MultiMousePointerInputModule.MouseState GetMousePointerEventData(
          int id)
        {
            MultiMouseDevice mouse = MultiMouseManager.Instance.Mice[id];
            MultiMousePointerEventData data1;
            int num = this.GetPointerData(-1, out data1, true, id) ? 1 : 0;
            data1.Reset();
            if (num != 0)
                data1.position = mouse.Position;
            Vector2 position = mouse.Position;
            data1.delta = position - data1.position;
            data1.position = position;
            data1.scrollDelta = mouse.ScrollDelta;
            data1.button = PointerEventData.InputButton.Left;
            this.eventSystem.RaycastAll((PointerEventData)data1, this.m_RaycastResultCache);
            RaycastResult firstRaycast = MultiMouseBaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
            data1.pointerCurrentRaycast = firstRaycast;
            this.m_RaycastResultCache.Clear();
            MultiMousePointerEventData data2;
            this.GetPointerData(-2, out data2, true, id);
            this.CopyFromTo(data1, data2);
            data2.button = PointerEventData.InputButton.Right;
            MultiMousePointerEventData data3;
            this.GetPointerData(-3, out data3, true, id);
            this.CopyFromTo(data1, data3);
            data3.button = PointerEventData.InputButton.Middle;
            this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, this.StateForMouseButton(0, id), data1);
            this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, this.StateForMouseButton(1, id), data2);
            this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, this.StateForMouseButton(2, id), data3);
            return this.m_MouseState;
        }

        protected MultiMousePointerEventData GetLastPointerEventData(int id)
        {
            MultiMousePointerEventData data;
            this.GetPointerData(id, out data, false, id);
            return data;
        }

        private static bool ShouldStartDrag(
          Vector2 pressPos,
          Vector2 currentPos,
          float threshold,
          bool useDragThreshold)
        {
            return !useDragThreshold || (double)(pressPos - currentPos).sqrMagnitude >= (double)threshold * (double)threshold;
        }

        protected virtual void ProcessMove(MultiMousePointerEventData pointerEvent)
        {
            GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
            this.HandlePointerExitAndEnter((PointerEventData)pointerEvent, gameObject);
        }

        protected virtual void ProcessDrag(MultiMousePointerEventData pointerEvent)
        {
            if (!pointerEvent.IsPointerMoving() || (Object)pointerEvent.pointerDrag == (Object)null)
                return;
            if (!pointerEvent.dragging && MultiMousePointerInputModule.ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, (float)this.eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
            {
                ExecuteEvents.Execute<IBeginDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.beginDragHandler);
                pointerEvent.dragging = true;
            }
            if (!pointerEvent.dragging)
                return;
            if ((Object)pointerEvent.pointerPress != (Object)pointerEvent.pointerDrag)
            {
                ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerUpHandler);
                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = (GameObject)null;
                pointerEvent.rawPointerPress = (GameObject)null;
            }
            ExecuteEvents.Execute<IDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.dragHandler);
        }

        public override bool IsPointerOverGameObject(int pointerId)
        {
            MultiMousePointerEventData pointerEventData = this.GetLastPointerEventData(pointerId);
            return pointerEventData != null && (Object)pointerEventData.pointerEnter != (Object)null;
        }

        protected void ClearSelection()
        {
            BaseEventData baseEventData = this.GetBaseEventData();
            foreach (Dictionary<int, MultiMousePointerEventData> dictionary in this.m_PointerData.Values)
            {
                foreach (PointerEventData currentPointerData in dictionary.Values)
                    this.HandlePointerExitAndEnter(currentPointerData, (GameObject)null);
            }
            this.m_PointerData.Clear();
            this.eventSystem.SetSelectedGameObject((GameObject)null, baseEventData);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("<b>Pointer Input Module of type: </b>" + (object)this.GetType());
            stringBuilder.AppendLine();
            foreach (KeyValuePair<int, Dictionary<int, MultiMousePointerEventData>> keyValuePair in this.m_PointerData)
            {
                if (keyValuePair.Value != null)
                {
                    stringBuilder.AppendLine("<B>Pointer:</b> " + (object)keyValuePair.Key);
                    stringBuilder.AppendLine(keyValuePair.Value.ToString());
                }
            }
            return stringBuilder.ToString();
        }

        protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
        {
            if (!((Object)ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo) != (Object)this.eventSystem.currentSelectedGameObject))
                return;
            this.eventSystem.SetSelectedGameObject((GameObject)null, pointerEvent);
        }

        protected class ButtonState
        {
            private PointerEventData.InputButton m_Button;
            private MultiMousePointerInputModule.MouseButtonEventData m_EventData;

            public MultiMousePointerInputModule.MouseButtonEventData eventData
            {
                get => this.m_EventData;
                set => this.m_EventData = value;
            }

            public PointerEventData.InputButton button
            {
                get => this.m_Button;
                set => this.m_Button = value;
            }
        }

        protected class MouseState
        {
            private List<MultiMousePointerInputModule.ButtonState> m_TrackedButtons = new List<MultiMousePointerInputModule.ButtonState>();

            public bool AnyPressesThisFrame()
            {
                for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
                {
                    if (this.m_TrackedButtons[index].eventData.PressedThisFrame())
                        return true;
                }
                return false;
            }

            public bool AnyReleasesThisFrame()
            {
                for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
                {
                    if (this.m_TrackedButtons[index].eventData.ReleasedThisFrame())
                        return true;
                }
                return false;
            }

            public MultiMousePointerInputModule.ButtonState GetButtonState(
              PointerEventData.InputButton button)
            {
                MultiMousePointerInputModule.ButtonState buttonState = (MultiMousePointerInputModule.ButtonState)null;
                for (int index = 0; index < this.m_TrackedButtons.Count; ++index)
                {
                    if (this.m_TrackedButtons[index].button == button)
                    {
                        buttonState = this.m_TrackedButtons[index];
                        break;
                    }
                }
                if (buttonState == null)
                {
                    buttonState = new MultiMousePointerInputModule.ButtonState()
                    {
                        button = button,
                        eventData = new MultiMousePointerInputModule.MouseButtonEventData()
                    };
                    this.m_TrackedButtons.Add(buttonState);
                }
                return buttonState;
            }

            public void SetButtonState(
              PointerEventData.InputButton button,
              PointerEventData.FramePressState stateForMouseButton,
              MultiMousePointerEventData data)
            {
                MultiMousePointerInputModule.ButtonState buttonState = this.GetButtonState(button);
                buttonState.eventData.buttonState = stateForMouseButton;
                buttonState.eventData.buttonData = data;
            }
        }

        public class MouseButtonEventData
        {
            public PointerEventData.FramePressState buttonState;
            public MultiMousePointerEventData buttonData;

            public bool PressedThisFrame() => this.buttonState == PointerEventData.FramePressState.Pressed || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;

            public bool ReleasedThisFrame() => this.buttonState == PointerEventData.FramePressState.Released || this.buttonState == PointerEventData.FramePressState.PressedAndReleased;
        }
    }
}
