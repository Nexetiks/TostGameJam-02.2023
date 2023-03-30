using Core.DecisionMaking.StateMachine;
using General;
using Interactions;
using NoGround.Character;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static NoGround.Character.HitPoints;

namespace Gameplay
{
    public class EnemyBehaviour : Clickable, ITarget
    {
        public event Action<float, float> OnHit;
        public event Action OnDeath;

        [SerializeField]
        private HitPoints _hitPoints = new HitPoints(10);
        [SerializeField]
        private float _speed = 2f;
        [SerializeField]
        private float _attackCooldownTime = 1f;
        [SerializeField]
        private Projectile _projectilePrefab;
        [SerializeField]
        private float _damage = 10;

        [SerializeField]
        private Rigidbody _rigidbody;

        private float _cooldown = 0f;

        private FiniteStateMachine _fsm;
        private ITarget _target;
        private Vector3 _movePosition;

        private float _positionStopOffset = 1f;
        private float _moveToPositionOffset = 1.5f;

        Transition _toMoving = new Transition("toMoving");
        Transition _toCombat = new Transition("toCombat");
        Transition _fromMoveToFinal = new Transition("diedAtMove");
        Transition _fromCombatToFinal = new Transition("diedAtCombat");
        Transition _backToMovement = new Transition("backToMovement");

        private void Awake()
        {
            SnapPivot = transform;
            Initialize();
        }

        private void OnEnable()
        {
            _hitPoints.OnDamageTaken += OnHitTaken;
        }

        private void OnDisable()
        {
            _hitPoints.OnDamageTaken -= OnHitTaken;
        }

        protected override void Update()
        {
            base.Update();
            _fsm.ExecuteActions();
        }

        public void Initialize()
        {
            _hitPoints.Reset();

            _fsm = new FiniteStateMachine();

            State initialState = new State("initial");
            State movingState = new State("moving");
            State combatState = new State("combat");
            State finalState = new State("final");

            _toMoving.SetTargetState(movingState);
            _toCombat.SetTargetState(combatState);
            _fromCombatToFinal.SetTargetState(finalState);
            _backToMovement.SetTargetState(movingState);

            initialState.AddTransition(_toMoving);

            movingState.AddTransition(_fromMoveToFinal);
            movingState.AddTransition(_toCombat);

            movingState.AddEntryAction(FindPositionToMove);
            movingState.AddUpdateAction(UpdateHitpoints2);
            movingState.AddUpdateAction(UpdateMove);

            combatState.AddTransition(_fromCombatToFinal);
            combatState.AddTransition(_backToMovement);

            combatState.AddEntryAction(FindTarget);
            combatState.AddUpdateAction(UpdateHitpoints);
            combatState.AddUpdateAction(UpdateAttack);
            combatState.AddUpdateAction(CheckDistanceFromThePosition);

            finalState.AddEntryAction(Kill);

            _fsm.SetInitialState(initialState);
            _toMoving.Trigger();
        }

        private void UpdateHitpoints2()
        {
            if (_hitPoints.Value <= 0)
            {
                _fromMoveToFinal.Trigger();
            }
        }

        private void UpdateHitpoints()
        {
            if (_hitPoints.Value <= 0)
            {
                _fromCombatToFinal.Trigger();
            }
        }

        private void FindPositionToMove()
        {
            float mapCenterOffset = 10f;
            var target = GameObject.FindWithTag("LandLevel").transform;

            transform.LookAt(target.position);
            Vector3 direction = (transform.position - target.position).normalized;
            _movePosition = direction * mapCenterOffset;
            _toMoving.Trigger();
        }

        private void UpdateMove()
        {
            transform.LookAt(_movePosition);
            _rigidbody.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
            if (Mathf.Abs((transform.position - _movePosition).magnitude) <= _positionStopOffset)
            {
                _toCombat.Trigger();
            }
        }

        private void FindTarget()
        {
            var target = GameObject.FindGameObjectWithTag("Base");
            if (target == null) return;
            _target = target.GetComponent<ITarget>();
        }

        private void UpdateAttack()
        {
            if (_target == null) return;

            if (_cooldown <= 0)
            {
                AttackTarget();
            }
            else
            {
                UpdateAttackCooldown();
            }
        }

        private void CheckDistanceFromThePosition()
        {
            if (Vector3.Distance(transform.position, _movePosition) > _moveToPositionOffset)
                _backToMovement.Trigger();
        }

        private void AttackTarget()
        {
            // TODO: Play attack animation
            Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.Fire(transform.position, _target, _damage);
            _cooldown = _attackCooldownTime;
            AudioManager.Instance.Sfxs.Shoot.Play();
        }

        private void UpdateAttackCooldown()
        {
            _cooldown -= Time.deltaTime;
        }

        private void Kill()
        {
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }

        public void Hit(float damage)
        {
            _hitPoints.TakeDamage(damage);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void OnHitTaken(float damageTaken, float remainingHitpoints)
        {
            Debug.Log($"Hit: {damageTaken}, health remaining: {remainingHitpoints}");
            OnHit?.Invoke(damageTaken, remainingHitpoints);
        }

        public override void OnCursorEnter(MultiMousePointerEventData eventData) { }

        public override void OnCursorExit(MultiMousePointerEventData eventData) { }

        public override void OnCursorClick(MultiMousePointerEventData eventData) 
        {
            var cursor = GameManager.Instance.CursorsManager.MouseToCursorDictionary[eventData.MouseDevice];
            Hit(cursor.Damage);
        }

        public override void OnCursorUp(MultiMousePointerEventData eventData) { }

        public override void OnCursorDown(MultiMousePointerEventData eventData) { }

        public override void OnCursorDrag(MultiMousePointerEventData eventData) { }
    }
}
