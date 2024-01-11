using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;


public class GameManager : Singleton<GameManager>
{
    private float maxPlayTime = 100;
    public bool isMove = false;

    //public Character character;
    //public Character target;


    public TextMeshProUGUI playTimeText;
    private bool playState;

    [SerializeField] private PlayersDistance distanceObj;
    [SerializeField] private GameObject mPlayerPrefab;
    [SerializeField] private GameObject wPlayerPrefab;
    private GameObject mPlayerCopy;
    private GameObject WPlayerCopy;

    public Image leftPlayerHpBar;
    public Image rightPlayerHpBar;


    PlayerKey playerLeftKey = new PlayerKey();
    PlayerKey playerRightKey = new PlayerKey();

    Dictionary<Index, Action> playerSettingDic = new Dictionary<Index, Action>();

    Action action;

   // delegate void CustomDel(int Index)
        // eunm을 이용해서 해볼까....?


    public bool PlayState
    {
        get { return playState; }
        set
        {
            playState = value;
        }
    }


    void Start()
    {

        KetSet();

        //PlayerSet();

        // coroutine = StartCoroutine(PlayTimeCo(maxPlayTime));

        //playerSettingDic.Add( 0 , action);


        //action = () => { };




    }


    public void KetSet()
    {
        playerLeftKey.forwardKey = KeyCode.D;
        playerLeftKey.backwardKey = KeyCode.A;
        playerLeftKey.highKickKey = KeyCode.W;
        playerLeftKey.middleKickKey = KeyCode.K;
        playerLeftKey.lowKickKey = KeyCode.S;
        playerLeftKey.parryingKey = KeyCode.I;
        playerLeftKey.jabKey = KeyCode.J;
        playerLeftKey.punchKey = KeyCode.L;

        playerRightKey.forwardKey = KeyCode.LeftArrow;
        playerRightKey.backwardKey = KeyCode.RightArrow;
        playerRightKey.highKickKey = KeyCode.UpArrow;
        playerRightKey.middleKickKey = KeyCode.Keypad2;
        playerRightKey.lowKickKey = KeyCode.DownArrow;
        playerRightKey.parryingKey = KeyCode.Keypad5;
        playerRightKey.jabKey = KeyCode.Keypad1;
        playerRightKey.punchKey = KeyCode.Keypad3;

    }

    //public void PlayerLeftPosition()
    //{

    //    PlayerSet(wPlayerPrefab, new Vector3(15, (float)0.1, 7), Quaternion.Euler(new Vector3(0, 90, 0)));
    //    WPlayerCopy.SetActive(true);
    //    WPlayerCopy.GetComponent<AnimatorManager>().key = playerLeftKey;
    //    //WPlayerCopy.GetComponent<Character>().HpImage = leftPlayerHpBar;
    //    distanceObj.leftTarget = WPlayerCopy.GetComponent<Character>().target;


    //    mPlayerCopy = Instantiate(mPlayerPrefab, new Vector3(20, (float)0.1, 7), Quaternion.Euler(new Vector3(0, -90, 0))); // new Quaternion(0, -90, 0));
    //    mPlayerCopy.SetActive(true);
    //    mPlayerCopy.GetComponent<AnimatorManager>().key = playerRightKey;
    //   // mPlayerCopy.GetComponent<Character>().HpImage = rightPlayerHpBar;
    //    distanceObj.rightTarget = mPlayerCopy.GetComponent<Character>().target;


    //    //WPlayerCopy.GetComponent<Character>().targetCharacter = mPlayerCopy.GetComponent<Character>();
    //    //mPlayerCopy.GetComponent<Character>().targetCharacter = WPlayerCopy.GetComponent<Character>();

       
    //}


