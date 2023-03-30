#Yet another Finite State Machine implementation.

## Interfaces

Use `IState` to define list of actions and transitions for state. 

- Entry actions are called when we enter the state
- Update actions are called every `ExecuteActions` when we are in the state
- Exit actions are called when we exit the state
- List of triggers is checked every `ExecuteActions`, the first with `IsTriggered` returning true is taken and executed


Use `ITransition` to define transitions between states.

- Set Target state so we FSM can move to next state
- Add actions that will be called when transition is triggered
- Define if transition is triggered


## Implementations

`State` is basic implementation for FSM `IState`, should be enough for us.

`Transition` is basic implementation for FSM `ITransition`, should be enough for us.

When you are done with setting up states and transitions set the initial state on `FiniteStateMachine` and you are ready to go. Call `ExecuteActions` every `Update` to execute current actions. Call `Transition.Trigger` to trigger the state change.