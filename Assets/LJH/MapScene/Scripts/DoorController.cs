using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviourPun, IPunObservable
{
    //플레이어
    GameObject player;
    Collider playerColl;

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

    public bool doorOpened = true;

    //문 제어권 변수
    [SerializeField] bool canDoorControll = true;
    DoorPopUp popUp;

    private void Start()
    {
        Init();
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
        if (canDoorControll && Input.GetKeyDown(KeyCode.E) && popUp.hitMe)
        {
            Debug.Log("E버튼 눌림");
            photonView.RPC("RPCDoorOpenAndClose", RpcTarget.AllViaServer, photonView.ViewID);
        }
    }

    /// <summary>
    /// 문 열고 닫는 RPC 함수
    /// </summary>
    [PunRPC]
    void RPCDoorOpenAndClose(int playerID)
    {
        // gasCo 초기화
        if (gasCo != null)
            StopCoroutine(gasCo);
        gasCo = null;


        // 문이 닫혀있을 때 열어주는 기능
        if (!doorOpened)
        {
            // 코루틴 변수 비워주고 다시 초기화
            if (DoorCloseCoL != null)
            {
                StopCoroutine(DoorCloseCoL);
                StopCoroutine(DoorCloseCoR);

                DoorCloseCoL = null;
                DoorCloseCoR = null;
            }
            doorOpened = true;
            DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
            DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
        }
        // 문이 열려있을 때 닫아주는 기능
        else if (doorOpened)
        {
            if (DoorOpenCoL != null)
            {
                // 코루틴 변수 비워주고 다시 초기화
                StopCoroutine(DoorOpenCoL);
                StopCoroutine(DoorOpenCoR);

                DoorOpenCoL = null;
                DoorOpenCoR = null;
            }
            doorOpened = false;
            DoorCloseCoL = StartCoroutine(doorScriptL.DoorCloseCoroutine());
            DoorCloseCoR = StartCoroutine(doorScriptR.DoorCloseCoroutine());
        }
    }



    /// <summary>
    /// 가스양에 체크해주는 코루틴
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
            // 가스가 0일 때, 자동으로 열리는
            if (gas <= 0)
            {
                if (DoorOpenCoL == null)
                    DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                if (DoorOpenCoR == null)
                    DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());

                // 가스 0이되면 일정 시간 지나고 나서 충전 가능
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

            //문 개방 여부에 따라 가스 충전/감소 시켜주는
            if (gasCo == null)
            {
                switch (doorOpened)
                {
                    case false:
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

        doorOpened = true;
        gasCo = StartCoroutine(DoorGasCoroutine());
    }

    /// <summary>
    /// 가스 충전해주는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasCoroutine()
    {
        while (gas < 100f)
        {
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
    /// 가스 감소해주는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasDeCoroutine()
    {
        while (gas > 0)
        {
            gas -= 25f * Time.deltaTime;

            if (gas <= 0)
            {
                gas = 0f;
            }
            gasText.text = $"{gas.ToString("F2")}%";

            yield return null;
        }
    }

    void Init()
    {
        doorScriptL = leftDoor.GetComponent<Door>();
        doorScriptR = rightDoor.GetComponent<Door>();

        StartCoroutine(GasCheck());

        popUp = GetComponent<DoorPopUp>();
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
