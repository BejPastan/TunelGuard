using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField]
    GameObject membersChart;
    [SerializeField]
    Animator ToggleAnimator;
    [SerializeField]
    Image[] membersImages = new Image[0];
    [SerializeField]
    GameObject memberImagePref;
    [SerializeField]
    Transform MEMBERS_CHART;
    [SerializeField]
    Slider TRIP_PROGRESS;

    bool membersChartActive = false;
    
    public void ToggleMembersChart(Button button)
    {
        if (membersChartActive)
        {
            ToggleAnimator.SetBool("Toggle", false);
            membersChartActive = false;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Show";
        }
        else
        {
            ToggleAnimator.SetBool("Toggle", true);
            membersChartActive = true;
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Hide";
        }
    }

    public void SetMembers(NPCCard members)
    {
        //Resize membersImages array
        Array.Resize(ref membersImages, membersImages.Length + 1);
        //instatiate new member image
        membersImages[membersImages.Length-1] = Instantiate(memberImagePref, MEMBERS_CHART).GetComponent<Image>();
        Debug.Log(membersImages[membersImages.Length - 1]);
        membersImages[membersImages.Length - 1].sprite = members.GetSprite();
        //set new member image rect position
        SetCardPosition(membersImages.Length - 1);
    }

    //change trip progress
    public void ChangeTripProgress(float progress)
    {
        TRIP_PROGRESS.value = progress;
    }

    public void RemoveMember(int[] index)
    {
        for(int i = 0; i < index.Length; i++)
        {
            Destroy(membersImages[index[i]].gameObject);
            membersImages[index[i]] = null;
        }
        //remove empty space from membersImage array
        membersImages = membersImages.Where(x => x != null).ToArray();
        //set new positions
        for (int i = 0; i < membersImages.Length; i++)
        {
            SetCardPosition(i);
        }
    }

    public void ClearCard()
    {
        //remove all members
        for (int i = 0; i < membersImages.Length; i++)
        {
            Destroy(membersImages[i].gameObject);
        }
        membersImages = new Image[0];
    }

    public void SetCardPosition(int index)
    {
        membersImages[index].rectTransform.anchoredPosition = new Vector3(0, (-50 * (index+1)), 0);
    }
}
