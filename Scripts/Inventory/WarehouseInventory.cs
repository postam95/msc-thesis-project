using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseInventory : Inventory
{
    public int mineralLevel;
    public int maximumMineralLevel;
    // Show the herbal level during the gameplay.
    public Healthbar mineralLevelBar;

    public new void Start()
    {
        this.name = "MineralInventory";
        mineralLevel = 0;
        UpdateMineralLevelBar();
    }

    public void IncreaseMineralLevel(int increase)
    {
        mineralLevel += increase;
        UpdateMineralLevelBar();
    }

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

    public bool IsWarehouseFull()
    {
        if (mineralLevel >= maximumMineralLevel)
        {
            return true;
        }
        return false;
    }


    public bool IsWarehouseEmpty()
    {
        if (mineralLevel == 0)
        {
            return true;
        }
        return false;
    }

    public bool IsWarehouseNearlyFull()
    {
        if (mineralLevel == maximumMineralLevel - 1)
        {
            return true;
        }
        return false;
    }

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
