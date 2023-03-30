using System;
using System.Collections.Generic;

namespace Core.DecisionMaking.StateMachine
{
    public interface ITransition
    {
        public IState GetTargetState();
        public List<Action> GetAction();
        public bool IsTriggered();
    }
}