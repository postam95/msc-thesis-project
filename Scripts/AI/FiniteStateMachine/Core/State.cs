// The base of every state.
public abstract class State
{

    // Initiates a new State.
    public State() {}

    // Called ONCE when the FSM agent enters to a state.
    public abstract void BeforeState(BaseCharacter character);
    // Called SEVERAL TIMES when the FSM agent is in a state.

    public abstract void InState(BaseCharacter character);

    // Called ONCE when the FSM agent leaves to a state.
    public abstract void AfterState(BaseCharacter character);

}
