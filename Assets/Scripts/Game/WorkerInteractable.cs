using System;
using System.Collections;
using System.Collections.Generic;
using General;
using Interactions;
using MultiMouse;
using UnityEngine;
using UnityEngine.EventSystems;
using WorkStations;
using Cursor = Cursors.Cursor;

namespace Gameplay
{
    public class WorkerInteractable : Clickable
    {
        public static event Action<SpitProjectileActionData> OnSpitStarted;

        public struct SpitProjectileActionData
        {
            public float flightDuration;
            public Vector3 startPosition;
            public Vector3 destination;
        }
            
        public enum WorkingLevelType
        {
            Asleep,
            Normal,
            Fast
        }

        [Serializable]
        public class WorkingLevel
        {
            [field: SerializeField]
            public int ClickThreshold { get; private set; }
            [field: SerializeField]
            public float ActionRate { get; private set; }
            [field: SerializeField]
            public WorkingLevelType WorkingLeveLType { get; private set; }
            [field: SerializeField]
            public float SecondsToWaitBeforeClickCountDecreases { get; private set; }

            public WorkingLevel(int clickThreshold, float actionRate, WorkingLevelType workingLeveLType)
            {
                ClickThreshold = clickThreshold;
                ActionRate = actionRate;
                WorkingLeveLType = workingLeveLType;
            }
        }

        [Serializable]
        public class WorkingLevelsHandler
        {
            [field: SerializeField]
            public List<WorkingLevel> WorkingLevels { get; private set; }

            private WorkingLevel currentWorkingLevel;

            public WorkingLevel CurrentWorkingLevel
            {
                get { return currentWorkingLevel; }
                private set
                {
                    currentWorkingLevel = value;
                    OnWorkingLevelChanged?.Invoke(currentWorkingLevel);
                }
            }

            [field: SerializeField]
            public int StartingLevelIndex { get; private set; } = 1;

            private int currentIndex;
            public event Action<WorkingLevel> OnWorkingLevelChanged;

            public WorkingLevelsHandler(List<WorkingLevel> workingLevels, WorkingLevel currentWorkingLevel, int startingLevelIndex)
            {
                WorkingLevels = workingLevels;
                CurrentWorkingLevel = currentWorkingLevel;
                StartingLevelIndex = startingLevelIndex;
            }

            public void SetDefaultWorkingLevel()
            {
                currentIndex = StartingLevelIndex;
                CurrentWorkingLevel = WorkingLevels[StartingLevelIndex];
            }

            public void UpdateWorkingLevels(int clicks)
            {
                if (clicks < CurrentWorkingLevel.ClickThreshold && currentIndex != 0)
                {
                    currentIndex--;
                    CurrentWorkingLevel = WorkingLevels[currentIndex];
                }
                else if (currentIndex < WorkingLevels.Count - 1 && clicks >= WorkingLevels[currentIndex + 1].ClickThreshold)
                {
                    currentIndex++;
                    CurrentWorkingLevel = WorkingLevels[currentIndex];
                }
            }
        }

        public enum WorkerState
        {
            Idle,
            Carried
        }

        public static Action<WorkerInteractable, IWorkStations> plucieEvent;

        [SerializeField]
        private Rigidbody rigidbody;
        [SerializeField]
        private WorkingLevelsHandler workingLevelsHandler;
        [SerializeField]
        private float holdTime = 0.4f;
        [SerializeField]
        private LayerMask layer;
        
        [SerializeField]
        private Transform spitProjectileSpawnPoint;

        private MultiMouseDevice attachedMouse;
        private Cursor attachedCursor;
        private WorkerState workerState = WorkerState.Idle;
        private IWorkStations workStation;
        private bool isHold = false;
        private bool isHolded = false;
        private float lastStateChangeTime;

        public WorkingLevelsHandler WorkingLevelHandler => workingLevelsHandler;

        private string DebugTag => $"[{nameof(WorkerInteractable)}]";

        public static float SpitProjectileFlyTime = 0.33f;

        private void Awake()
        {
            GameManager.Instance.Workers.Add(this);
            workingLevelsHandler.OnWorkingLevelChanged += OnWorkingLevelChanged;
            workingLevelsHandler.SetDefaultWorkingLevel();
            StartCoroutine(UseUse());
        }

