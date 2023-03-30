using UnityEngine;
using UnityEngine.EventSystems;

namespace Interactions.Interactable
{
    public class TestInheritanceClickable : Clickable
    {
        public override void OnCursorEnter(MultiMousePointerEventData eventData)
        {
            Debug.Log("TestInheritanceClickable OnCursorEnter");
        }

        public override void OnCursorExit(MultiMousePointerEventData eventData)
        {
            Debug.Log("TestInheritanceClickable OnCursorExit");
        }

        public override void OnCursorClick(MultiMousePointerEventData eventData)
        {
            Debug.Log("TestInheritanceClickable OnCursorClick");
        }

        public override void OnCursorUp(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorDown(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorDrag(MultiMousePointerEventData eventData)
        {
        }
    }
}