using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour, IHitMe
{
    [SerializeField] GameObject keyInfo;

    public bool HitMe { get; set; }


    private void Update()
    {
        KeyInfoOnOff(HitMe);
    }

    void KeyInfoOnOff(bool tf)
    {
        keyInfo.SetActive(tf);
    }
}
