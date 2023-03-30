using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.DecisionMaking.StateMachine
{
    public class State : IState
    {
        public string Name;

        protected List<Action> entryActions = new();
        protected List<Action> updateActions = new();
        protected List<Action> exitActions = new();
        protected List<ITransition> transitions = new();

        public State(string name)
        {
            Name = name;
            //AddEntryAction(() => Debug.Log($"Entering {Name} state."));
            //AddExitAction(() => Debug.Log($"Exiting {Name} state."));
        }

        public void AddEntryAction(Action action)
        {
            entryActions.Add(action);
        }

        public void AddUpdateAction(Action action)
        {
            updateActions.Add(action);
        }

        public void AddExitAction(Action action)
        {
            exitActions.Add(action);
        }

        public void AddTransition(ITransition transition)
        {
            transitions.Add(transition);
        }

        public List<Action> GetEntryAction()
        {
            return entryActions;
        }

        public List<Action> GetAction()
        {
            return updateActions;
        }

        public List<Action> GetExitAction()
        {
            return exitActions;
        }

        public List<ITransition> GetTransitions()
        {
            return transitions;
        }
    }
}