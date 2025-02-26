using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    Door doorScriptL;
    Door doorScriptR;
    Coroutine doorCo;
    private void Start()
    {
        doorScriptL = leftDoor.GetComponent<Door>();
        doorScriptR = rightDoor.GetComponent<Door>();

        doorCo = StartCoroutine(doorScriptL.DoorOpenCoroutine());
        doorCo = StartCoroutine(doorScriptR.DoorOpenCoroutine());
    }
}
