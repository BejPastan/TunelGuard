using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class WaitingRoom : MonoBehaviour
{
    //NPCs in the waiting room list
    [SerializeField]
    NPC[] npc = new NPC[0];
    [SerializeField]
    NPCCard[] npcCards = new NPCCard[0];
    //NPC gameobjects slots
    [SerializeField]
    Transform[] npcSlots = new Transform[0];

    int npcSlotsLeft = 0;

    //list of NPC's prefabs
    public GameObject npcPrefabs;
    public GameObject npcCardPrefab;

    public void StartGame()
    {
        Timer.OnStartOfDay += NewDay;
    }

    private void NewDay()
    {
        //get random number of new NPCs
        int newNPCs = Random.Range(1, 3);
        //get new NPCs
        for (int i = 0; i < newNPCs; i++)
        {
            GetNewNPC();
        }
    }
    
    //get new NPC to the waiting room
    private void GetNewNPC()
    {
        //check if there is a free slot
        if (npcSlotsLeft < npcSlots.Length)
        {
            //create new NPC and NPC card
            NPC newNPC = CreateNPC(npcSlotsLeft);
            NPCCard newNPCCard = CreateNPCCard(ref newNPC);
            //resize arrays
            System.Array.Resize(ref npc, npcSlotsLeft+1);
            System.Array.Resize(ref npcCards, npcSlotsLeft+1);
            //add new NPC and NPC card to arrays
            npc[npcSlotsLeft] = newNPC;
            npcCards[npcSlotsLeft] = newNPCCard;
            //decrease free slots
            npcSlotsLeft++;
        }
    }

    //create new NPC
    private NPC CreateNPC(int slotNum)
    {
        //create new NPC
        GameObject newNPC = Instantiate(npcPrefabs);
        newNPC.transform.SetParent(npcSlots[slotNum]);
        newNPC.transform.localPosition = Vector3.zero;
        newNPC.transform.localScale = Vector3.one;
        NPC npc = newNPC.GetComponent<NPC>();
        //in future add randomization of NPC's parameters
        npc.CreateNPC(0.5f, 0.5f, 0.5f);
        return npc;
    }

    //create new NPC card
    private NPCCard CreateNPCCard(ref NPC npc)
    {
        //create new NPC card
        //in future add instantiation of NPC card
        GameObject newNPCCard = Instantiate(npcCardPrefab);
        newNPCCard.transform.SetParent(transform);
        NPCCard npcCard = newNPCCard.GetComponent<NPCCard>();
        npcCard.SetNPC(npc);
        return npcCard;
    }

    //remove NPC from waiting room
    public void RemoveNPC(NPC npc)
    {
        //iterate through all NPCs
        for(int i=0; i<this.npc.Length; i++)
        {
            //check if NPC is null
            if (this.npc[i] == npc)
            {
                this.npc[i] = null;
            }
        }
    }

    //static void remove NPC card from waiting room
    public void RemoveNPCCard(NPCCard npcCard)
    {
        //iterate through all NPC cards
        for (int i = 0; i < npcCards.Length; i++)
        {
            //check if NPC card is null
            if (npcCards[i] == npcCard)
            {
                npcCards[i] = null;
            }
        }
    }

    public void ReturnNPC(NPC toReturn, NPCCard NPCCard)
    {
        //find NPCCard in array
        for(int i=0; i<npcCards.Length; i++)
        {
            if (npcCards[i] == NPCCard)
            {
                //return NPC to waiting room
                npc[i] = toReturn;
                npcCards[i] = NPCCard;
                return;
            }
        }
    }

    //get NPC cards from waiting room
    public NPCCard[] GetCardsFromNPC()
    {
        NPCCard[] ToReturn = new NPCCard[0];
        //iterate through all NPCs
        for (int i = 0; i < npc.Length; i++)
        {
            //check if NPC is null
            if (npc[i] != null)
            {
                //resize array
                System.Array.Resize(ref ToReturn, ToReturn.Length + 1);
                //add NPC card to array
                ToReturn[ToReturn.Length - 1] = npcCards[i];
            }
        }
        return ToReturn;
    }

    //return NPCs array from waiting room
    public NPC[] GetNPCs()
    {
        return npc;
    }
}
