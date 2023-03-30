using System;
using System.Collections.Generic;

namespace Core.DecisionMaking.StateMachine
{
    public interface IState
    {
        public List<Action> GetEntryAction();
        public List<Action> GetAction();
        public List<Action> GetExitAction();
        public List<ITransition> GetTransitions();
    }
}