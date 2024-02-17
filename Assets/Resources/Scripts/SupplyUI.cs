using TMPro;
using UnityEngine;

public class SupplyUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI supplyText;

    public void UpdateSupplyUI(int supplyDemands)
    {
        supplyText.text = "remaining to be delivered" + supplyDemands.ToString();
    }
}
