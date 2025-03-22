using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewDoor : MonoBehaviourPun, IHitMe
{

    float doorSpeed = 5f;
    private Coroutine doorCo;
    public bool isClosed = true;

    private float openedAngle = 100f;
    private float closedAngle = 180f;

    public bool HitMe { get; set; }

    private void Update()
    {
        if (HitMe && Input.GetKeyDown(KeyCode.E))
        {
            if (doorCo == null)
            {
                photonView.RPC("RPCDoor", RpcTarget.AllViaServer);
            }
        }
    }


    // 유니티액션을 이용한 메서드
    //[PunRPC]
    //void RPChitMeShare()
    //{
    //    indoor.hitMeEvent?.Invoke(indoor.hitMe);
    //}


    [PunRPC]
    void RPCDoor()
    {
        doorCo = StartCoroutine(DoorCoroutine());
    }

    IEnumerator DoorCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 2f;
        Vector3 vec = transform.rotation.eulerAngles;
        Quaternion startAngle = Quaternion.Euler(vec);
        Quaternion targetAngle;
        if(isClosed)
        {
            targetAngle = Quaternion.Euler(openedAngle, vec.y, vec.z);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * doorSpeed;
                float t = Mathf.Clamp01(elapsedTime / duration);
                transform.rotation = Quaternion.Lerp(startAngle, targetAngle, t);
                yield return null;
                
                if(isClosed)
                    isClosed = !isClosed;
            }
            doorCo = null;
            transform.rotation = targetAngle;
        }
        else
        {
            targetAngle = Quaternion.Euler(closedAngle, vec.y, vec.z);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * doorSpeed;
                float t = Mathf.Clamp01(elapsedTime / duration);
                transform.rotation = Quaternion.Lerp(startAngle, targetAngle, t);
                yield return null;
                
                if(!isClosed)
                    isClosed = !isClosed;
            }
            doorCo = null;
            transform.rotation = targetAngle;
        }
    }
}

