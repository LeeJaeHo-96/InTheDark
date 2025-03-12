using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InDoor : MonoBehaviour
{
    bool isClosed;
    NavMeshObstacle obstacle;

    private void Start()
    {
        isClosed = false;
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;
    }
}
