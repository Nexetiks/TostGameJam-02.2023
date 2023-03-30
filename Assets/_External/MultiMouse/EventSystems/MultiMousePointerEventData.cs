using MultiMouse;

namespace UnityEngine.EventSystems
{
    public class MultiMousePointerEventData : PointerEventData
    {
        private int mouseModelId;

        public int MouseDeviceId => this.mouseModelId;

        public MultiMouseDevice MouseDevice => MultiMouseManager.Instance.Mice[this.mouseModelId];

        public MultiMousePointerEventData(EventSystem eventSystem, int mouseModelId)
          : base(eventSystem)
          => this.mouseModelId = mouseModelId;
    }
}
