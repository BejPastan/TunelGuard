using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    Health health;
    [SerializeField]
    Age age;
    [SerializeField]
    Gender gender;
    [SerializeField]
    SpriteRenderer[] spriteRenderers;

    //create new NPC
    public void CreateNPC(float injuryChance, float ageChance, float genderChance)
    {
        //set health
        SetHealth(injuryChance);
        //set age
        SetAge(ageChance);
        //set gender
        SetGender(genderChance);
        CalcCapacity();
        MakeCharacter();
    }

    //set sprites for this NPC
    private void MakeCharacter()
    {
        //get all sprites for head from resources
        List<Sprite> headSprites = Resources.LoadAll<Sprite>("Sprites/Characters/HEADS").ToList();
        SelectSprite(ref headSprites, gender, Health._, age);
        SetSprite(ref headSprites, spriteRenderers[0]);
        //get all sprites for body from resources
        List<Sprite> bodySprites = Resources.LoadAll<Sprite>("Sprites/Characters/BODIES").ToList();
        SelectSprite(ref bodySprites, gender, health, age);
        SetSprite(ref bodySprites, spriteRenderers[1]);
        //get all sprites for legs from resources
        List<Sprite> legsSprites = Resources.LoadAll<Sprite>("Sprites/Characters/LEGS").ToList();
        SelectSprite(ref legsSprites, gender, Health._, age);
        SetSprite(ref legsSprites, spriteRenderers[2]);
        SetSprite(ref legsSprites, spriteRenderers[3]);
        //get all sprites for arms from resources
        List<Sprite> armsSprites = Resources.LoadAll<Sprite>("Sprites/Characters/ARMS").ToList();
        SelectSprite(ref  armsSprites, gender, Health._, age);
        SetSprite(ref armsSprites, spriteRenderers[4]);
        SetSprite(ref armsSprites, spriteRenderers[5]);

        //based on health remove some sprites
        //select random arm and leg
        if(health == Health.Medium)
        {
            //select one random leg or arm to remove    
            int legOrArm = Random.Range(2, 6);
            RemoveSprite(spriteRenderers[legOrArm]);
        }
        else if(health == Health.Heavy)
        {
            //select two random legs or arms to remove
            int legOrArm1 = Random.Range(2, 6);
            int legOrArm2 = Random.Range(2, 6);
            while(legOrArm1 == legOrArm2)
            {
                legOrArm2 = Random.Range(0, 4);
            }
            RemoveSprite(spriteRenderers[legOrArm1]);
            RemoveSprite(spriteRenderers[legOrArm2]);
        }
    
        //set color for legs
        Color legsColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1));
        spriteRenderers[2].color = legsColor;
        spriteRenderers[3].color = legsColor;
        //set color for arms and body
        Color armsColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1));
        spriteRenderers[1].color = armsColor;
        spriteRenderers[4].color = armsColor;
        spriteRenderers[5].color = armsColor;

        //if child set scale to 0.6
        if(age == Age.Child)
        {
            //get sprite gameobject
            Transform sprite = transform.GetChild(0);
            sprite.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }

    //remove sprite
    private void RemoveSprite(SpriteRenderer renderer)
    {
        renderer.sprite = null;
    }

    //selecting Sprite
    private void SelectSprite(ref List<Sprite> sprites, Gender gender = Gender._, Health health = Health._, Age age = Age._)
    {;
        for(int i=0; i<sprites.Count; i++)
        {
            if (sprites[i].name.Contains(gender.ToString()) && sprites[i].name.Contains(health.ToString()) && sprites[i].name.Contains(age.ToString()))
            {

            }
            else
            {
                sprites.RemoveAt(i);
                i--;
            }
        }
    }

    //set sprite
    private void SetSprite(ref List<Sprite> sprites, SpriteRenderer renderer)
    {
        int spriteIndex = Random.Range(0, sprites.Count);
        renderer.sprite = sprites[spriteIndex];
    }

    //set health
    private void SetHealth(float injuryChance)
    {
        float healthRoll = Random.Range(0f, 1f);
        if (healthRoll < injuryChance)
        {
            health = Health.Light;
        }
        else if (healthRoll < injuryChance * 2)
        {
            health = Health.Medium;
        }
        else if (healthRoll < injuryChance * 3)
        {
            health = Health.Heavy;
        }
        else
        {
            health = Health.Healthy;
        }
    }
    
    //set age
    private void SetAge(float ageChance)
    {
        float ageRoll = Random.Range(0f, 1f);
        if (ageRoll < ageChance*0.75)
        {
            age = Age.Child;
        }
        else if (ageRoll < ageChance * 1.5)
        {
            age = Age.Old;
        }
        else
        {
            age = Age.Adult;
        }
        
        if (age == Age.Child)
        {
            //set scale to 0.6
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }

    //set gender
    private void SetGender(float genderChance)
    {
        float genderRoll = Random.Range(0f, 1f);
        if (genderRoll < genderChance)
        {
            gender = Gender.Female;
        }
    }

    //calculate capacity
    public int CalcCapacity()
    {
        float capacity = 8;
        //decrease capacity based on health
        if (health == Health.Light)
        {
            capacity = capacity * 0.75f;
        }
        else if (health == Health.Medium)
        {
            capacity = capacity * 0.25f;
        }
        else if (health == Health.Heavy)
        {
            capacity = capacity * 0f;
        }
        //decrease capacity based on age
        if (age == Age.Child)
        {
            capacity = capacity * 0.25f;
        }
        else if (age == Age.Old)
        {
            capacity = capacity * 0.75f;
        }
        //decreas capacity based on gender
        if(gender == Gender.Female)
        {
            capacity = capacity * 0.8f;
        }

        return (int)capacity;
    }



    public void Hide()
    {
        //hide all sprite renderers
        for(int i=0; i<spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = false;
        }
    }

    public void Unhide()
    {
        //unhide all sprite renderers
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = true;
        }
    }

    public void Remove()
    {
        //remove NPC from waiting room
        Destroy(gameObject);
    }
}

enum Health
{
    Healthy,//zdrowy
    Light,//rany na ciele
    Medium,//upierdala jedną kończynę
    Heavy,//upierdala dwie kończyny
    _,//null
}

enum Age
{
    Adult,
    Old,
    Child,
    _,//null
}

enum Gender
{
    Male,
    Female,
    _,//null
}
