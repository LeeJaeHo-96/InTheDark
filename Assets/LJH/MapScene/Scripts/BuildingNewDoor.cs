using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingNewDoor : MonoBehaviourPun, IHitMe
{
    [SerializeField] InDoor indoor;

    float doorSpeed = 5f;
    private Coroutine doorCo;
    public bool isClosed = true;

    //현재 y값 받아와야함
    private float openedAngle;
    private float closedAngle;


    public bool HitMe { get; set; }

    private void Start()
    {
        Vector3 defaultVec = transform.rotation.eulerAngles;
        openedAngle = defaultVec.y + 100f;
        closedAngle = defaultVec.y;
    }
    private void Update()
    {
        if (HitMe && Input.GetKeyDown(KeyCode.E))
        {
            if (doorCo == null)
            {
                if (indoor != null)
                {
                    photonView.RPC("RPCObstacle", RpcTarget.AllViaServer);
                }
                photonView.RPC("RPCDoor", RpcTarget.AllViaServer);
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


