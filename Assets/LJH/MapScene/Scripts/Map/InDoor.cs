using UnityEngine;
using UnityEngine.AI;

public class InDoor : MonoBehaviour
{
    [SerializeField] BuildingNewDoor door;

    public NavMeshObstacle obstacle;
    bool hitMe = false;
    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;

    }

    private void Update()
    {
        if(hitMe != door.hitMe)
            door.hitMe = hitMe;    
    }

}
