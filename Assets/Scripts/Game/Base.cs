using System;
using General;
using Interactions;
using NoGround.Character;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class Base : MonoBehaviour, IClickable, ITarget
    {
        public event Action<float, float> OnHit;
        public event Action OnDeath;

        [SerializeField]
        private HitPoints _hitPoints = new HitPoints(100);
        [SerializeField]
        private Transform spawnPoint = default;

        public PayCounter payCounter;

        [field: SerializeField]
        public int CurrentClickCount { get; set; } = 0;
        [field: SerializeField]
        public int MaxClickCunt { get; set; } = 10;
        [field: SerializeField]
        public float TimeOfLastClick { get; set; } = 0;
        public ClickableMode ClickableMode { get; set; }
        public float SecondToWaitBeforeClickCountDecreases { get; set; }
        public float SecondsToDecreasesOnePoint { get; set; }
        [field: SerializeField]
        public float SecondPerOneClickDisappear { get; set; } = 5;
        public Action OnCounterCountChange { get; set; }
        public Action OnCounterMaxValue { get; set; }

        public Transform SnapPivot => spawnPoint;

        private string DebugTag => $"{nameof(Base)}";

        private void OnEnable()
        {
            _hitPoints.OnDamageTaken += OnHitTaken;
            _hitPoints.OnHitPointsDepleted += OnHitpointsDepleted;
            payCounter.OnCountingFinished += CreateWorker;
        }

        private void OnDisable()
        {
            _hitPoints.OnDamageTaken -= OnHitTaken;
            _hitPoints.OnHitPointsDepleted -= OnHitpointsDepleted;
            payCounter.OnCountingFinished -= CreateWorker;
        }

        public void OnCursorEnter(MultiMousePointerEventData eventData)
        {
            Debug.Log($"{DebugTag} {nameof(OnCursorEnter)}");
        }

        public void OnCursorExit(MultiMousePointerEventData eventData)
        {
            Debug.Log($"{DebugTag} {nameof(OnCursorExit)}");
        }

        public void OnCursorClick(MultiMousePointerEventData eventData)
        {
            ++CurrentClickCount;
            Debug.Log($"{DebugTag} {nameof(OnCursorClick)}");
        }

        public void OnCursorUp(MultiMousePointerEventData eventData)
        {
            if (payCounter.gameObject.activeSelf)
            {
                payCounter.StopCounting();
            }
        }

        public void OnCursorDown(MultiMousePointerEventData eventData)
        {
            payCounter.gameObject.SetActive(true);
            if (GameManager.Instance.Gold >= 10)
            {
                payCounter.StartCounting();
            }
        }

        private void CreateWorker()
        {
            Debug.Log("Create worker");
            GameManager.Instance.Gold -= 10;
            GameManager.Instance.CreateWorker(SnapPivot.position, Quaternion.identity);
            payCounter.gameObject.SetActive(false);
        }

        public void OnCursorDrag(MultiMousePointerEventData eventData)
        {
            
        }

        public void Hit(float damage)
        {
            _hitPoints.TakeDamage(damage);
            AudioManager.Instance.Sfxs.Impact.Play();
        }

        public Vector3 GetPosition()
        {
            return SnapPivot.position;
        }

        private void OnHitTaken(float damageTaken, float remainingHitpoints)
        {
            Debug.Log($"{DebugTag} Hit: {damageTaken}, health remaining: {remainingHitpoints}");
            OnHit?.Invoke(damageTaken, remainingHitpoints);
        }

        private void OnHitpointsDepleted()
        {
            Debug.Log($"{DebugTag} Base destroyed!");
            OnDeath?.Invoke();
        }
    }
}