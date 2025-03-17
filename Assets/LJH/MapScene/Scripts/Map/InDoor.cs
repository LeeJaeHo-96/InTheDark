using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class InDoor : MonoBehaviourPun, IHitMe
{
    [SerializeField] BuildingNewDoor door;

    public NavMeshObstacle obstacle;
    public bool HitMe 
    {
        get => HitMe; 
        set
        {
            if(HitMe != door.HitMe) 
                door.HitMe = HitMe;
        }
    }

    //이벤트로 작성할 경우 예시
    //public UnityAction<bool> hitMeEvent;


    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;

        //hitMeEvent += HitMeShare;


    }

    public void HitMeShare(bool hitMe)
    {
        if (door != null)
            door.HitMe = hitMe;
    }



}
