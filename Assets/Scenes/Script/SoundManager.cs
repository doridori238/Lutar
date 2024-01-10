using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public GameObject soundComponentPrefab;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);    
    }

    public void Play(AudioClip clip)
    {
       GameObject soundPrefab = Instantiate(soundComponentPrefab);
        soundPrefab.GetComponent<SoundComponent>().Play(clip);
    }

}


