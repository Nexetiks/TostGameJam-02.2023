using System.Collections.Generic;
using Gameplay;
using General;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WorkStations
{
    public class ExampleWorkStation : MonoBehaviour, IWorkStations
    {
        public Transform SnapPivot { get; }
        public List<WorkerInteractable> WorkersOnStation { get; set; }

        public void OnCursorEnter(MultiMousePointerEventData eventData)
        {
        }

        public void OnCursorExit(MultiMousePointerEventData eventData)
        {
        }

        public void AddWorker(WorkerInteractable worker)
        {
            GameManager.Instance.WorkStations.Add(this);
        }

        public void RemoveWorker(WorkerInteractable worker)
        {
            GameManager.Instance.WorkStations.Remove(this);
        }

        public void Use()
        {
        }
    }
}