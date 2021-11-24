// Represents a warehouse object storage.
public class WarehouseInventory : Inventory
{

    // Shows the mineral level during the gameplay.
    public Healthbar mineralLevelBar;
    // Stores the mineral level of the warehouse.
    public int mineralLevel;
    // The maximum mineral level of the warehouse.
    public int maximumMineralLevel;

    // Start is called before the first frame update.
    public new void Start()
    {
        this.name = "WarehouseInventory";
        mineralLevel = 0;
        UpdateMineralLevelBar();
    }

    // Increases the mineral level of the warehouse
    // by the parameter.
    public void IncreaseMineralLevel(int increase)
    {
        mineralLevel += increase;
        UpdateMineralLevelBar();
    }

    // Decreases the mineral level of the warehouse
    // by the parameter.
    public void DecreaseMineralLevel(int decrease)
    {
        if (mineralLevel - decrease < 0)
        {
            mineralLevel = 0;
        }
        else
        {
            mineralLevel -= decrease;
        }
        UpdateMineralLevelBar();
    }

    // Returns whether the warehouse is full.
    public bool IsWarehouseFull()
    {
        if (mineralLevel >= maximumMineralLevel)
        {
            return true;
        }
        return false;
    }

    // Returns whether the warehouse is empty.
    public bool IsWarehouseEmpty()
    {
        if (mineralLevel == 0)
        {
            return true;
        }
        return false;
    }

    // Returns whether the warehouse is nearly full.
    public bool IsWarehouseNearlyFull()
    {
        if (mineralLevel == maximumMineralLevel - 1)
        {
            return true;
        }
        return false;
    }

    // Updates the level bar that shows the mineral
    // level in the game.
    private void UpdateMineralLevelBar()
    {
        if (mineralLevel > maximumMineralLevel)
        {
            mineralLevelBar.UpdateHealth(1.0f);

        }
        else if (0 > mineralLevel)
        {
            mineralLevelBar.UpdateHealth(0.0f);
        }
        else
        {
            mineralLevelBar.UpdateHealth((float) mineralLevel / maximumMineralLevel);
        }
    }

}