    public void PlayerSet(GameObject player, Vector3 pot ,Quaternion rot )
    {
        player = Instantiate(player, pot, rot);
        if (DataSaver.instance.selectIndex == 0)
        {
            player.SetActive(true);
            player.GetComponent<AnimatorManager>().key = playerLeftKey;
            player.GetComponent<UiManager>().LHpImage = leftPlayerHpBar;
            distanceObj.leftTarget = player.GetComponent<Character>().target;
        }
        else
        {
            player.SetActive(true);
            player.GetComponent<AnimatorManager>().key = playerRightKey;
            player.GetComponent<UiManager>().RHpImage = leftPlayerHpBar;
            distanceObj.rightTarget = player.GetComponent<Character>().target;
        }
    
    }


    //public void PlayerRightPosition()
    //{
    //    mPlayerCopy = Instantiate(mPlayerPrefab, new Vector3(15, (float)0.1, 7), Quaternion.Euler(new Vector3(0, 90, 0)));//  new Quaternion(0, 90, 0));
    //    mPlayerCopy.SetActive(true);
    //    mPlayerCopy.GetComponent<AnimatorManager>().key = playerLeftKey;
    //    //mPlayerCopy.GetComponent<Character>().HpImage = leftPlayerHpBar;
    //    distanceObj.leftTarget = mPlayerCopy.GetComponent<Character>().target;


    //    WPlayerCopy = Instantiate(wPlayerPrefab, new Vector3(20, (float)0.1, 7), Quaternion.Euler(new Vector3(0, -90, 0))); // new Quaternion(0, -90, 0));
    //    WPlayerCopy.SetActive(true);
    //    WPlayerCopy.GetComponent<AnimatorManager>().key = playerRightKey;
    //   // WPlayerCopy.GetComponent<Character>().HpImage = rightPlayerHpBar;
    //    distanceObj.rightTarget = WPlayerCopy.GetComponent<Character>().target;

    //    WPlayerCopy.GetComponent<Character>().targetCharacter = mPlayerCopy.GetComponent<Character>();
    //    mPlayerCopy.GetComponent<Character>().targetCharacter = WPlayerCopy.GetComponent<Character>();


    //}


    public void HpImageBar() // 이미지 두개! // 수정해야함 -> 이미지가 두개로 늘었기 때문에 손을 봐야함[매개변수를 2개 사용하던가 말던가]
    {
        isMove = false;
        //HpImage.fillAmount = (float)imageHp / (float)maxHp;
    }


    private void Update()
    {
        TimeStop();
    }


    Coroutine coroutine;

    void TimeStop()
    {
        if (WPlayerCopy.GetComponent<Character>().Hp <= 0)
        {
            StopCoroutine(coroutine);
        }
        else if (mPlayerCopy.GetComponent<Character>().Hp <= 0)
        {
            StopCoroutine(coroutine);
        }
    }


    IEnumerator PlayTimeCo(float maxPlayTime)
    {
        while (maxPlayTime > 0)
        {
            maxPlayTime -= Time.deltaTime;
            playTimeText.text = " 시간 : " + (int)maxPlayTime;
            yield return null;
        }
        maxPlayTime = 0;
        playState = false;
        playTimeText.text = " 시간 : " + (int)maxPlayTime;
        GameOverJugement();

        yield return null;
    }

    public void GameOverJugement()
    {
       
        if (WPlayerCopy.GetComponent<Character>().Hp >= mPlayerCopy.GetComponent<Character>().Hp)
        {
            WPlayerCopy.GetComponent<Character>().Win();
            mPlayerCopy.GetComponent<Character>().Lose();
        }
        if (WPlayerCopy.GetComponent<Character>().Hp <= mPlayerCopy.GetComponent<Character>().Hp)
        {
            WPlayerCopy.GetComponent<Character>().Lose();
            mPlayerCopy.GetComponent<Character>().Win();
        }
        if (WPlayerCopy.GetComponent<Character>().Hp == mPlayerCopy.GetComponent<Character>().Hp)
        {
            WPlayerCopy.GetComponent<Character>().Win();
            mPlayerCopy.GetComponent<Character>().Win();
        }


    }

}