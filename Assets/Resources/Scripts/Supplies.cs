using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supplies : MonoBehaviour
{
    [SerializeField]
    int suppllyDemands;
    [SerializeField]
    SupplyUI supplyUI;

    public void StartDay()
    {
        //update UI
        supplyUI.UpdateSupplyUI(suppllyDemands);
    }

    public void DeliverSupply(int delivered)
    {
        suppllyDemands -= delivered;
        supplyUI.UpdateSupplyUI(suppllyDemands);
    }
}
