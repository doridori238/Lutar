using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private GameObject mPlayerCopy;
    private GameObject wPlayerCopy;

    //private GameObject playerCopy;

    public Image leftPlayerHpBar;
    public Image rightPlayerHpBar;

    private int LmaxHp;
    private int LimageHp;

    private int RmaxHp;
    private int RimageHp;


    [SerializeField] PlayerKey playerLeftKey = new PlayerKey();
    [SerializeField] PlayerKey playerRightKey = new PlayerKey();


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

        PlayerSet(mPlayerPrefab);
        PlayerSet(wPlayerPrefab);

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
        if (DataSaver.instance.selectIndex == 1)
        {
            mPlayerCopy = Instantiate(playerPrefab, new Vector3(15, (float)0.1, 7), Quaternion.Euler(new Vector3(0, 90, 0)));
            mPlayerCopy.SetActive(true);
            mPlayerCopy.GetComponent<AnimatorManager>().key = playerLeftKey;
            LmaxHp = mPlayerCopy.GetComponent<Character>().MaxHp;
            LimageHp = mPlayerCopy.GetComponent<Character>().Hp;
            UiManager.instance.LHpImage.fillAmount = (float)LimageHp / (float)LmaxHp;
            UiManager.instance.LHpImage = leftPlayerHpBar;
            distanceObj.leftTarget = mPlayerCopy.GetComponent<Character>().target;
            DataSaver.instance.selectIndex = 0;
        }
        else if (DataSaver.instance.selectIndex != 1)
        {
            wPlayerCopy = Instantiate(playerPrefab, new Vector3(20, (float)0.1, 7), Quaternion.Euler(new Vector3(0, -90, 0)));
            wPlayerCopy.SetActive(true);
            wPlayerCopy.GetComponent<AnimatorManager>().key = playerRightKey;
            RmaxHp = wPlayerCopy.GetComponent<Character>().MaxHp;
            RimageHp = wPlayerCopy.GetComponent<Character>().Hp;
            UiManager.instance.RHpImage.fillAmount = (float)RimageHp / (float)RmaxHp;
            UiManager.instance.RHpImage = rightPlayerHpBar;
            distanceObj.rightTarget = wPlayerCopy.GetComponent<Character>().target;
            DataSaver.instance.selectIndex = 1;
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
        playState = false;
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