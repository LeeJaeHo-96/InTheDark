using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    GameObject computerCanvas;
    PopUp popUp;

    private void Start()
    {
        popUp = GetComponent<PopUp>();
        computerCanvas = transform.GetChild(0).GetComponent<GameObject>();
    }

    private void Update()
    {
        ComputerOnOff();
    }

    void ComputerOnOff()
    {
        if(popUp.HitMe && Input.GetKeyDown(KeyCode.E))
        {
            computerCanvas.SetActive(!computerCanvas.activeSelf);
        }
    }
}
