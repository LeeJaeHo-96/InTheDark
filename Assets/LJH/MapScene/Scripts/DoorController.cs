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

    float gas = 100f;
    Coroutine gasCo;

    Door doorScriptL;
    Door doorScriptR;
    public Coroutine DoorOpenCoL;
    public Coroutine DoorOpenCoR;

    public Coroutine DoorCloseCoL;
    public Coroutine DoorCloseCoR;

    public bool doorOpend = true;

    //문 제어권 여부
    bool canDoorControll = true;
    private void Start()
    {
        doorScriptL = leftDoor.GetComponent<Door>();
        doorScriptR = rightDoor.GetComponent<Door>();

        StartCoroutine(GasCheck());


    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            DoorOpen();
        }
    }

    void DoorOpen()
    {
        if (canDoorControll && Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("RPCDoorOpenAndClose", RpcTarget.AllViaServer, photonView.ViewID);
        }
    }

    /// <summary>
    /// 문 개방되는 함수
    /// </summary>
    [PunRPC]
    void RPCDoorOpenAndClose(int playerID)
    {
        // E버튼 누를때마다 가스 코루틴 초기화 시킴
        if (gasCo != null)
            StopCoroutine(gasCo);
        gasCo = null;

        // 문 닫혀있는 상태일 때 E누르면 문열림
        if (!doorOpend)
        {
            // 문닫기를 멈추고 열기 실행
            if (DoorCloseCoL != null)
            {
                StopCoroutine(DoorCloseCoL);
                StopCoroutine(DoorCloseCoR);

                DoorCloseCoL = null;
                DoorCloseCoR = null;
            }
            doorOpend = true;
            DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
            DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
        }
        // 문 열려있는 상태일 때 E누르면 문닫힘
        else if (doorOpend)
        {
            if (DoorOpenCoL != null)
            {
                // 문열기를 멈추고 닫기 실행
                StopCoroutine(DoorOpenCoL);
                StopCoroutine(DoorOpenCoR);

                DoorOpenCoL = null;
                DoorOpenCoR = null;
            }
            doorOpend = false;
            DoorCloseCoL = StartCoroutine(doorScriptL.DoorCloseCoroutine());
            DoorCloseCoR = StartCoroutine(doorScriptR.DoorCloseCoroutine());
        }
    }



    /// <summary>
    /// 문 열림 상태에 따라 가스를 충전, 감소 시키는 함수
    /// </summary>
    IEnumerator GasCheck()
    {
        while (true)
        {
            if (gas >= 100)
            {
                if (gasCo != null)
                    StopCoroutine(gasCo);

                gasCo = null;
                canDoorControll = true;
            }
            //가스 0되면 강제로 문 개방됨
            if (gas <= 0)
            {
                if (DoorOpenCoL == null)
                    DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                if (DoorOpenCoR == null)
                    DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());

                //가스가 100이 될 때까지 제어권 상실
                if (canDoorControll)
                {
                    canDoorControll = false;
                    StopCoroutine(gasCo);
                    gasCo = null;
                    if (gasCo == null)
                    {
                        gasCo = StartCoroutine(GasZeroRechargeCoroutine());
                    }
                }
            }

            //문 개방 여부에 따라 코루틴 변경
            if (gasCo == null)
            {
                switch (doorOpend)
                {
                    case false:
                        Debug.Log("가스감소발동");
                        gasCo = StartCoroutine(DoorGasDeCoroutine());
                        break;

                    case true:
                        gasCo = StartCoroutine(DoorGasCoroutine());
                        break;
                }
            }

            yield return null;
        }
    }

    IEnumerator GasZeroRechargeCoroutine()
    {
        yield return new WaitForSeconds(2f);

        doorOpend = true;
        gasCo = StartCoroutine(DoorGasCoroutine());
    }

    /// <summary>
    /// 문 닫혀있을 때, 가스 충전되는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasCoroutine()
    {
        while (gas < 100f)
        {
            Debug.Log("가스증가");
            gas += 25f * Time.deltaTime;

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
            Debug.Log("가스감소");
            gas -= 25f * Time.deltaTime;

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
