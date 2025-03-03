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
        photonView.RPC("RPCDoorOpenAndClose", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// 문 개방되는 함수
    /// </summary>
    [PunRPC]
    void RPCDoorOpenAndClose()
    {
        //가스 0되면 강제로 문 개방됨
        if (gas <= 0)
        {
            if (DoorOpenCoL == null)
                DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
            if (DoorOpenCoR == null)
                DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // E버튼 누를때마다 가스 코루틴 초기화 시킴
            if(gasCo != null)
                StopCoroutine(gasCo);
            gasCo = null;

            // 문 닫혀있는 상태일 때 E누르면 문열림
            if (!doorOpend)
            {
                Debug.Log("문 열림");
                doorOpend = true;
                DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
            }
            // 문 열려있는 상태일 때 E누르면 문닫힘
            else if (doorOpend)
            {
                Debug.Log("문 닫힘");
                doorOpend = false;
                DoorCloseCoL = StartCoroutine(doorScriptL.DoorCloseCoroutine());
                DoorCloseCoR = StartCoroutine(doorScriptR.DoorCloseCoroutine());

            }
        }
    }



    /// <summary>
    /// 문 열림 상태에 따라 가스를 충전, 감소 시키는 함수
    /// </summary>
    void GasCheck()
    {
        if (gasCo == null)
        {
            switch (doorOpend)
            {
                case false:
                    gasCo = StartCoroutine(DoorGasDeCoroutine());
                    break;

                case true:
                    gasCo = StartCoroutine(DoorGasCoroutine());
                    break;
            }
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
