using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class DataSaver : Singleton<DataSaver>
{
    public int selectIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
 
    public void SetIndex(int value)
    {
        selectIndex = value;
    }
}

