using System;
using System.Collections.Generic;
using Cursors;
using Gameplay;
using Interactions;
using MultiMouse;
using UnityEngine;
using UnityEngine.EventSystems;
using WorkStations;

namespace General
{
    public struct WorkerSpawnData
    {
        public WorkerInteractable Worker;
        public int WorekrsAmount;

        public WorkerSpawnData(WorkerInteractable worker, int worekrsAmount)
        {
            Worker = worker;
            WorekrsAmount = worekrsAmount;
        }
    }

    public class GameManager : SingletonUnity<GameManager>
    {
        public struct ClickData
        {
            public IInteractable interactable;
            public MultiMousePointerEventData eventData;
        }

        public Action<ClickData> OnBeforeInteractableClicked;
        public Action<ClickData> OnAfterInteractableClicked;
        public Action<WorkerSpawnData> OnWorkerSpawned;
        public Action<int> OnGoldChange;

        [field: SerializeField]
        public CursorsManager CursorsManager { get; private set; }
        [field: SerializeField]
        public CameraController CameraController { get; private set; }
        public UIManager UIManager;

        public Dictionary<MultiMouseDevice, IInteractable> Hovers;
        private int gold = 0;

        public int Gold
        {
            get { return gold; }
            set
            {
                gold = value;
                OnGoldChange?.Invoke(gold);
            }
        }

        public HashSet<WorkerInteractable> Workers;
        public WorkerInteractable wokrerPrefab;
        public HashSet<IWorkStations> WorkStations;

        private Base _playerBase;

        protected override void Awake()
        {
            Hovers = new Dictionary<MultiMouseDevice, IInteractable>();
            Workers = new HashSet<WorkerInteractable>();
            WorkStations = new HashSet<IWorkStations>();
            _playerBase = GameObject.FindWithTag("Base").GetComponent<Base>();
            _playerBase.OnDeath += EndGame;
        }

        public WorkerInteractable CreateWorker(Vector3 position, Quaternion rotation)
        {
            WorkerInteractable worker = Instantiate(wokrerPrefab, position, rotation);
            WorkerSpawnData workerData = new WorkerSpawnData(wokrerPrefab, Workers.Count);
            OnWorkerSpawned?.Invoke(workerData);
            return worker;
        }

        private void EndGame()
        {
            _playerBase.OnDeath -= EndGame;
            UIManager.ShowEndgameScreen();
        }
    }
}