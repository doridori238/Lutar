using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum KICK_TYPE
{
   HIGH,
   MIDDLE,
   LOW
}
public enum PUNCH_TYPE
{
   PLEFT,
   PRIGHT
}

public enum MOVE
{ 
   LEFT,
   RIGHT
}

public enum PARRYING
{ 
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

    public string[] kickNames = new string[3];
    public string[] punchNames = new string[2];
    public string[] moves = new string[2];
    public string[] parrying = new string[1];
        
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
            StartCoroutine(AddInputKeyCo());
         
      }
        gameManager.PlayState = true;
    }

      
    IEnumerator AddInputKeyCo() 
    {
       if (time <= 0.5f)
       {
          if (Input.GetKey(key.highKickKey))
            {
                gameManager.PlayState = true;
                animator.SetTrigger(kickNames[(int)KICK_TYPE.HIGH]);
          }
          else if (Input.GetKey(key.lowKickKey))
            {
                gameManager.PlayState = true;
                animator.SetTrigger(kickNames[(int)KICK_TYPE.LOW]);
          }
          else
            {
                gameManager.PlayState = true;
                animator.SetTrigger(kickNames[(int)KICK_TYPE.MIDDLE]);
                    
          }
       }
       yield return null;
    }

    public void Punch()
    {
        gameManager.PlayState = true;
        if (Input.GetKeyDown(key.jabKey))
       {
          animator.SetTrigger(punchNames[(int)PUNCH_TYPE.PLEFT]);
       }

       if (Input.GetKeyDown(key.punchKey))
       {
          animator.SetTrigger(punchNames[(int)PUNCH_TYPE.PRIGHT]);
       }

    }

    public void Parrying()
    {
        gameManager.PlayState = true;
        if (Input.GetKeyDown(key.parryingKey))
       {
          animator.SetTrigger(parrying[(int)PARRYING.PARRY]);
       }


    }


    public void PlayerMove()
    {
        gameManager.PlayState = true;
        if (Input.GetKeyDown(key.forwardKey))
       {
            if (Vector3.Distance(player.targetCharacter.transform.position,player.transform.position) >= 1)
            { 
              animator.SetTrigger( moves[(int)MOVE.RIGHT]);
            }
       }

       if (Input.GetKeyDown(key.backwardKey))
       {
          animator.SetTrigger(moves[(int)MOVE.LEFT]);

       }
    }

    


    void ResetAllTrigger()
    {
        foreach(string s in kickNames)
        {
            animator.ResetTrigger(s);
        }
        foreach (string s in punchNames)
        {
            animator.ResetTrigger(s);
        }
        foreach (string s in moves)
        {
            animator.ResetTrigger(s);
        }foreach(string s in parrying)
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
