using UnityEngine;

// Represents the Deliver Medicine action of the Shaman GOAP Agent.
public class DeliverMedicine : GoapAction
{

    // Runs before the action ONCE.
    public override void BeforeAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        PrepareMovementForThisAction();
        shamanGoapAgent.animator.SetBool("isWalking", true);
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in DeliverMedicine: BeforeAction");
    }

    // Runs in the action ONCE.
    public override void ExecuteAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in DeliverMedicine: ExecuteAction");
    }

    // Runs after the action ONCE.
    public override void AfterAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.animator.SetBool("isWalking", false);
        shamanGoapAgent.PutDownMedicine();
        shamanGoapAgent.EmptyMedicineBucket();
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in DeliverMedicine: AfterAction");
    }

    // Preparing agent movements for deliver medicine.
    protected override void PrepareMovementForThisAction()
    {
        // Shaman can go faster, because he is can't wait
        // to deliver medicine.
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.navMeshAgent.speed = 15;
        shamanGoapAgent.navMeshAgent.angularSpeed = 160;
        shamanGoapAgent.navMeshAgent.acceleration = 5;
    }

}
