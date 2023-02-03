using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    string str="1_c";
    int x=12;
    void Start()
    {
       Checking(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Checking()
    {   
        
        Debug.Log(x.ToString().Length);
        bool res=str.Contains(Convert.ToChar(x));
        Debug.Log(res);
    }
}
