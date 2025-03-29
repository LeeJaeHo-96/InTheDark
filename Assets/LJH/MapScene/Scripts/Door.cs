using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviourPun
{
    //사운드
    SoundManager _soundManager => SoundManager.Instance;
    
    [SerializeField] DoorController doorCon;
    
    [SerializeField] GameObject door;
    [SerializeField] GameObject oppoDoor;
    [SerializeField] GameObject pill;
    [SerializeField] GameObject doorStopper;

    Vector3 doorPos;
    Vector3 oppoDoorPos;
    Vector3 pillPos;
    Vector3 doorStopperPos;


    Collider leftColl;
    Collider rightColl;

    //문 열닫 속도용 변수
    float doorSpeed = 1f;

    private void Start()
    {
        doorPos = door.transform.position;
        oppoDoorPos = oppoDoor.transform.position;
        pillPos = pill.transform.position;
        doorStopperPos = doorStopper.transform.position;

    }

    [PunRPC]
    public void RPCDoorOpen()
    {
        photonView.RPC("DoorOpenCoroutine", RpcTarget.AllViaServer);
    }
    /// <summary>
    /// 문 열릴때
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoorOpenCoroutine()
    {
        SoundManager.Instance.PlaySfx(_soundManager.SoundDatas.SoundDict["OpenDoor"]);
        while (Vector3.Distance(transform.position, pillPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pillPos, doorSpeed * Time.deltaTime);

            yield return null;
        }
    }

    [PunRPC]
    public void RPCDoorClose()
    {
        photonView.RPC("DoorCloseCoroutine", RpcTarget.AllViaServer);
    }
    /// <summary>
    /// 문 닫을때
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoorCloseCoroutine()
    {
        SoundManager.Instance.PlaySfx(_soundManager.SoundDatas.SoundDict["CloseDoor"]);
        while (Vector3.Distance(transform.position, doorStopperPos) > 0.6f)
        {
            transform.position = Vector3.MoveTowards(transform.position, doorStopperPos, doorSpeed * Time.deltaTime);

            yield return null;
        }
    }

}
