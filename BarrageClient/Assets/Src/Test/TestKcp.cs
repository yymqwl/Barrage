using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;

public class TestKcp : MonoBehaviour
{ 
    void Start()
    {
        //Log.Debug($"{DateTime.Now...}");
        Log.Debug($"{TimeHelper.ClientNowSeconds()}");
    }

  
    void Update()
    {
        
    }
}
