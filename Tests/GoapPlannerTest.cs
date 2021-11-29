using System.Collections.Generic;
using Moq;
using NUnit.Framework;

// Test class for GoapPlanner.
public class GoapPlannerTest
{

    // Test for GoapPlanner initialization.
    [Test]
    public void ItShouldInitializeTheGoapPlanner()
    {
        // Initialization.
        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction = new Mock<GoapAction>().Object;
        actions.Add(goapAction);

        // Test.
        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Asserts.
        Assert.IsNotNull(goapPlanner);

        int expectedGoapActionsCount = 1;
        int goapActionsCount = goapPlanner.actions.Count;
        Assert.AreEqual(expectedGoapActionsCount, goapActionsCount);

        GoapAction expectedGoapActionInList = actions[0];
        GoapAction goapActioninList = goapPlanner.actions[0];
        Assert.AreEqual(expectedGoapActionInList, goapActioninList);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalStateIsEqualWithTheCurrentState()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();
        state.Add(Conditions.enoughMineralInWarehouse, true);

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction = new Mock<GoapAction>().Object;
        goapAction.afterEffects.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction.cost = 1.0f;
        actions.Add(goapAction);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.enoughMineralInWarehouse, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Test.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Assert.
        Assert.IsNotNull(plan);
        int expectedSizeOfTheResultQueue = 0;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsNotAchievable()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();
        state.Add(Conditions.enoughMineralInWarehouse, true);

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction = new Mock<GoapAction>().Object;
        goapAction.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction.cost = 1.0f;
        actions.Add(goapAction);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMineral, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNull(plan);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsAchievable()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();
        state.Add(Conditions.enoughMineralInWarehouse, true);

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction = new Mock<GoapAction>().Object;
        goapAction.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction.cost = 1.0f;
        actions.Add(goapAction);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMagicPlant, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNotNull(plan);

        int expectedSizeOfTheResultQueue = 1;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsAchievableAndThereAreMoreThanOneSolutions()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();
        state.Add(Conditions.enoughMineralInWarehouse, true);

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction1 = new Mock<GoapAction>().Object;
        goapAction1.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction1.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction1.cost = 2.0f;
        actions.Add(goapAction1);

        GoapAction goapAction2 = new Mock<GoapAction>().Object;
        goapAction2.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction2.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction2.cost = 1.0f;
        actions.Add(goapAction2);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMagicPlant, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNotNull(plan);

        int expectedSizeOfTheResultQueue = 1;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);

        float expectedCostOfTheAction = 1.0f;
        float costOfTheAction = plan.Count;
        Assert.AreEqual(expectedCostOfTheAction, costOfTheAction);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsAchievableAndThereAreMultipleActionsInTheSolution()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();
        state.Add(Conditions.enoughMineralInWarehouse, true);

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction1 = new Mock<GoapAction>().Object;
        goapAction1.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        goapAction1.afterEffects.Add(Conditions.hasMedicine, true);
        goapAction1.cost = 2.0f;
        actions.Add(goapAction1);

        GoapAction goapAction2 = new Mock<GoapAction>().Object;
        goapAction2.preConditions.Add(Conditions.hasMedicine, true);
        goapAction2.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction2.cost = 1.0f;
        actions.Add(goapAction2);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMagicPlant, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNotNull(plan);

        int expectedSizeOfTheResultQueue = 2;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsAchievableAndThereAreNoSubStatesInTheBaseState()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction1 = new Mock<GoapAction>().Object;
        goapAction1.afterEffects.Add(Conditions.hasMedicine, true);
        goapAction1.cost = 2.0f;
        actions.Add(goapAction1);

        GoapAction goapAction2 = new Mock<GoapAction>().Object;
        goapAction2.preConditions.Add(Conditions.hasMedicine, true);
        goapAction2.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction2.cost = 1.0f;
        actions.Add(goapAction2);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMagicPlant, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNotNull(plan);

        int expectedSizeOfTheResultQueue = 2;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);
    }

    //Test for GoapPlanner Plan method.
    [Test]
    public void ItShouldTestPlannerWhenTheGoalIsAchievableAndThereAreMultiplePreconditions()
    {
        // Initialization.
        Dictionary<Conditions, bool> state = new Dictionary<Conditions, bool>();

        List<GoapAction> actions = new List<GoapAction>();

        GoapAction goapAction1 = new Mock<GoapAction>().Object;
        goapAction1.afterEffects.Add(Conditions.hasMineral, true);
        goapAction1.cost = 2.0f;
        actions.Add(goapAction1);

        GoapAction goapAction2 = new Mock<GoapAction>().Object;
        goapAction2.afterEffects.Add(Conditions.hasMagicPlant, true);
        goapAction2.cost = 2.0f;
        actions.Add(goapAction2);

        GoapAction goapAction3 = new Mock<GoapAction>().Object;
        goapAction3.preConditions.Add(Conditions.hasMineral, true);
        goapAction3.preConditions.Add(Conditions.hasMagicPlant, true);
        goapAction3.afterEffects.Add(Conditions.hasMedicine, true);
        goapAction3.cost = 1.0f;
        actions.Add(goapAction3);

        GoapGoal goapGoal = new Mock<GoapGoal>(Conditions.hasMedicine, true).Object;

        GoapPlanner goapPlanner = new GoapPlanner(actions);

        // Tested function.
        Queue<GoapAction> plan = goapPlanner.Plan(goapGoal, state);

        // Asserts.
        Assert.IsNotNull(plan);

        int expectedSizeOfTheResultQueue = 3;
        int sizeOfResultQueue = plan.Count;
        Assert.AreEqual(expectedSizeOfTheResultQueue, sizeOfResultQueue);
    }

}
