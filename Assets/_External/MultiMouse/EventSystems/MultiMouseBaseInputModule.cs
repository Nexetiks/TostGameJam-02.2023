using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
    [RequireComponent(typeof(EventSystem))]
    public abstract class MultiMouseBaseInputModule : BaseInputModule
    {
        private AxisEventData m_AxisEventData;
        private BaseEventData m_BaseEventData;


        protected new void HandlePointerExitAndEnter(
          PointerEventData currentPointerData,
          GameObject newEnterTarget)
        {
            if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null || (UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)null)
            {
                for (int index = 0; index < currentPointerData.hovered.Count; ++index)
                    ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[index], (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
                currentPointerData.hovered.Clear();
                if ((UnityEngine.Object)newEnterTarget == (UnityEngine.Object)null)
                {
                    currentPointerData.pointerEnter = (GameObject)null;
                    return;
                }
            }
            if ((UnityEngine.Object)currentPointerData.pointerEnter == (UnityEngine.Object)newEnterTarget && (bool)(UnityEngine.Object)newEnterTarget)
                return;
            GameObject commonRoot = MultiMouseBaseInputModule.FindCommonRoot(currentPointerData.pointerEnter, newEnterTarget);
            if ((UnityEngine.Object)currentPointerData.pointerEnter != (UnityEngine.Object)null)
            {
                for (Transform transform = currentPointerData.pointerEnter.transform; (UnityEngine.Object)transform != (UnityEngine.Object)null && (!((UnityEngine.Object)commonRoot != (UnityEngine.Object)null) || !((UnityEngine.Object)commonRoot.transform == (UnityEngine.Object)transform)); transform = transform.parent)
                {
                    ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, (BaseEventData)currentPointerData, ExecuteEvents.pointerExitHandler);
                    currentPointerData.hovered.Remove(transform.gameObject);
                }
            }
            currentPointerData.pointerEnter = newEnterTarget;
            if (!((UnityEngine.Object)newEnterTarget != (UnityEngine.Object)null))
                return;
            for (Transform transform = newEnterTarget.transform; (UnityEngine.Object)transform != (UnityEngine.Object)null && (UnityEngine.Object)transform.gameObject != (UnityEngine.Object)commonRoot; transform = transform.parent)
            {
                ExecuteEvents.Execute<IPointerEnterHandler>(transform.gameObject, (BaseEventData)currentPointerData, ExecuteEvents.pointerEnterHandler);
                currentPointerData.hovered.Add(transform.gameObject);
            }
        }

        protected override AxisEventData GetAxisEventData(
          float x,
          float y,
          float moveDeadZone)
        {
            if (this.m_AxisEventData == null)
                this.m_AxisEventData = new AxisEventData(this.eventSystem);
            this.m_AxisEventData.Reset();
            this.m_AxisEventData.moveVector = new Vector2(x, y);
            this.m_AxisEventData.moveDir = MultiMouseBaseInputModule.DetermineMoveDirection(x, y, moveDeadZone);
            return this.m_AxisEventData;
        }

        protected override BaseEventData GetBaseEventData()
        {
            if (this.m_BaseEventData == null)
                this.m_BaseEventData = new BaseEventData(this.eventSystem);
            this.m_BaseEventData.Reset();
            return this.m_BaseEventData;
        }

        public override bool IsPointerOverGameObject(int pointerId) => false;

        public override bool ShouldActivateModule() => this.enabled && this.gameObject.activeInHierarchy;

        public override void DeactivateModule()
        {
        }

        public override void ActivateModule()
        {
        }

        public override void UpdateModule()
        {
        }

        public override bool IsModuleSupported() => true;
    }
}
