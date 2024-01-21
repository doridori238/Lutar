using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum AllMove_TYPE
{
    HIGHKICK,
    MIDDLEKICK,
    LOWKCIK,

    PLEFTPUNCH,
    PRIGHTPUNCH,

    LMOVE,
    RMOVE,

    PARRY
}


[Serializable]
public struct PlayerKey
{
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode highKickKey;
    public KeyCode middleKickKey;
    public KeyCode lowKickKey;
    public KeyCode parryingKey;
    public KeyCode jabKey;
    public KeyCode punchKey;
   public PlayerKey(PlayerData keyDate)
    {

        forwardKey = (KeyCode)keyDate.forWardKey;
        backwardKey = (KeyCode)keyDate.backWardKey;
        highKickKey = (KeyCode)keyDate.highKickKey;
        middleKickKey = (KeyCode)keyDate.middleKickKey;
        lowKickKey = (KeyCode)keyDate.lowKickKey;
        parryingKey = (KeyCode)keyDate.parryingKey;
        jabKey = (KeyCode)keyDate.jabKey;
        punchKey = (KeyCode)keyDate.punchKey;

    }
}

public class AnimatorManager : MonoBehaviour
{
    
    public PlayerKey key;

    public string[] allMoveName = new string[8];
        
    public Animator animator = null;
    private float time = 0;
    Character player;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = this.GetComponent<Character>();


    }

    public void InputKeycodeK() 
    {
      if (Input.GetKeyDown(key.middleKickKey))
      {
            GameManager.instance.PlayState = true;
            StartCoroutine(AddInputKeyCo());
      }
    }


    public class KeyInputYield : CustomYieldInstruction
    {
        Dictionary<KeyCode, Action> keyCodeDic = new Dictionary<KeyCode, Action>();
        KeyCode[] keycodeArr;
        public KeyInputYield(KeyCode[] keycodeArr, Action[] action)
        {
            this.keycodeArr = keycodeArr;
            for (int i = 0; i < keycodeArr.Length; i++)
                keyCodeDic.Add(keycodeArr[i], action[i]);   
        }
        public override bool keepWaiting
        {
            get 
            {
                foreach (KeyCode keyCode in keycodeArr)
                {      
                   if (Input.GetKey(keyCode))
                   {
                       keyCodeDic[keyCode]();
                       return false;
                   }                       
                }
                return true;
            }   
        }
    }

    IEnumerator AddInputKeyCo()
    {
        KeyInputYield customCo = new KeyInputYield(
                            new KeyCode[] { key.highKickKey, key.lowKickKey, key.middleKickKey },
                            new Action[] { () => { animator.SetTrigger(allMoveName[(int)AllMove_TYPE.HIGHKICK]); }
                                          ,() => { animator.SetTrigger(allMoveName[(int)AllMove_TYPE.LOWKCIK]); } 
                                          ,()=>  { animator.SetTrigger(allMoveName[(int)AllMove_TYPE.MIDDLEKICK]);
                                          } });
        yield return customCo;
           
    }


    public void Punch()
    {
        GameManager.instance.PlayState = true;
        if (Input.GetKeyDown(key.jabKey))
       {
          animator.SetTrigger(allMoveName[(int)AllMove_TYPE.PLEFTPUNCH]);
       }

       if (Input.GetKeyDown(key.punchKey))
       {
          animator.SetTrigger(allMoveName[(int)AllMove_TYPE.PRIGHTPUNCH]);
       }

    }

    public void Parrying()
    {
        GameManager.instance.PlayState = true;
        if (Input.GetKeyDown(key.parryingKey))
       {
          animator.SetTrigger(allMoveName[(int)AllMove_TYPE.PARRY]);
       }


    }


    public void PlayerMove()
    {
        GameManager.instance.PlayState = true;
        if (Input.GetKeyDown(key.forwardKey))
       {
            if (Vector3.Distance(GameManager.instance.mPlayerCopy.transform.position, GameManager.instance.wPlayerCopy.transform.position) >= 1)
            {
                animator.SetTrigger(allMoveName[(int)AllMove_TYPE.RMOVE]);
            }
            else return;
       }

       if (Input.GetKeyDown(key.backwardKey))
       {
          animator.SetTrigger(allMoveName[(int)AllMove_TYPE.LMOVE]);
       }
    }



    /// <summary>
    /// 키 누적 막음
    /// </summary>
    void ResetAllTrigger()
    {

        foreach (string s in allMoveName)
        {
            animator.ResetTrigger(s);
        }

    }


    void Update()
    {
       PlayerMove();
       InputKeycodeK(); 
       Punch();
       Parrying();
    }

    
}
