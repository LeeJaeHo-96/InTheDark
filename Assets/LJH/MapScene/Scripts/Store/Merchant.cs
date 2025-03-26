using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    int totalPrice;

    List<GameObject> sellItemList = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            totalPrice += other.GetComponent<Item>().GetSellPrice();
            sellItemList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            totalPrice -= other.GetComponent<Item>().GetSellPrice();
            sellItemList.Remove(other.gameObject);
        }
    }

    public void SellItem()
    {
        for (int i = 0; i < sellItemList.Count; i++)
        {
            Destroy(sellItemList[i]);
            IngameManager.Instance.money += totalPrice;
        } 
    }
}
