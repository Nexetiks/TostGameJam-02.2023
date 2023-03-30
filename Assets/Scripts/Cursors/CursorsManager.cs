using MultiMouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cursors
{
    public class CursorsManager : MonoBehaviour
    {
        public GameObject CursorsHolder;
        public Dictionary<MultiMouseDevice, Cursor> MouseToCursorDictionary { get; private set; } = new Dictionary<MultiMouseDevice, Cursor>();

        [SerializeField]
        private Cursor _cursorPrefab;

        private MultiMouseManager _multiMouseManager => MultiMouseManager.Instance;
        private ulong _cursorCreationsCounter = 0;

        private string DebugTag => $"[{nameof(CursorsManager)}]";

        private void OnEnable()
        {
            _multiMouseManager.OnMouseConnected += MultiMouseManager_OnMouseConnected;
            _multiMouseManager.OnMouseDisconnected += MultiMouseManager_OnMouseDisconnected;
        }

        private void OnDisable()
        {
            _multiMouseManager.OnMouseConnected -= MultiMouseManager_OnMouseConnected;
            _multiMouseManager.OnMouseDisconnected += MultiMouseManager_OnMouseDisconnected;
        }

        private void MultiMouseManager_OnMouseConnected(MultiMouseDevice mouse)
        {
            InitializeCursorWithRandomColor(mouse);
        }

        private void MultiMouseManager_OnMouseDisconnected(MultiMouseDevice mouse)
        {
            RemoveCursor(mouse);
        }

        private void InitializeCursorWithRandomColor(MultiMouseDevice mouse)
        {
            _cursorCreationsCounter++;
            string name = $"Cursor {_cursorCreationsCounter}";
            Color color = Random.ColorHSV();
            Debug.Log($"{DebugTag} Creating new cursor: {name} for mouse {mouse.Name}");
            Cursor newCursor = Instantiate(_cursorPrefab, CursorsHolder.transform);
            newCursor.Initialize(mouse, $"Cursor {_cursorCreationsCounter}");
            newCursor.SetColor(Random.ColorHSV());
            MouseToCursorDictionary.Add(mouse, newCursor);
        }

        private void RemoveCursor(MultiMouseDevice mouse)
        {
            if (!MouseToCursorDictionary.ContainsKey(mouse))
            {
                Debug.LogWarning($"Mouse {mouse.Name} does not have a cursor, nothing will be removed.");
                return;
            }

            Debug.Log($"{DebugTag} Removing mouse {mouse.Name}");

            var cursor = MouseToCursorDictionary[mouse];
            cursor.Clean();
            MouseToCursorDictionary.Remove(mouse);
            Destroy(cursor.gameObject);
        }
    }
}
