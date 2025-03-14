using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class InDoor : MonoBehaviour
{
    bool isClosed;
    NavMeshObstacle obstacle;
    public bool hitMe;
    float gage = 0;

    Coroutine doorCo;

    private void Start()
    {
        hitMe = false;
        isClosed = false;
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;
    }

    private void Update()
    {
        if (hitMe && Input.GetKey(KeyCode.E))
        {
            doorCo = StartCoroutine(DoorIncreaseCoRoutine());
        }
        else if (hitMe && Input.GetKey(KeyCode.E))
        {
            StopCoroutine(doorCo);
            doorCo = null;
            gage = 0;
        }
    }

    IEnumerator DoorIncreaseCoRoutine()
    {
        while (true)
        {
            //progressBar.fillAmount += 0.5f * Time.deltaTime;
            
            gage += 0.5f * Time.deltaTime;
            yield return null;


            if (gage >= 1f)
            {
                if (isClosed)
                    OpenDoor();

                else
                    CloseDoor();
            }
            //if (progressBar.fillAmount >= 1)
                //player.transform.position = buildingSpawnerPos;
        }
    }

    void OpenDoor()
    {
        Vector3 doorPos = new Vector3(0, 5, 0);
        transform.position += doorPos;
        isClosed = false;
        obstacle.enabled = false;
    }

    void CloseDoor()
    {
        Vector3 doorPos = new Vector3(0, 5, 0);
        transform.position -= doorPos;
        isClosed = true;
        obstacle.enabled = true;
    }
}
