using System.Collections.Generic;
using Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using WorkStations;

namespace Interactions.Interactable
{
    public class Mine : Clickable, IWorkStations
    {
        [SerializeField]
        private Gatherable gatherable;

        public List<WorkerInteractable> WorkersOnStation { get; set; } = new List<WorkerInteractable>();

        private void Awake()
        {
            OnCounterMaxValue += CreateGatherable;
        }

        private void OnDestroy()
        {
            OnCounterMaxValue -= CreateGatherable;
        }

        public override void OnCursorEnter(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorExit(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorClick(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorUp(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorDown(MultiMousePointerEventData eventData)
        {
            AudioManager.Instance.Sfxs.Rock.Play();
        }

        public override void OnCursorDrag(MultiMousePointerEventData eventData)
        {
        }

        public void AddWorker(WorkerInteractable worker)
        {
            WorkersOnStation.Add(worker);
        }

        public void RemoveWorker(WorkerInteractable worker)
        {
            WorkersOnStation.Remove(worker);
        }

        public void Use()
        {
            CurrentClickCount++;
        }

        private void CreateGatherable()
        {
            Vector3 myPosition = transform.position;
            Gatherable instantiatedGatherable = Instantiate(gatherable, transform.position, quaternion.identity);
        }
    }
}