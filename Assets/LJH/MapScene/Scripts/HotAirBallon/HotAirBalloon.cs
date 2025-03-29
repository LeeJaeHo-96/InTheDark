using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotAirBalloon : MonoBehaviourPun
{
    Rigidbody rigid;

    [HideInInspector] public GameObject spawnPoint;
    Vector3 spawnPos;
    [HideInInspector] public GameObject destinationPoint;
    Vector3 destinationPos;

    List<Item> itemList = new List<Item>();

    GameObject basket;

    float speed = 15f;
    float boomDelayTime = 60f;

    public Coroutine boomCo;
    // 스폰 위치 -110
    // 스탑 위치 220

    private void Start()
    {
        spawnPos = spawnPoint.transform.position;
        destinationPos = destinationPoint.transform.position;
        
    }
    private void FixedUpdate()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, destinationPos, Time.deltaTime * speed);
        float distance = Vector3.Distance(spawnPos, destinationPos) / 2;

        if(Vector3.Distance(transform.position, destinationPos) <= distance)
        {
            DropBasket();
        }    
    }


    void DropBasket()
    {
        if (basket == null)
        {
            //열기구에서 분리하고 리지드바디 추가하여 자연스럽게 떨어지게 함
            basket = transform.GetChild(0).gameObject;
            basket.transform.parent = null;
            basket.AddComponent<Rigidbody>();

            boomCo = StartCoroutine(BoomAirBalloon(boomDelayTime));
        }
    }

    public IEnumerator BoomAirBalloon(float boomDelayTime)
    {
        yield return new WaitForSeconds(boomDelayTime);
        basket.transform.parent = gameObject.transform;
        Destroy(gameObject);
        //Todo : 폭발 애니메이션 추가해야함
    }
}
