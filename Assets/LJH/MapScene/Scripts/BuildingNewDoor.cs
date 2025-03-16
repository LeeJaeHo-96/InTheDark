using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingNewDoor : MonoBehaviourPun
{
    [SerializeField] InDoor indoor;

    float doorSpeed = 5f;
    private Coroutine doorCo;
    public bool isClosed = true;

    //현재 y값 받아와야함
    private float openedAngle;
    private float closedAngle;

    public bool hitMe = false;

    private void Start()
    {
        Vector3 defaultVec = transform.rotation.eulerAngles;
        openedAngle = defaultVec.y + 100f;
        closedAngle = defaultVec.y;
    }
    private void Update()
    {
        if (hitMe && Input.GetKeyDown(KeyCode.E))
        {
            if (doorCo == null)
            {
                Debug.Log("눌림");
                if (indoor != null)
                {
                    photonView.RPC("RPCObstacle", RpcTarget.AllViaServer);
                }
                photonView.RPC("RPCDoor", RpcTarget.AllViaServer);
                Debug.Log("1차 실행됨");
            }
        }
    }

    [PunRPC]
    void RPCObstacle()
    {
        indoor.obstacle.enabled = !indoor.obstacle.enabled;
    }

    [PunRPC]
    void RPCDoor()
    {
        doorCo = StartCoroutine(DoorCoroutine());
        Debug.Log("RPCDoor 실행됨");
    }

    IEnumerator DoorCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 2f;
        Vector3 vec = transform.rotation.eulerAngles;
        Quaternion startAngle = Quaternion.Euler(vec);
        Quaternion targetAngle;
        if (isClosed)
        {
            Debug.Log("문닫혀있음");
            targetAngle = Quaternion.Euler(vec.x, openedAngle, vec.z);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * doorSpeed;
                float t = Mathf.Clamp01(elapsedTime / duration);
                transform.rotation = Quaternion.Lerp(startAngle, targetAngle, t);
                yield return null;

                if (isClosed)
                    isClosed = !isClosed;
            }
            doorCo = null;
            transform.rotation = targetAngle;
        }
        else
        {
            Debug.Log("문열려있음");
            targetAngle = Quaternion.Euler(vec.x, closedAngle, vec.z);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime * doorSpeed;
                float t = Mathf.Clamp01(elapsedTime / duration);
                transform.rotation = Quaternion.Lerp(startAngle, targetAngle, t);
                yield return null;

                if (!isClosed)
                    isClosed = !isClosed;
            }
            doorCo = null;
            transform.rotation = targetAngle;
        }
    }
}


