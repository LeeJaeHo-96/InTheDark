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
        //DoorOpen();
        //GasCheck();
    }

    void DoorOpen()
    {
        photonView.RPC("RPCDoorOpenAndClose", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// �� ����Ǵ� �Լ�
    /// </summary>
    [PunRPC]
    void RPCDoorOpenAndClose()
    {
        //���� 0�Ǹ� ������ �� �����
        if (gas <= 0)
        {
            if (DoorOpenCoL == null)
                DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
            if (DoorOpenCoR == null)
                DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // E��ư ���������� ���� �ڷ�ƾ �ʱ�ȭ ��Ŵ
            if(gasCo != null)
                StopCoroutine(gasCo);
            gasCo = null;

            // �� �����ִ� ������ �� E������ ������
            if (!doorOpend)
            {
                Debug.Log("�� ����");
                doorOpend = true;
                DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
            }
            // �� �����ִ� ������ �� E������ ������
            else if (doorOpend)
            {
                Debug.Log("�� ����");
                doorOpend = false;
                DoorCloseCoL = StartCoroutine(doorScriptL.DoorCloseCoroutine());
                DoorCloseCoR = StartCoroutine(doorScriptR.DoorCloseCoroutine());

            }
        }
    }



    /// <summary>
    /// �� ���� ���¿� ���� ������ ����, ���� ��Ű�� �Լ�
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
    /// �� �������� ��, ���� �����Ǵ� �ڷ�ƾ
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
    /// �� �������� �� ���� ���ҵǴ� �ڷ�ƾ
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
