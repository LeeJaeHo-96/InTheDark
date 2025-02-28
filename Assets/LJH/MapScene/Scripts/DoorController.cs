using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] TMP_Text gasText;

    float gas = 0f;
    Coroutine gasCo;

    Door doorScriptL;
    Door doorScriptR;
    public Coroutine DoorCoL;
    public Coroutine DoorCoR;

    public bool doorOpend = true;
    private void Start()
    {
        doorScriptL = leftDoor.GetComponent<Door>();
        doorScriptR = rightDoor.GetComponent<Door>();

    }

    private void Update()
    {
        DoorOpen();
        GasCheck();
    }

    void DoorOpen()
    {
        photonView.RPC("RPCDoorOpen", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// 문 개방되는 함수
    /// </summary>
    [PunRPC]
    void RPCDoorOpen()
    {
        //가스 0되면 강제로 문 개방됨
        if (gas <= 0)
        {
            DoorCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
            DoorCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (doorOpend)
            {
                doorOpend = false;
                DoorCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                DoorCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
            }
        }
    }


    /// <summary>
    /// 가스량이 100 미만이라면 충전해주는 함수
    /// </summary>
    void GasCheck()
    {
        if (gasCo == null && gas < 100f)
        {
            gasCo = StartCoroutine(DoorGasCoroutine());
        }

        else if(gasCo != null && gas >= 100f)
        {
            StopCoroutine(gasCo);
            gasCo = null;
        }
    }

    /// <summary>
    /// 문 닫혀있을 때, 가스 충전되는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasCoroutine()
    {
        while (gas < 100f)
        {
            gas += 10f * Time.deltaTime;

            if (gas >= 100f)
            {
                gas = 100f;
            }
            gasText.text = $"{gas.ToString("F2")}%";

            yield return null;
        }
    }

    /// <summary>
    /// 문 열려있을 때 가스 감소되는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasDeCoroutine()
    {
        while (gas > 0)
        {
            gas -= 10f * Time.deltaTime;

            if (gas <= 0)
            {
                gas = 0f;
            }
            gasText.text = $"{gas.ToString("F2")}%";

            yield return null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gas);
        }
        else
        {
            gas = (float)stream.ReceiveNext();
        }
    }
}
