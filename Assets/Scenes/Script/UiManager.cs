using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    public bool state;

    public GameObject choiceimage;
    public GameObject explanation;
    public GameObject setImage;
    public GameObject main;
    
    private void Start()
    {
        state = false;

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


}

  


