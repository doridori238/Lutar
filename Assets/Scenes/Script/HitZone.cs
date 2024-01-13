using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Presets;
using UnityEngine;

public enum HIT_TYPE
{
    BODY,
    HEAD,
    LEG
}

[System.Serializable]
public struct HitZoneDate
{
   
    public AudioClip hitClip;
    public GameObject soundPrefab;

    public Character characterScript;
    public Character targetPlayer;
    public float hitMul;

    public AttackZone attackZone;
}


public class HitZone : MonoBehaviour, IHitAble
{
    public HitZoneDate hitZoneDate = new HitZoneDate();


    public Animator animator = null;
    public bool isNextAttackBonus = false;

    [SerializeField] HIT_TYPE Type;

    public HIT_TYPE TYPE
    {
        get { return Type; }
        set { 
           
            Type = value;
            animatorDel(atkDic[Type]);
        }
    }

    Dictionary<HIT_TYPE, string> atkDic = new Dictionary<HIT_TYPE, string>();

    public delegate void CustomDel(string animatorName);
    public CustomDel animatorDel;

    public Action colliderAction;
    public Action actionEffectAdd;
   
    private void Start()
    {
        Character character = transform.root.GetComponent<Character>();
        hitZoneDate.characterScript = character;
        animator = character.gameObject.GetComponent<Animator>();
        hitZoneDate.targetPlayer = character.targetCharacter;
        


        atkDic.Add(HIT_TYPE.HEAD, "HitHead");
        atkDic.Add(HIT_TYPE.BODY, "HitBody");
        atkDic.Add(HIT_TYPE.LEG, "HitLeg");


        animatorDel = (string animatorName) => { animator.SetTrigger(animatorName); };
        colliderAction = () => {
                foreach (var getHitColliders in hitZoneDate.characterScript.hitColliders)
                {
                    getHitColliders.enabled = false;
                }
            };
        
    }


    private void OnTriggerEnter(Collider other) 
    {
      
        //AttackZone attackZone = null;
        if (other.GetComponent<AttackZone>() == null)
            return;
        else
            hitZoneDate.attackZone = other.GetComponent<AttackZone>();


        if (hitZoneDate.characterScript.IsParrying == true)
        {
            colliderAction();
            SoundManager.instance.Play(hitZoneDate.hitClip);
            hitZoneDate.characterScript.IsNextAttackBonus = true;

        }

        else
        {       
           Hit(hitZoneDate.attackZone.attackZoneDate.atk * (int)hitZoneDate.hitMul);
           //TYPE = Type;
            
            hitZoneDate.targetPlayer.IsNextAttackBonus = false;
        }

    }


    public void Hit(int damage)
    {
        if (hitZoneDate.characterScript.IsParrying)
        {
            hitZoneDate.characterScript.IsNextAttackBonus = true;
            return;
        }

        SoundManager.instance.Play(hitZoneDate.hitClip);
        hitZoneDate.characterScript.Hp -= damage;
        
    }
}


