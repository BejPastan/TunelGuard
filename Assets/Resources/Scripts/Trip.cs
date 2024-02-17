using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Trip : MonoBehaviour
{
    [SerializeField]
    NPCCard[] members;
    [SerializeField]
    int membersNum = 2;
    int[] toReleas = new int[0];
    [SerializeField]
    GuideUI guideUI;
    Button tripButton;

    bool middleTrip = false;

    float tripTime = 5;//how long trip takes

    public void PrepareTrip(Button button)
    {
        members = new NPCCard[membersNum];
        tripButton = button;
        //find preparing trip object
        PreparingTrip preparingTrip = FindAnyObjectByType<PreparingTrip>();
        //start minigame
        preparingTrip.StartMinigame(this);
    }

    /// <summary>
    /// SEtting new member to trip, return true if there is a free slot
    /// </summary>
    /// <param name="newMember"></param>
    /// <param name="toReleas"></param>
    /// <returns></returns>
    public bool SetMembers(NPCCard newMember, bool toReleas)
    {
        //put newMember into members array
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] == null)
            {
                members[i] = newMember;
                if(toReleas)
                {
                    Array.Resize(ref this.toReleas, this.toReleas.Length + 1);
                    this.toReleas[this.toReleas.Length - 1] = i;
                }
                guideUI.SetMembers(newMember);
                //check if any nulls left
                if (members.Contains(null))
                {
                    return true;
                }
            }
        }
        return false;
    }



    //start trip
    public void StartTrip()
    {
        if (members[0]!=null)
        {
            tripButton.interactable = false;
            //remove people from waiting room
            for (int i = 0; i < members.Length; i++)
            {
                //remove NPC from waiting room
                if (members[i] != null)
                    members[i].HideNPC();
                else
                    break;
            }
            //start trip progress
            Debug.Log("Start trip");
            StartCoroutine(TripProgress());
        }
    }

    //IEnumarator update trip progress and check if trip is over
    IEnumerator TripProgress()
    {
        float tripDistance = 0;
        while (tripDistance<tripTime/2)
        {
            tripDistance += Time.deltaTime;
            guideUI.ChangeTripProgress(2*tripDistance/tripTime);
            yield return null;
        }
        if(!middleTrip)
        {
            MiddleTrip();
            middleTrip = true;
        }
        while (tripDistance>0)
        {
            tripDistance -= Time.deltaTime;
            guideUI.ChangeTripProgress(2*tripDistance / tripTime);
            yield return null;
        }
        EndTrip();
        StopCoroutine(TripProgress());
    }

    //middle of trip
    private void MiddleTrip()
    {
        guideUI.RemoveMember(toReleas);
        //iterate through toReleas array
        for (int i = 0; i < toReleas.Length; i++)
        {
            //release NPC
            Debug.Log("Release NPC");
            members[toReleas[i]].ReleasNPC();
            members[toReleas[i]] = null;
        }
        //remove empty cells from members array
        //members = members.Where(x => x != null).ToArray();
    }

    private void EndTrip()
    {       
        //return people to waiting roome
        for (int i = 0; i < members.Length; i++)
        {
            //return NPC to waiting room
            if (members[i] != null)
                members[i].ReturnNPCToWaitingRoom();
        }
        guideUI.ClearCard();
        //find Supply object
        Supplies supplies = FindAnyObjectByType<Supplies>();
        //deliver supply
        int delivered = 0;
        for (int i = 0; i < members.Length; i++)
        {
            //add supply to delivered
            if (members[i] != null)
            {
                delivered += members[i].GetNPC().CalcCapacity();
            }
        }
        supplies.DeliverSupply(delivered);
        //reset members array
        members = new NPCCard[membersNum];
        middleTrip = false;
        tripButton.interactable = true;
    }
}
