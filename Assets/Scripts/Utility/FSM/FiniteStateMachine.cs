using System;
using System.Collections.Generic;

namespace Core.DecisionMaking.StateMachine
{
    public class FiniteStateMachine
    {
        private IState _initialState;
        private IState _currentState;

        public void SetInitialState(IState initialState)
        {
            _initialState = initialState;
        }

        public void ExecuteActions()
        {
            if (_currentState == null)
                _currentState = _initialState;

            var actions = Update();

            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }

        private List<Action> Update()
        {
            if (_currentState == null)
                return null;

            ITransition triggeredTransition = null;

            foreach (var transition in _currentState.GetTransitions())
            {
                if (transition.IsTriggered())
                {
                    triggeredTransition = transition;
                    break;
                }
            }

            if (triggeredTransition == null)
                return _currentState.GetAction();

            IState targetState = triggeredTransition.GetTargetState();

            List<Action> actions = new List<Action>();
            actions.AddRange(_currentState.GetExitAction());
            actions.AddRange(triggeredTransition.GetAction());
            actions.AddRange(targetState.GetEntryAction());

            _currentState = targetState;

            return actions;
        }
    }
}