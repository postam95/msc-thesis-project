using UnityEngine;

// Represents the Make Medicine action of the Shaman GOAP Agent.
public class MakeMedicine : GoapAction
{

    // Runs before the action ONCE.
    public override void BeforeAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        PrepareMovementForThisAction();
        shamanGoapAgent.animator.SetBool("isWalking", true);
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in MakeMedicine: BeforeAction");
    }

    // Runs in the action ONCE.
    public override void ExecuteAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.EmptyMagicPlantBucket();
        shamanGoapAgent.EmptyMineralBucket();
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in GoToWarehouse: ExecuteAction");
    }

    // Runs after the action ONCE.
    public override void AfterAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.LoadUpMedicineBucket();
        shamanGoapAgent.animator.SetBool("isWalking", false);
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in MakeMedicine: AfterAction");
    }

    // Preparing agent movements for making medicine.
    protected override void PrepareMovementForThisAction()
    {
        // Shaman has to slow down, because the mineral and
        // the magic plant are heavy.
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.navMeshAgent.speed = 14;
        shamanGoapAgent.navMeshAgent.angularSpeed = 140;
        shamanGoapAgent.navMeshAgent.acceleration = 4;
    }

}
