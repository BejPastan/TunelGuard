using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCard : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    Sprite sprite;
    NPC npc;

    Camera screenCamera;

    //get screen camera
    private void GetCamera()
    {
        screenCamera = GameObject.Find("Screen Camera").GetComponent<Camera>();
    }

    private void MakePhoto()
    {
        screenCamera.transform.position = npc.transform.position + new Vector3(0, 1, -10);

        //change camera ration to 1:1
        screenCamera.aspect = 1;
        //get screencamera view
        RenderTexture rt = new RenderTexture(256, 256, 24);
        screenCamera.targetTexture = rt;
        screenCamera.Render();
        RenderTexture.active = rt;
        //create texture
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        screenShot.Apply();
        //set texture to sprite
        sprite = Sprite.Create(screenShot, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        //set sprite to spriteRenderer
        spriteRenderer.sprite = sprite;
    }

    //NPC getter
    public NPC GetNPC()
    {
        return npc;
    }

    /// <summary>
    /// Initialize NPC card
    /// </summary>
    /// <param name="npc"></param>
    //set NPC
    public void SetNPC(NPC npc)
    {
        this.npc = npc;
        GetCamera();
        MakePhoto();
        Hide();
    }


    /// <summary>
    /// Hide card
    /// </summary>
    public void Hide()
    {
        //unactivate card sprite renderer
        spriteRenderer.enabled = false;
    }
    
    /// <summary>
    /// Hide NPC in waiting room
    /// </summary>
    public void HideNPC()
    {
        npc.Hide();
        RemoveNPCFromWaitingRoom();
    }

    /// <summary>
    /// Unhide card
    /// </summary>
    public void Unhide()
    {
        spriteRenderer.enabled = true;
        //set random rotation in range -20 to 20
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-20, 20));
        //set position to 0,0,0
        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Remove NPC from waiting room
    /// </summary>
    public void RemoveNPCFromWaitingRoom()
    {
        //get waiting room object
        WaitingRoom waitingRoom = FindAnyObjectByType<WaitingRoom>();
        //remove NPC from waiting room
        waitingRoom.RemoveNPC(npc);
    }

    public void ReturnNPCToWaitingRoom()
    {
        WaitingRoom waitingRoom = FindAnyObjectByType<WaitingRoom>();
        waitingRoom.ReturnNPC(npc, this);
        npc.Unhide();
    }

    /// <summary>
    /// Remove NPC from waiting room and destroy NPC and NPC card
    /// </summary>
    public void ReleasNPC()
    {
        //remove NPC from waiting room
        RemoveNPCFromWaitingRoom();
        npc.Remove();
        //remove NPC card
        Destroy(gameObject);
    }

    /// <summary>
    /// Return card sprite
    /// </summary>
    /// <returns></returns>
    //get sprite
    public Sprite GetSprite()
    {
        return sprite;
    }
}
