using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketInventory : Inventory
{
    public int medicineLevel;
    public int maximumMedicineLevel;
    // Show the herbal level during the gameplay.
    public Healthbar medicineLevelBar;

    public new void Start()
    {
        this.name = "MarketInventory";
        medicineLevel = 0;
        UpdateMarketLevelBar();
    }

    public void IncreaseMedicineLevel(int increase)
    {
        medicineLevel += increase;
        UpdateMarketLevelBar();
    }

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

    public bool IsMarketFull()
    {
        if (medicineLevel >= maximumMedicineLevel)
        {
            return true;
        }
        return false;
    }

    public bool IsMarketNearlyFull()
    {
        if (medicineLevel == maximumMedicineLevel - 1)
        {
            return true;
        }
        return false;
    }

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
