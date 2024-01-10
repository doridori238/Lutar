using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static DataSaver instance = null;
    public int selectIndex = 0;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetIndex(int value)
    {
        selectIndex = value;
    }
}
