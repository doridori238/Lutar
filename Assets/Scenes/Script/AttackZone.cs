using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static AttackZone;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class AttackZoneDate
{
    public GameObject parryingPrefab;
    public GameObject parryingCopy;

    public GameObject effectcopy;
    public GameObject baseEffectPrefab;

    public Action colliderAction;
    public Action actionEffectAdd;
    public Action actionbonusEffectAdd;

    public Character characterScript;
    public int atk;
    public bool bonusValue;
}


public class AttackZone : MonoBehaviour, IAttackAble
{
    public AttackZoneDate attackZoneDate = new AttackZoneDate();
    
    private void Start()
    {
        attackZoneDate.characterScript = transform.root.GetComponent<Character>();
        attackZoneDate.colliderAction = () => {
            foreach (var getAtkColliders in attackZoneDate.characterScript.atkColliders)
            {
                getAtkColliders.enabled = false;
            }
        };

        attackZoneDate.bonusValue = attackZoneDate.characterScript.IsNextAttackBonus;
        attackZoneDate.actionEffectAdd = () => {  attackZoneDate.effectcopy = Instantiate(attackZoneDate.baseEffectPrefab, transform.position, transform.rotation); };
        attackZoneDate.actionbonusEffectAdd = () => { attackZoneDate.parryingCopy = Instantiate(attackZoneDate.parryingPrefab, transform.position, transform.rotation); };
    }


    private void OnTriggerEnter(Collider other)
    {      
        attackZoneDate.colliderAction();
        
    }

    public void Attack(IHitAble hitAble)
    {
        if (attackZoneDate.bonusValue == true)
        {
            hitAble.Hit(attackZoneDate.atk * 2);
            attackZoneDate.actionbonusEffectAdd();
            attackZoneDate.bonusValue = !attackZoneDate.bonusValue;

        }
        else
            hitAble.Hit(attackZoneDate.atk);
        attackZoneDate.actionEffectAdd();
            
    }

  
}
