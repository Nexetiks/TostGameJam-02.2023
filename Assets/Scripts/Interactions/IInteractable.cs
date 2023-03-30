using System;
using General;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions
{
    public interface IInteractable : IPointerEnterHandler, IPointerExitHandler
    {
        Transform SnapPivot { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        String name { get; }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;
            GameManager.Instance.Hovers[data.MouseDevice] = this;
            OnCursorEnter(data);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            MultiMousePointerEventData data = eventData as MultiMousePointerEventData;

            if (GameManager.Instance.Hovers.TryGetValue(data.MouseDevice, out IInteractable interactable) && interactable == this)
            {
                GameManager.Instance.Hovers.Remove(data.MouseDevice);
            }

            OnCursorExit(data);
        }

        void OnCursorEnter(MultiMousePointerEventData eventData);
        void OnCursorExit(MultiMousePointerEventData eventData);
    }
}