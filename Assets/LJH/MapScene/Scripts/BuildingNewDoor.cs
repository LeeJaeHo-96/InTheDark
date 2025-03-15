using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingNewDoor : MonoBehaviour
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
                indoor.obstacle.enabled = !indoor.obstacle.enabled;
                doorCo = StartCoroutine(DoorCoroutine());
            }
        }
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
            targetAngle = Quaternion.Euler(openedAngle, vec.y, vec.z);
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
            targetAngle = Quaternion.Euler(closedAngle, vec.y, vec.z);
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


