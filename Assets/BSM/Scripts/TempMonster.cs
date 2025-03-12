using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TempMonster : MonoBehaviourPun
{
    private int _hp = 300;

    private LayerMask _itemLayer;

    private void Awake()
    {
        _itemLayer = LayerMask.NameToLayer("Item");
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (_itemLayer.value == other.gameObject.layer)
        {
            Item item = other.GetComponent<Item>(); 

            if (item.IsAttacking && item.AttackItem())
            {
                TakeDamage(item.GetItemDamage());  
            } 
        }
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        photonView.RPC(nameof(TakeDamageRPC), RpcTarget.AllBuffered, _hp);
    }

    [PunRPC]
    private void TakeDamageRPC(int damage)
    {
        _hp = damage;
        Debug.Log($"남은 체력 :{_hp}");
    }
    
    
}
