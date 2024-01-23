using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System.IO;
using JetBrains.Annotations;
using Unity.VisualScripting;

[Serializable]
public class PlayerData
{
    public int forWardKey;
    public int backWardKey;
    public int highKickKey;
    public int middleKickKey;
    public int lowKickKey;
    public int parryingKey;
    public int jabKey;
    public int punchKey;

    public PlayerData(int[] playerkey)
    {
        backWardKey = playerkey[0];
        highKickKey = playerkey[1];
        middleKickKey = playerkey[2];
        lowKickKey = playerkey[3];
        parryingKey = playerkey[4];
        jabKey = playerkey[5];
        punchKey = playerkey[6];
        forWardKey = playerkey[7];

    }

}



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

    Character wPlayerCharacter;
    Character mPlayerCharacter;

    PlayerKey playerLeftKey;
    PlayerKey playerRightKey;


    string path = @"Assets\Resources\";
    string leftfilename = "LeftPlayerData";
    string rightfilename = "RightPlayerDate";

    public bool PlayState
    {
        get { return playState; }
        set
        {
            playState = value;
        }
    }


    public void SaveDate(string filename, PlayerData data)
    {
        StreamWriter writer;

        if (File.Exists(path + filename + ".txt") == false)
        {
            writer = File.CreateText(path + filename + ".txt");
            writer.Write(JsonUtility.ToJson(data));
        }
        else
        {
            writer = new StreamWriter(path + filename + ".txt");
            writer.Write(JsonUtility.ToJson(data));
        }
        writer.Close();
       
    }

    
    public PlayerData LoadData(string fileName)
    {
       string loadfile = Resources.Load(fileName).ToString();
        if (loadfile != null)
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadfile);
          
            return playerData;
        }
        return null;

    }



    void Start()
    {

        SaveDate(leftfilename,new PlayerData(new int[]
        { (int)KeyCode.A, (int)KeyCode.W,(int)KeyCode.K,(int)KeyCode.S,(int)KeyCode.I,(int)KeyCode.J,(int)KeyCode.L,(int)KeyCode.D }));
        SaveDate(rightfilename, new PlayerData(new int[]
        { (int)KeyCode.RightArrow,(int)KeyCode.UpArrow,(int)KeyCode.Keypad2,(int)KeyCode.DownArrow,(int)KeyCode.Keypad5,(int)KeyCode.Keypad1,(int)KeyCode.Keypad3,(int)KeyCode.LeftArrow }));


        Debug.Log(LoadData(leftfilename));
        playerLeftKey = new PlayerKey(LoadData(leftfilename));
        playerRightKey = new PlayerKey(LoadData(rightfilename));


        PlayerSet(wPlayerPrefab);
        PlayerSet(mPlayerPrefab);

        wPlayerCharacter = wPlayerCopy.GetComponent<Character>();
        mPlayerCharacter = mPlayerCopy.GetComponent<Character>();

        coroutine = StartCoroutine(PlayTimeCo(maxPlayTime));

    }

  

    public void PlayerSet(GameObject playerPrefab)
    {
        
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

        if (wPlayerCharacter.Hp <= 0)
        {
            StopCoroutine(coroutine);
        }
        else if (mPlayerCharacter.Hp <= 0)
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