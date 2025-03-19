using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotAirBalloon : MonoBehaviour
{
    Rigidbody rigid;
    Vector3 destination;
    List<Item> itemList = new List<Item>();

    GameObject basket;

    float speed = 5;
    float boomDelayTime;

    private void Start()
    {
        destination = transform.position + new Vector3(0, 0, 50);
    }
    private void FixedUpdate()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);

        if(Vector3.Distance(transform.position, destination) <= 7f)
        {
            DropBasket();
        }    
    }

    void DropBasket()
    {
        if (transform.GetChild(0).name == "Basket")
        {
            basket = transform.GetChild(0).gameObject;
            basket.transform.parent = null;
            basket.AddComponent<Rigidbody>();
        }

    }
}
