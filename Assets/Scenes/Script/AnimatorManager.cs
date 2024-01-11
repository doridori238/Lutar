using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


[System.Serializable]
public class PlayerKey
{
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode highKickKey;
    public KeyCode middleKickKey;
    public KeyCode lowKickKey;
    public KeyCode parryingKey;
    public KeyCode jabKey;
    public KeyCode punchKey;
}


public class AnimatorManager : MonoBehaviour
{
    public GameManager gameManager;

    public PlayerKey key;

    public string[] allMoveName = new string[8];
        
    public Animator animator = null;
    private float time = 0;
    Character player;


    void Start()
    {
       animator = GetComponent<Animator>();
        gameManager = GameManager.instance;
        player = GetComponent<Character>();
    }

    public void InputKeycodeK() 
    {
      if (Input.GetKeyDown(key.middleKickKey))
      {
            gameManager.PlayState = true;
            StartCoroutine(AddInputKeyCo());
      }
    }

      
    IEnumerator AddInputKeyCo() 
    {
       if (time <= 0.5f)
       {
          if (Input.GetKey(key.highKickKey))           
                animator.SetTrigger(allMoveName[(int)AllMove_TYPE.HIGHKICK]);         
          
          else if (Input.GetKey(key.lowKickKey))                        
                animator.SetTrigger(allMoveName[(int)AllMove_TYPE.LOWKCIK]);          
         
          else            
                animator.SetTrigger(allMoveName[(int)AllMove_TYPE.MIDDLEKICK]);
                    
         
       }
       yield return null;
    }

    public void Punch()
    {
        gameManager.PlayState = true;
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
        gameManager.PlayState = true;
        if (Input.GetKeyDown(key.parryingKey))
       {
          animator.SetTrigger(allMoveName[(int)AllMove_TYPE.PARRY]);
       }


    }


    public void PlayerMove()
    {
        gameManager.PlayState = true;
        if (Input.GetKeyDown(key.forwardKey))
       {
            if (Vector3.Distance(player.targetCharacter.transform.position, player.transform.position) >= 1)
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
