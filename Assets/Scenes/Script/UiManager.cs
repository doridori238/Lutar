using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;

public class UiManager : Singleton<UiManager>
{
    public bool state;

    public GameObject choiceimage;
    public GameObject explanation;
    public GameObject setImage;
    public GameObject main;

    //public bool isMove = false;
    public Image LHpImage;
    public Image RHpImage;
    //private int maxHp;
    //private int imageHp;

    private void Start()
    {
        state = false;
        //imageHp = GetComponent<Character>().Hp;
        //maxHp = GetComponent<Character>().MaxHp;

        //HpImageBar();
    }

    public void OnClickAudioButton(string name)
    {
        state = !state;
        SetImage(name);
     
    }

    public void SetImage(string name)
    {       
        setImage.SetActive(state);
    }

    //public void HpImageBar() // 이미지 두개! 
    //{
    //    isMove = false;
    //    HpImage.fillAmount = (float)imageHp / (float)maxHp;
    //}
}

  


