using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparingTrip : MonoBehaviour
{
    //canva bakground
    [SerializeField]
    GameObject canvaBackground;

    //NPCCards
    NPCCard[] npcCards;
    
    bool minigame = false;
    int npcCardNum = 0;

    Trip trip;

    //start Minigame
    public void StartMinigame(Trip trip)
    {
        //freez time
        GameController.FreezTime();
        //enabel canva background
        canvaBackground.SetActive(true);
        //start minigame
        //get NPC cards from waiting room
        npcCards = FindAnyObjectByType<WaitingRoom>().GetCardsFromNPC();
        //get all NPC from waiting room
        NPC[] npcs = FindAnyObjectByType<WaitingRoom>().GetNPCs();
        //activate NPC cards
        for (int i = 0; i < npcCards.Length; i++)
        {
            if (npcs[i] != null)
            {
                Debug.Log(npcCards[i]);
                npcCards[i].Unhide();
            }
        }
        minigame = true;
        this.trip = trip;
        npcCardNum = npcCards.Length-1;
        if(npcCardNum == -1)
        {
            StopMinigame();
        }
    }

    private void Update()
    {
        
        if(minigame)
        {
            //check if player push arrow key or one of WASD keys
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log(npcCardNum);
                StartCoroutine(PushCard(npcCards[npcCardNum], new Vector3(0, 10, 0)));
                if(!trip.SetMembers(npcCards[npcCardNum], true))
                {
                    StopMinigame();
                }
                npcCardNum--;

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log(npcCardNum);
                StartCoroutine(PushCard(npcCards[npcCardNum], new Vector3(0, -10, 0)));
                if(!trip.SetMembers(npcCards[npcCardNum], false))
                {
                    StopMinigame();
                }
                npcCardNum--;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(PushCard(npcCards[npcCardNum], new Vector3(-20, 0, 0)));
                npcCardNum--;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(PushCard(npcCards[npcCardNum], new Vector3(20, 0, 0)));
                npcCardNum--;
            }

            try
            {
                if (npcCards[npcCardNum]==null)
                {
                    npcCardNum--;
                }
            }
            catch
            {

            }

            //need add check minigame again to make sure that minigame wasn't stoped by other way
            if(npcCardNum == -1 && minigame)
            {
                StopMinigame();
            }

        }
    }

    //stop minigame
    public void StopMinigame()
    {
        //unfreez time
        GameController.UnfreezTime();
        //hide NPC cards
        for (int i = 0; i < npcCards.Length; i++)
        {
            npcCards[i].Hide();
        }
        trip.StartTrip();
        //disabel canva background
        canvaBackground.SetActive(false);
        minigame = false;
        npcCards = new NPCCard[0];
    }

    //push NPC card in the direction
    private IEnumerator PushCard(NPCCard card, Vector3 direction)
    {
        
        //get card position
        Vector3 cardPosition = card.transform.position;
        //get target position
        Vector3 targetPosition = cardPosition + direction;
        //get time
        float time = 0;
        //move card
        while (time < 0.5f)
        {
            //get real time beetwen frames
            time += Time.unscaledDeltaTime;
            card.transform.position = Vector3.Lerp(cardPosition, targetPosition, time)*2;
            yield return null;
        }
        //stop corutine
        yield break;
    }

}