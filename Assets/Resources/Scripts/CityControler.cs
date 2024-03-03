using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CityControler : MonoBehaviour
{
    [SerializeField]
    Sprite[] districtsSprites;

    District[] districts = new District[0];
    Transform[] districtsObj = new Transform[0];
    [SerializeField]
    float mapScale;

    private void OnValidate()
    {
        //if added new district
        if (districtsSprites.Length > districts.Length)
        {
            for(int i = districtsObj.Length; i < districtsSprites.Length; i++)
                CreateDistrict(i);   
        }
        //if removed district
        else if (districtsSprites.Length < districts.Length)
        {
            //remove last element from districtsObj
            EditorApplication.delayCall += () =>
            {
                for (int i = districts.Length; i > districtsSprites.Length; i--)
                    RemoveDistrict();
            };
        }
        //if set sprite to array
        for (int i = 0; i < districtsSprites.Length; i++)
        {
            districts[i].SetSprite(districtsSprites[i]);
        }
    }

    private async void RemoveDistrict()
    {
        DestroyImmediate(districtsObj[districtsObj.Length - 1].gameObject);
        Array.Resize(ref districtsObj, districtsObj.Length - 1);
        Array.Resize(ref districts, districts.Length - 1);
    }

    private void CreateDistrict(int id)
    {
        //create new districtObj
        Array.Resize(ref districtsObj, id+1);
        Array.Resize(ref districts, id+1);
        //create nw districtObj
        districtsObj[id] = new GameObject("District " + (id)).AddComponent<RectTransform>();
        districtsObj[id].SetParent(transform);
        districtsObj[id].localPosition = new Vector3(0, 0, 0);
        districtsObj[id].localScale = Vector3.one*mapScale ;
        districtsObj[id].gameObject.AddComponent<District>();
        districtsObj[id].gameObject.AddComponent<Image>();
        //set district to array
        districts[id] = districtsObj[id].GetComponent<District>();
        districts[id].Create(districtsObj[id].transform);
        districtsSprites[id] = GetSprite(id);
        districts[id].SetSprite(districtsSprites[id]);
    }

    private Sprite GetSprite(int number)
    {
        //get file from resources/sprites/Map
        try
        {
            Debug.Log((number+1));
            return Resources.Load<Sprite>("Sprites/Map/" + (number+1));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }
}
