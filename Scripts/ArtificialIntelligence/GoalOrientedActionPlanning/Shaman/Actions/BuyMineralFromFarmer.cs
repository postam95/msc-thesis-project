using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the Buy Mineral From Farmer action of the Shaman GOAP Agent.
public class BuyMineralFromFarmer : GoapAction
{

    // Runs before the action ONCE.
    public override void BeforeAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.animator.SetBool("isWalking", true);
        PrepareMovementForThisAction();
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in BuyMineralFromFarmer: BeforeAction");
    }

    // Runs in the action ONCE.
    public override void ExecuteAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in BuyMineralFromFarmer: ExecuteAction");
    }

    // Runs after the action ONCE.
    public override void AfterAction()
    {
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.animator.SetBool("isWalking", false);
        shamanGoapAgent.LoadUpMineralBucket();
        Debug.Log("GOAP Id = " + shamanGoapAgent.characterId + " is in BuyMineralFromFarmer: AfterAction");
    }

    // Preparing agent movements for buy mineral from farmer.
    protected override void PrepareMovementForThisAction()
    {
        // Shaman can go faster, because the pocket is empty.
        ShamanGoapAgent shamanGoapAgent = (ShamanGoapAgent)this.owner;
        shamanGoapAgent.navMeshAgent.speed = 16;
        shamanGoapAgent.navMeshAgent.angularSpeed = 180;
        shamanGoapAgent.navMeshAgent.acceleration = 6;
    }

}
