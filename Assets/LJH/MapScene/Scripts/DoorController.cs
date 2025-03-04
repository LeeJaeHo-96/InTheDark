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

    //�� ����� ����
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
    /// �� ����Ǵ� �Լ�
    /// </summary>
    [PunRPC]
    void RPCDoorOpenAndClose(int playerID)
    {
        // E��ư ���������� ���� �ڷ�ƾ �ʱ�ȭ ��Ŵ
        if (gasCo != null)
            StopCoroutine(gasCo);
        gasCo = null;


        // �� �����ִ� ������ �� E������ ������
        if (!doorOpend)
        {
            // ���ݱ⸦ ���߰� ���� ����
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
        // �� �����ִ� ������ �� E������ ������
        else if (doorOpend)
        {
            if (DoorOpenCoL != null)
            {
                // �����⸦ ���߰� �ݱ� ����
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
    /// �� ���� ���¿� ���� ������ ����, ���� ��Ű�� �Լ�
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
            //���� 0�Ǹ� ������ �� �����
            if (gas <= 0)
            {
                if (DoorOpenCoL == null)
                    DoorOpenCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                if (DoorOpenCoR == null)
                    DoorOpenCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());

                //������ 100�� �� ������ ����� ���
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

            //�� ���� ���ο� ���� �ڷ�ƾ ����
            if (gasCo == null)
            {
                switch (doorOpend)
                {
                    case false:
                        Debug.Log("�������ҹߵ�");
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
    /// �� �������� ��, ���� �����Ǵ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasCoroutine()
    {
        while (gas < 100f)
        {
            Debug.Log("��������");
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
    /// �� �������� �� ���� ���ҵǴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator DoorGasDeCoroutine()
    {
        while (gas > 0)
        {
            Debug.Log("��������");
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
