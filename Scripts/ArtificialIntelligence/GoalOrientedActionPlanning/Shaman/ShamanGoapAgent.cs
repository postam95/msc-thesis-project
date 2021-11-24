using System.Collections.Generic;
using UnityEngine;

// Represents the Shaman GOAP Agent.
public class ShamanGoapAgent : GoapAgent
{

    // The Shaman takes the minerals from the warehouse.
    // The Inventory stores information about the state
    // of the warehouse.
    public Inventory warehouseInventory;
    // The Shaman takes the medicine to the market.
    // The Inventory stores information about the state
    // of the warehouse.
    public Inventory marketInventory;
    // The Shaman goes to the warehouse for mineral.
    public GameObject warehouseObject;
    // The Shaman goes to the garden for magic plant.
    public GameObject gardenObject;
    // The Shaman goes to the medical centre to make medicine.
    public GameObject medicalCentreObject;
    // The Shaman goes to the farm for mineral.
    public GameObject farmObject;
    // The Shaman goes to the market to deliver medicine.
    public GameObject marketObject;
    // Animator component of this character to manipulate
    // its movement.
    public Animator animator;
    // Showing the magic plant level during the game.
    public Healthbar magicPlantLevelBar;
    // Showing the mineral level during the game.
    public Healthbar mineralLevelBar;
    // Showing the medicine level during the game.
    public Healthbar medicineLevelBar;
    // Defines the maximum number of magic plant.
    private float magicPlantBucketSize;
    // Stores the current magic plant level.
    public float magicPlantLevel;
    // Defines the maximum number of medicine.
    private float medicineBucketSize;
    // Stores the current medicine level.
    public float medicineLevel;
    // Defines the maximum number of mineral.
    private float mineralBucketSize;
    // Stores the current mineral level.
    public float mineralLevel;

    // Start is called before the first frame update.
    void Start()
    {
        // Initialization.
        base.Start();
        animator = this.GetComponent<Animator>();
        InitializeActions();
        mineralBucketSize = 100;
        medicineBucketSize = 100;
        magicPlantBucketSize = 100;
        EmptyMineralBucket();
        EmptyMedicineBucket();
        EmptyMagicPlantBucket();
        UpdateHealthBar(magicPlantLevelBar, magicPlantBucketSize, magicPlantLevel);
        UpdateHealthBar(mineralLevelBar, mineralBucketSize, mineralLevel);
        UpdateHealthBar(medicineLevelBar, medicineBucketSize, medicineLevel);
    }

    // Update is called once per frame
    void Update()
    {
        MarketInventory inventory = (MarketInventory)this.marketInventory;
        // If the market is NOT full and the Shaman has no goal
        // it adds a new goal to make a plan for.
        if (!inventory.IsMarketFull() && currentGoal == null)
        {
            GoapGoal goal = new GoapGoal(Conditions.medicineDelivered, true);
            currentGoal = goal;
            Debug.Log("GOAP Id = " + this.characterId + " has added a new goal.");
        }
    }

    // Sets zero the magic plant level.
    public void EmptyMagicPlantBucket()
    {
        magicPlantLevel = 0.0f;
        UpdateHealthBar(magicPlantLevelBar, magicPlantBucketSize, magicPlantLevel);
    }

    // Sets maximum value for the magic plant level.
    public void LoadUpMagicPlantBucket()
    {
        magicPlantLevel = magicPlantBucketSize;
        UpdateHealthBar(magicPlantLevelBar, magicPlantBucketSize, magicPlantLevel);
    }

    // Sets zero the medicine level.
    public void EmptyMedicineBucket()
    {
        medicineLevel = 0.0f;
        UpdateHealthBar(medicineLevelBar, medicineBucketSize, medicineLevel);
    }

    // Sets maximum value for the medicine level.
    public void LoadUpMedicineBucket()
    {
        medicineLevel = medicineBucketSize;
        UpdateHealthBar(medicineLevelBar, medicineBucketSize, medicineLevel);
    }

    // Sets zero the mineral level.
    public void EmptyMineralBucket()
    {
        mineralLevel = 0.0f;
        UpdateHealthBar(mineralLevelBar, mineralBucketSize, mineralLevel);
    }

    // Sets maximum value for the mineral level.
    public void LoadUpMineralBucket()
    {
        mineralLevel = mineralBucketSize;
        UpdateHealthBar(mineralLevelBar, mineralBucketSize, mineralLevel);
    }

    // Updates the Shaman's healthbar.
    void UpdateHealthBar(Healthbar healthLevelbar, float bucketSize, float currentLevel)
    {
        if (currentLevel > bucketSize)
        {
            healthLevelbar.UpdateHealth(1.0f);

        }
        else if (0.0f > magicPlantLevel)
        {
            healthLevelbar.UpdateHealth(0.0f);
        }
        else
        {
            healthLevelbar.UpdateHealth(currentLevel / bucketSize);
        }
    }

