using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    Door doorScriptL;
    Door doorScriptR;
    public Coroutine doorCoL;
    public Coroutine doorCoR;

    public bool doorOpend = true;
    private void Start()
    {
        doorScriptL = leftDoor.GetComponent<Door>();
        doorScriptR = rightDoor.GetComponent<Door>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (doorOpend)
            {
                doorOpend = false;
                doorCoL = StartCoroutine(doorScriptL.DoorOpenCoroutine());
                doorCoR = StartCoroutine(doorScriptR.DoorOpenCoroutine());
            }
        }
    }
}
