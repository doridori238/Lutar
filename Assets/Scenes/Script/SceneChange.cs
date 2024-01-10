using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
   
    public void OnClickStartButton(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

        public void Gamequit()
    {
        Application.Quit();
    }

}
