using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    private int hp;

    public bool leftPlayer;

    private Quaternion originRot;
    private bool isAttacking;
    private bool isNextAttackBonus = false;
    public Character targetCharacter;
    public Animator animator;
    
    public AnimatorManager animatorManager;
  
    public GameObject bonusePrefab;
    private GameObject bonuseCopy;
    public GameObject winPrefab;
    private GameObject winPrefabCopy;
    public GameObject losePrefab;
    private GameObject loseCopy;

    public AudioClip bonusClip;

    public Collider[] atkColliders = new Collider[6];
    public Collider[] hitColliders = new Collider[6];

    public AudioClip humanClip;

    private int maxHp;

    public bool IsNextAttackBonus
    {  
       get { return isNextAttackBonus; }
       set { isNextAttackBonus = value;}
    }


    IEnumerator CoolTimeCo()
    {
        while(true)
        {
            if(IsNextAttackBonus)
            {
                AttackBonus();
                SoundManager.instance.Play(bonusClip);
                yield return new WaitForSeconds(3);
                IsNextAttackBonus = false;
            }
            yield return null;
        }
    }

    private void Start()
    {
        
        animatorManager = GetComponent<AnimatorManager>();
        StartCoroutine(CoolTimeCo());
        
        originRot = transform.rotation;
    }


    public int MaxHp
    {
        get => maxHp;
    }



    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            UiManager.instance.HpUpdate((float)hp /(float)MaxHp, leftPlayer);
            Debug.Log(Hp);

            if (this.hp <= 0)
            {
                Lose();
                targetCharacter.Win();
                
            }

        }
    }


    public void AttackBonus()
    {
        bonuseCopy = Instantiate(bonusePrefab, transform.position, transform.rotation);
    }


    public void Win()
    { 
        animator.SetTrigger("WinPose");
        winPrefabCopy = Instantiate(winPrefab, transform.position, transform.rotation);
    }


    public void Lose()
    {
        animator.SetTrigger("LosePose");
        loseCopy = Instantiate(losePrefab, transform.position, transform.rotation);
    }


    private void Update()
    {
       
        Vector3 zAxisOffset = new Vector3(transform.position.x, transform.position.y, 10);
        transform.rotation = originRot;
        transform.position = zAxisOffset;

    }

    


    private bool isattacking;
    public bool Isattacking
    {
        get { return isattacking; }
        set
        {
            isattacking = value;
            
        }
    }


    private bool isParrying;
    public bool IsParrying
    {
        get { return isParrying; }

        set
        {
            isParrying = value;
          
        }
    }


    public void ParryingStart()
    {
        IsParrying = true;
        
    }


    public void ParryingEnd()
    {
        IsParrying = false;

        foreach (var getHitColliders in hitColliders)
        {
            getHitColliders.enabled = true;
        }
    }


    public void AttackStart()
    {
        SoundManager.instance.Play(humanClip);
        isattacking = true;
        foreach (var getAtkColliders in atkColliders)
        {
            getAtkColliders.enabled = true;
        }

    }

    public void AttackEnd()
    {
        isattacking = false;
        foreach (var getAtkColliders in atkColliders)
        {
            getAtkColliders.enabled = false;
        }
       
    }

   
}


