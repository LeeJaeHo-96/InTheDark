using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [SerializeField] GameObject computerCanvas;
    PopUp popUp;

    private void Start()
    {
        popUp = GetComponent<PopUp>();
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
