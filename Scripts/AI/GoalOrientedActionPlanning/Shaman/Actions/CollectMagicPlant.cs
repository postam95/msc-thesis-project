using UnityEngine;

// Represents the Collect Magic Plant action of the Shaman GOAP Agent.
public class CollectMagicPlant : GoapAction
{
    // Runs before the action ONCE.
    public override void BeforeAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        PrepareMovementForThisAction();
        shamanGoapAgent.animator.SetBool("isWalking", true);
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in CollectMagicPlant: BeforeAction");
    }

    // Runs in the action ONCE.
    public override void ExecuteAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in CollectMagicPlant: ExecuteAction");
    }

    // Runs after the action ONCE.
    public override void AfterAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.animator.SetBool("isWalking", false);
        shamanGoapAgent.LoadUpMagicPlantBucket();
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in CollectMagicPlant: AfterAction");
    }


    // Preparing agent movements for collect magic plant.
    protected override void PrepareMovementForThisAction()
    {
        // Shaman can go faster, because the pocket is empty.
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.navMeshAgent.speed = 16;
        shamanGoapAgent.navMeshAgent.angularSpeed = 180;
        shamanGoapAgent.navMeshAgent.acceleration = 6;
    }

}