    // Initializes the GOAP Actions for the Shaman.
    // These actions will be processed during the
    // planning.
    public override void InitializeActions()
    {
        actions.Add(InitializeBuyHerbalFromFarmer());
        actions.Add(InitializeCollectHerbal());
        actions.Add(InitializeCollectMineral());
        actions.Add(InitializeDeliverMedicine());
        actions.Add(InitializeMakeMedicine());
    }

    // Initializes the Buy Herbal From Farmer GOAP Action.
    private GoapAction InitializeBuyHerbalFromFarmer()
    {
        BuyMineralFromFarmer buyHerbalFromFarmer = new BuyMineralFromFarmer();
        buyHerbalFromFarmer.nameOfTheAction = "BuyHerbalFromFarmer";
        buyHerbalFromFarmer.afterEffects.Add(Conditions.hasMineral, true);
        buyHerbalFromFarmer.position = farmObject;
        buyHerbalFromFarmer.cost = 2.0f;
        buyHerbalFromFarmer.owner = this;

        return buyHerbalFromFarmer;
    }

    // Initializes the Collect Herbal GOAP Action.
    private GoapAction InitializeCollectHerbal()
    {
        CollectMagicPlant collectHerbal = new CollectMagicPlant();
        collectHerbal.nameOfTheAction = "CollectHerbal";
        collectHerbal.afterEffects.Add(Conditions.hasMagicPlant, true);
        collectHerbal.position = gardenObject;
        collectHerbal.owner = this;

        return collectHerbal;
    }

    // Initializes the Collect Mineral GOAP Action.
    private GoapAction InitializeCollectMineral()
    {
        CollectMineral collectMineral = new CollectMineral();
        collectMineral.nameOfTheAction = "CollectMineral";
        collectMineral.preConditions.Add(Conditions.enoughMineralInWarehouse, true);
        collectMineral.afterEffects.Add(Conditions.hasMineral, true);
        collectMineral.position = warehouseObject;
        collectMineral.owner = this;

        return collectMineral;
    }

    // Initializes the Deliver Medicine GOAP Action.
    private GoapAction InitializeDeliverMedicine()
    {
        DeliverMedicine deliverMedicine = new DeliverMedicine();
        deliverMedicine.nameOfTheAction = "DeliverMedicine";
        deliverMedicine.preConditions.Add(Conditions.hasMedicine, true);
        deliverMedicine.afterEffects.Add(Conditions.medicineDelivered, true);
        deliverMedicine.position = marketObject;
        deliverMedicine.owner = this;

        return deliverMedicine;
    }

    // Initializes the Make Medicine GOAP Action.
    private GoapAction InitializeMakeMedicine()
    {
        MakeMedicine makeMedicine = new MakeMedicine();
        makeMedicine.nameOfTheAction = "MakeMedicine";
        makeMedicine.preConditions.Add(Conditions.hasMagicPlant, true);
        makeMedicine.preConditions.Add(Conditions.hasMineral, true);
        makeMedicine.afterEffects.Add(Conditions.hasMedicine, true);
        makeMedicine.position = medicalCentreObject;
        makeMedicine.owner = this;

        return makeMedicine;
    }

    // Gives the medicine to the market.
    public void PutDownMedicine()
    {
        MarketInventory inventory = (MarketInventory)this.marketInventory;
        inventory.IncreaseMedicineLevel(1);
    }

    // Takes the mineral from the warehouse.
    public void PickUpMineralFromWarehouse()
    {
        WarehouseInventory inventory = (WarehouseInventory)this.warehouseInventory;
        inventory.DecreaseMineralLevel(1);
    }

    // The GOAP Planner needs states that relevant to
    // the GOAP Agent to start the planning process.
    // This method will assembly and return the set
    // of these states.
    public override Dictionary<Conditions, bool> GenerateAgentRelevantStates()
    {
        Dictionary<Conditions, bool> states = new Dictionary<Conditions, bool>();
        Debug.Log("alma");
        if (medicineLevel > 0.0f)
        {
            states.Add(Conditions.hasMedicine, true);
        }
        if (mineralLevel > 0.0f)
        {
            states.Add(Conditions.hasMineral, true);
        }
        if (magicPlantLevel > 0.0f)
        {
            states.Add(Conditions.hasMagicPlant, true);
        }
        WarehouseInventory inventory = (WarehouseInventory)this.warehouseInventory;
        if (!inventory.IsWarehouseEmpty())
        {
            states.Add(Conditions.enoughMineralInWarehouse, true);
        }

        return states;
    }

}