        private void OnDestroy()
        {
            GameManager.Instance.Workers.Remove(this);
        }

        private IEnumerator UseUse()
        {
            while (true)
            {
                if (workStation != null && workerState == WorkerState.Idle && currentClickCount > 0 && workingLevelsHandler.CurrentWorkingLevel.WorkingLeveLType != WorkingLevelType.Asleep)
                {
                    GetComponent<Animator>().SetTrigger("Spit");
                    plucieEvent?.Invoke(this, workStation);
                    //workStation.Use();
                    //AudioManager.Instance.Sfxs.Pop.Play();
                }

                yield return new WaitForSeconds(workingLevelsHandler.CurrentWorkingLevel.ActionRate);
            }
        }

        private void OnWorkingLevelChanged(WorkingLevel workingLevel)
        {
            lastStateChangeTime = Time.time;
        }

        protected override void Update()
        {
            if (Time.time - lastStateChangeTime > workingLevelsHandler.CurrentWorkingLevel.SecondsToWaitBeforeClickCountDecreases)
            {
                base.Update();
            }

            UpdateCarryPosition();
            workingLevelsHandler.UpdateWorkingLevels(currentClickCount);
        }

        public override void OnCursorClick(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorDown(MultiMousePointerEventData eventData)
        {
            isHold = true;
            cursorDownTime = Time.time;
            AudioManager.Instance.Sfxs.Slap.Play();
        }

        public override void OnCursorUp(MultiMousePointerEventData eventData)
        {
            if (isHolded)
            {
                eventData.MouseDevice.Position = Camera.main.WorldToScreenPoint(SnapPivot.position);

                if (attachedMouse == eventData.MouseDevice)
                {
                    Drop();
                    attachedMouse = null;
                    workerState = WorkerState.Idle;
                }

                Ray ray = new Ray(transform.position, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit, 999, layer))
                {
                    IWorkStations oldWorkStation = workStation;
                    workStation = hit.collider.GetComponent<IWorkStations>();

                    if (workStation == null)
                    {
                        workStation = hit.collider.GetComponentInParent<IWorkStations>();
                    }

                    if (oldWorkStation != workStation && oldWorkStation != null)
                    {
                        oldWorkStation.RemoveWorker(this);
                    }

                    if (workStation != null)
                    {
                        workStation.AddWorker(this);
                        Debug.Log("found");
                    }
                }
            }

            isHold = false;
            isHolded = false;
        }

        private float cursorDownTime;

        public override void OnCursorDrag(MultiMousePointerEventData eventData)
        {
            if ((Time.time - cursorDownTime) > holdTime && isHold)
            {
                if (isHolded == false)
                {
                    if (attachedMouse == null)
                    {
                        attachedMouse = eventData.MouseDevice;
                        attachedCursor = GameManager.Instance.CursorsManager.MouseToCursorDictionary[attachedMouse];
                        workerState = WorkerState.Carried;
                        Pickup();
                    }
                }

                isHolded = true;
            }
        }

        public override void OnCursorEnter(MultiMousePointerEventData eventData)
        {
        }

        public override void OnCursorExit(MultiMousePointerEventData eventData)
        {
        }

        private void Pickup()
        {
            rigidbody.isKinematic = true;
        }

        private void UpdateCarryPosition()
        {
            if (attachedMouse == null) return;

            float offset = 2;
            Vector3 position = attachedCursor.transform.position;
            position.y += offset;
            transform.position = position;
        }

        private void Drop()
        {
            rigidbody.isKinematic = false;
        }

        public void AnimationSpitTriggered()
        {
            StartCoroutine(SpitProjectile());
        }

        private IEnumerator SpitProjectile()
        {
            Debug.Log($"Spit Start");
            AudioManager.Instance.Sfxs.Pop.Play();
            Vector3 startPoint = spitProjectileSpawnPoint.position;
            Vector3 endPoint = (workStation as MonoBehaviour).transform.position - Vector3.up*.5f;

            OnSpitStarted?.Invoke(new SpitProjectileActionData()
            {
                startPosition = startPoint,
                destination = endPoint,
                flightDuration = SpitProjectileFlyTime
            });
            yield return new WaitForSeconds(SpitProjectileFlyTime);
            Debug.Log($"Spit Hit");
            workStation.Use();
        }
    }
}