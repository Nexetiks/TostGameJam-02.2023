using Core.DecisionMaking.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class DayPhases : MonoBehaviour
    {
        public static event Action OnNight;
        public static event Action OnDay;

        [SerializeField]
        float dayTimeInSeconds = 60f;
        [SerializeField]
        float nightTimeInSeconds = 90f;

        private FiniteStateMachine _fsm;
        Transition _start = new Transition("start");
        Transition _dusk = new Transition("dusk");
        Transition _dawn = new Transition("dawn");

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _fsm = new FiniteStateMachine();

            State initial = new State("initial");
            State day = new State("day");
            State night = new State("night");

            _start.SetTargetState(day);
            _dusk.SetTargetState(night);
            _dusk.AddTransitionAction(() => OnNight?.Invoke());
            _dawn.SetTargetState(day);
            _dawn.AddTransitionAction(() => OnDay?.Invoke());

            initial.AddTransition(_start);

            day.AddTransition(_dusk);
            day.AddEntryAction(StartDay);

            night.AddTransition(_dawn);
            night.AddEntryAction(StartNight);

            _fsm.SetInitialState(initial);
            _start.Trigger();
        }

        private void Update()
        {
            _fsm.ExecuteActions();
        }

        private void StartDay()
        {
            StartCoroutine(DayCoroutine());
        }

        private IEnumerator DayCoroutine()
        {
            yield return new WaitForSeconds(dayTimeInSeconds);
            _dusk.Trigger();
        }

        private void StartNight()
        {
            StartCoroutine(NightCoroutine());
        }

        private IEnumerator NightCoroutine()
        {
            EnemiesSpawner.Instance.SpawnWave();
            yield return new WaitForSeconds(nightTimeInSeconds);
            _dawn.Trigger();
        }
    }
}
