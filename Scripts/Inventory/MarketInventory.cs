// Represents a market object storage.
public class MarketInventory : Inventory
{

    // Shows the medicine level during the gameplay.
    public Healthbar medicineLevelBar;
    // Stores the medicine level of the market.
    public int medicineLevel;
    // The maximum medicine level of the market.
    public int maximumMedicineLevel;

    // Start is called before the first frame update.
    public new void Start()
    {
        this.name = "MarketInventory";
        medicineLevel = 0;
        UpdateMarketLevelBar();
    }

    // Increases the medicine level of the market
    // by the parameter.
    public void IncreaseMedicineLevel(int increase)
    {
        medicineLevel += increase;
        UpdateMarketLevelBar();
    }

    // Decreases the medicine level of the market
    // by the parameter.
    public void DecreaseMedicineLevel(int decrease)
    {
        if (medicineLevel - decrease < 0)
        {
            medicineLevel = 0;
        }
        else
        {
            medicineLevel -= decrease;
        }
        UpdateMarketLevelBar();
    }

    // Returns whether the market is full.
    public bool IsMarketFull()
    {
        if (medicineLevel >= maximumMedicineLevel)
        {
            return true;
        }
        return false;
    }

    // Returns whether the market is nearly full.
    public bool IsMarketNearlyFull()
    {
        if (medicineLevel == maximumMedicineLevel - 1)
        {
            return true;
        }
        return false;
    }

    // Returns whether the market has medicine
    // available.
    public bool hasMedicineAvailable()
    {
        if (medicineLevel > 0)
        {
            return true;
        }
        return false;
    }


    // Updates the level bar that shows the medicine
    // level in the game.
    private void UpdateMarketLevelBar()
    {
        if (medicineLevel > maximumMedicineLevel)
        {
            medicineLevelBar.UpdateHealth(1.0f);

        }
        else if (0 > medicineLevel)
        {
            medicineLevelBar.UpdateHealth(0.0f);
        }
        else
        {
            medicineLevelBar.UpdateHealth((float)medicineLevel / maximumMedicineLevel);
        }
    }

}
