using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class InDoor : MonoBehaviourPun
{
    [SerializeField] BuildingNewDoor door;

    public NavMeshObstacle obstacle;
    public bool hitMe = false;

    public UnityAction<bool> hitMeEvent;

    //프로퍼티로 구현 예시
    //public bool meHit
    //{
    //    get => hitMe;
    //    set
    //    {
    //        if (door != null)
    //            if (hitMe != door.hitMe)
    //                door.hitMe = hitMe;
    //    }
    //}

    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;

        hitMeEvent += HitMeShare;


    }

    public void HitMeShare(bool hitMe)
    {
        if (door != null)
            door.hitMe = hitMe;
    }



}
