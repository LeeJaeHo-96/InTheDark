using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SellButton : MonoBehaviour, IHitMe
{
    public bool HitMe { get; set; }

    [SerializeField] Merchant merchant;
    PopUp popUp;

    private void Start()
    {
        popUp = GetComponent<PopUp>();
    }

    void Update()
    {
        if(popUp.HitMe && Input.GetKeyDown(KeyCode.E))
        {
            merchant.SellItem();
        }
    } 
}
