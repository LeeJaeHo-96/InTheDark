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
            Debug.Log(totalPrice);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            totalPrice -= other.GetComponent<Item>().GetSellPrice();
            sellItemList.Remove(other.gameObject);
            Debug.Log(totalPrice);
        }
    }

    public void SellItem()
    {
        Debug.Log("π∞∞«∆»æ“¿Ω");
        for (int i = 0; i < sellItemList.Count; i++)
        {
            Destroy(sellItemList[i]);

            IngameManager.Instance.money += totalPrice;
        } 
    }
}
