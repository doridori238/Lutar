using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;



public class GameManager : Singleton<GameManager>
{
   
    private float maxPlayTime = 100;
    public bool isMove = false;

  
    public TextMeshProUGUI playTimeText;
    private bool playState;

    [SerializeField] private PlayersDistance distanceObj;
    [SerializeField] private GameObject mPlayerPrefab;
    [SerializeField] private GameObject wPlayerPrefab;
    public GameObject mPlayerCopy;
    public GameObject wPlayerCopy;



    private Image leftPlayerHpBar;
    private Image rightPlayerHpBar;

    private int LmaxHp;
    private int LimageHp;

    private int RmaxHp;
    private int RimageHp;


    PlayerKey playerLeftKey = new PlayerKey();
    PlayerKey playerRightKey = new PlayerKey();


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


        PlayerSet(wPlayerPrefab);
        PlayerSet(mPlayerPrefab);


        coroutine = StartCoroutine(PlayTimeCo(maxPlayTime));

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


    public void PlayerSet(GameObject playerPrefab)
    {
        //if (playerPrefab.GetComponent<Character>().targetCharacter == null)
        //    return;
        if (DataSaver.instance.selectIndex == 1)
        {
            mPlayerCopy = Instantiate(playerPrefab, new Vector3(15, 0.1f, 7), Quaternion.Euler(new Vector3(0, 90, 0)));
            SetPlayerInfo(mPlayerCopy, 0, playerLeftKey);
            DataSaver.instance.selectIndex = 0;
        }
        else if (DataSaver.instance.selectIndex != 1)
        {
            wPlayerCopy = Instantiate(playerPrefab, new Vector3(20, 0.1f, 7), Quaternion.Euler(new Vector3(0, -90, 0)));
            SetPlayerInfo(wPlayerCopy, 1, playerRightKey);
            DataSaver.instance.selectIndex = 1;
        }
    }

    void SetPlayerInfo(GameObject playerObj, int index, PlayerKey key)
    {
        playerObj.GetComponent<AnimatorManager>().key = key;
        PlayState = true;
        Character character = playerObj.GetComponent<Character>();
        DataSaver.instance.selectIndex = index;
        if (index == 0)
        {
            character.leftPlayer = true;
            leftPlayerHpBar = UiManager.instance.LHpImage;
            UiManager.instance.LHpImage.fillAmount = (float)character.Hp / (float)character.MaxHp;
            distanceObj.leftTarget = character.target;
        }
        else
        {
            character.leftPlayer = false;
            rightPlayerHpBar = UiManager.instance.RHpImage;
            UiManager.instance.RHpImage.fillAmount = (float)character.Hp / (float)character.MaxHp;
            distanceObj.rightTarget = character.target;
        }
    }


    private void Update()
    {
        TimeStop();
    }


    Coroutine coroutine;

    void TimeStop()
    {
        if (wPlayerCopy.GetComponent<Character>().Hp <= 0)
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
        PlayState = false;
        playTimeText.text = " 시간 : " + (int)maxPlayTime;
        GameOverJugement();

        yield return null;
    }

    public void GameOverJugement()
    {

        if (wPlayerCopy.GetComponent<Character>().Hp >= mPlayerCopy.GetComponent<Character>().Hp)
        {
            wPlayerCopy.GetComponent<Character>().Win();
            mPlayerCopy.GetComponent<Character>().Lose();
        }
        if (wPlayerCopy.GetComponent<Character>().Hp <= mPlayerCopy.GetComponent<Character>().Hp)
        {
            wPlayerCopy.GetComponent<Character>().Lose();
            mPlayerCopy.GetComponent<Character>().Win();
        }
        if (wPlayerCopy.GetComponent<Character>().Hp == mPlayerCopy.GetComponent<Character>().Hp)
        {
            wPlayerCopy.GetComponent<Character>().Win();
            mPlayerCopy.GetComponent<Character>().Win();
        }


    }

}