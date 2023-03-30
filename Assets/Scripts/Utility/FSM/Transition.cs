using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DecisionMaking.StateMachine
{
    public class Transition : ITransition
    {
        public string Name;
        protected IState targetState;
        protected List<Action> transitionActions = new();
        protected bool isTriggered = false;

        public Transition(string name)
        {
            Name = name;
            //AddTransitionAction(() => Debug.Log($"Transition {Name} is being executed."));
            // Reset the triggered state on transition
            AddTransitionAction(() => isTriggered = false);
        }

        public void SetTargetState(IState state)
        {
            targetState = state;
        }

        public void AddTransitionAction(Action action)
        {
            transitionActions.Add(action);
        }

        public void Trigger()
        {
            isTriggered = true;
        }

        public IState GetTargetState()
        {
            return targetState;
        }

        public List<Action> GetAction()
        {
            return transitionActions;
        }

        public bool IsTriggered()
        {
            return isTriggered;
        }
    }
}