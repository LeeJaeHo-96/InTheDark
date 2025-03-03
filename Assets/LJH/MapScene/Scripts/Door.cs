using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviourPun
{
    [SerializeField] DoorController doorCon;
    [SerializeField] GameObject leftPill;
    [SerializeField] GameObject rightPill;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    Collider leftColl;
    Collider rightColl;

    //문 열닫 속도용 변수
    Vector3 doorSpeed = new Vector3(0, 0, 0.1f);

    private void Start()
    {

        leftColl = leftPill.GetComponent<Collider>();
        rightColl = rightPill.GetComponent<Collider>();

    }

    private void Update()
    {
        if (doorCon.DoorOpenCoL != null)
            Debug.Log(doorCon.DoorOpenCoL);
        else
            Debug.Log("코루틴비어있음");
    }

    [PunRPC]
    public void RPCDoorOpen()
    {
        photonView.RPC("DoorOpenCoroutine", RpcTarget.AllBuffered);
    }
    /// <summary>
    /// 문 열릴때
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoorOpenCoroutine()
    {
        while (true)
        {
            if(gameObject.name == "LeftDoor")
                gameObject.transform.position += doorSpeed;
            if(gameObject.name == "RightDoor")
                gameObject.transform.position -= doorSpeed;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    [PunRPC]
    public void RPCDoorClose()
    {
        photonView.RPC("DoorCloseCoroutine", RpcTarget.AllBuffered);
    }
    /// <summary>
    /// 문 닫을때
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoorCloseCoroutine()
    {
        while (true)
        {
            if (gameObject.name == "LeftDoor")
                gameObject.transform.position -= doorSpeed;
            if (gameObject.name == "RightDoor")
                gameObject.transform.position += doorSpeed;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PhotonNetwork.IsMasterClient) return; // 마스터 클라이언트에서만 충돌 처리

        if (gameObject.name == "LeftDoor")
        {
            if (collision.collider.CompareTag(Tag.Pill))
            {
                Debug.Log("열리면서 충돌 발생 (왼쪽 문)");
                if (doorCon.DoorOpenCoL != null)
                    doorCon.StopCoroutine(doorCon.DoorOpenCoL);
                doorCon.DoorOpenCoL = null;

                photonView.RPC("RPCDoorCollision", RpcTarget.All, "LeftDoor", "open");
            }

            if (collision.collider.CompareTag(Tag.ShipDoor))
            {
                Debug.Log("닫히면서 충돌 발생 (왼쪽 문)");
                if (doorCon.DoorCloseCoL != null)
                    doorCon.StopCoroutine(doorCon.DoorCloseCoL);
                doorCon.DoorOpenCoL = null;

                photonView.RPC("RPCDoorCollision", RpcTarget.All, "LeftDoor", "close");
            }
        }
        else if (gameObject.name == "RightDoor")
        {
            if (collision.collider.CompareTag(Tag.Pill))
            {
                Debug.Log("열리면서 충돌 발생 (오른쪽 문)");
                if (doorCon.DoorOpenCoR != null)
                    doorCon.StopCoroutine(doorCon.DoorOpenCoR);
                doorCon.DoorOpenCoR = null;

                photonView.RPC("RPCDoorCollision", RpcTarget.All, "RightDoor", "open");
            }

            if (collision.collider.CompareTag(Tag.ShipDoor))
            {
                Debug.Log("닫히면서 충돌 발생 (오른쪽 문)");
                if (doorCon.DoorCloseCoR != null)
                    doorCon.StopCoroutine(doorCon.DoorCloseCoR);
                doorCon.DoorOpenCoL = null;

                photonView.RPC("RPCDoorCollision", RpcTarget.All, "RightDoor", "close");
            }
        }
    }

    [PunRPC]
    void RPCDoorCollision(string doorName, string action)
    {
        Debug.Log($"{doorName} 문이 {action} 도중 충돌함");

        if (doorName == "LeftDoor")
        {
            if (action == "open" && doorCon.DoorOpenCoL != null)
                doorCon.StopCoroutine(doorCon.DoorOpenCoL);
            else if (action == "close" && doorCon.DoorCloseCoL != null)
                doorCon.StopCoroutine(doorCon.DoorCloseCoL);
            doorCon.DoorOpenCoL = null;
        }
        else if (doorName == "RightDoor")
        {
            if (action == "open" && doorCon.DoorOpenCoR != null)
                doorCon.StopCoroutine(doorCon.DoorOpenCoR);
            else if (action == "close" && doorCon.DoorCloseCoR != null)
                doorCon.StopCoroutine(doorCon.DoorCloseCoR);
            doorCon.DoorOpenCoR = null;
        }
    }
}
