using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[Serializable]
public class District : MonoBehaviour
{
    [SerializeField]
    Image image;
    [SerializeField]
    DistrictState state;
    [SerializeField]
    float supplyReqBase = 10;
    float supplyReq;
    [SerializeField]
    District[] adjacentDistricts = new District[0];

    //supplyReq getter

    //getting adjacent districts
    private void GetNeighbour()
    {

    }

    /// <summary>
    /// set new neighbour
    /// </summary>
    public void SetNeighbour(ref District newNeighbour)
    {
        Array.Resize(ref adjacentDistricts, adjacentDistricts.Length + 1);
        adjacentDistricts[adjacentDistricts.Length - 1] = newNeighbour;
    }

    private void CalcSupply()
    {
        switch(state)
        {
            case DistrictState.peace:
                {
                    supplyReq = supplyReqBase;
                    break;
                }
            case DistrictState.fight:
                {
                    supplyReq = supplyReqBase * 3;
                    break;
                }
            case DistrictState.captured:
                {
                    supplyReq = 0;
                    break;
                }
        }
    }

    public void Create(Transform districtObj)
    {
        image = districtObj.GetComponent<Image>();
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void DeiverSupply(float deliveredSupply)
    {
        
    }
}

enum DistrictState
{
    peace,
    fight,
    captured
}