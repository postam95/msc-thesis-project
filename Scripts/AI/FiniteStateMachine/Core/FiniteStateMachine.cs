// Represents a Finite State Machine for the FSM-based
// agents.
public class FiniteStateMachine
{

    // This character owns the FSM instance.
    private BaseCharacter owner;
    // Stores the current state.
    private State _currentState;
    public State CurrentState { get => _currentState; set => _currentState = value; }
    // Stores the previous state.
    private State _previousState;
    public State PreviousState { get => _previousState; set => _previousState = value; }

    // Initiates the Finite State Machine object by
    // setting the properties.
    public FiniteStateMachine(BaseCharacter owner)
    {
        this.owner = owner;
        _currentState = null;
        _previousState = null;
    }

    // The core method of this class. It keeps moving
    // the FSM agent while updating the InState method.
    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.InState(owner);
        }
    }

    // Changes the state.
    public void ChangeState(State newState)
    {
        // Saves the current state to previous state.
        _previousState = _currentState;
        // First, closes the current state.
        _currentState.AfterState(owner);
        // Changes the current state.
        _currentState = newState;
        // Finally, runs the entering actions of the state. 
        _currentState.BeforeState(owner);
    }

    // It is really frequent that the FSM wants to enter into
    // the previous state. This method provides it.
    public void ReturnToPreviousState()
    {
        // Changing the current and previous states.
        State tempState = _currentState;
        _currentState = _previousState;
        _previousState = tempState;
        // Runs the closing method of the previous state.
        _previousState.AfterState(owner);
        // Finally, runs the entering actions of the state. 
        _currentState.BeforeState(owner);
    }

}
