using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class InDoor : MonoBehaviour, IPunObservable
{
    [SerializeField] BuildingNewDoor door;

    public NavMeshObstacle obstacle;
    public bool hitMe = false;
    private void Start()
    {
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = true;

    }

    private void Update()
    {
        if (door != null)
            if (hitMe != door.hitMe)
                door.hitMe = hitMe;    
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(obstacle.enabled);
            stream.SendNext(hitMe);
        }
        else
        {
            this.obstacle.enabled = (bool)stream.ReceiveNext();
            this.hitMe = (bool)stream.ReceiveNext();
        }
    }
}
