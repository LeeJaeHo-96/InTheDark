using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLJH : MonoBehaviour
{
    int monitorRefreshRate;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Screen.brightness);
        int monitorRefreshRate = Screen.currentResolution.refreshRate;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
