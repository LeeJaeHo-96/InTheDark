using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    GameObject player;
    Collider playerCol;

    [SerializeField] GameObject keyInfo;

    private void Start()
    {
        //Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        KeyInfoOnOff(other);
    }

    private void OnTriggerExit(Collider other)
    {
        KeyInfoOnOff(other);
    }

    void KeyInfoOnOff(Collider other)
    {
        if (other == playerCol)
        {
                Debug.Log("�����");
                keyInfo.SetActive(!keyInfo.activeSelf);
        }
    }


    void Init()
    {
        player = GameObject.FindWithTag(Tag.Player); 
        playerCol = player.GetComponent<Collider>();

    }
}
