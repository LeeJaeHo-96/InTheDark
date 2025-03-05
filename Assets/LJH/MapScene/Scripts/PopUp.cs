using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject player;
    public Collider playerCol;

    [SerializeField] GameObject keyInfo;

    public bool hitMe;

    private void Update()
    {
        KeyInfoOnOff(hitMe);
    }

    void KeyInfoOnOff(bool tf)
    {
        keyInfo.SetActive(tf);
    }
}
