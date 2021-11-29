using UnityEngine;

// In this state the Farmer FSM agent collects herbal.
public class CollectingState : State
{

    // Provides only one instance of this class, thread-safely.
    private static CollectingState instance = null;
    private static readonly object padlock = new object();

    // Initiates the this state.
    CollectingState()
    {
    }

    // Handles the singleton object.
    public static CollectingState Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CollectingState();
                }
                return instance;
            }
        }
    }

    // Runs when the agent enters into this state.
    public override void BeforeState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;
        farmer.animator.SetBool("isCollecting", true);

        farmer.StartCollectingHerbal();

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is entering collecting state.");
    }

    // Runs when the agent is in this state.
    public override void InState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;

        if (farmer.IsHerbalBucketFull())
        {
            farmer.StopCollectingHerbal();
            farmer.ChangeState(TakeHerbalsHomeState.Instance);
        }

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is in collecting state.");
    }

    // Runs when the agent leaves into this state.
    public override void AfterState(BaseCharacter character)
    {
        Farmer farmer = (Farmer)character;
        farmer.animator.SetBool("isCollecting", false);

        Debug.Log("FSM Id = " + farmer.CharacterId + " farmer is leaving collecting state.");
    }

}
