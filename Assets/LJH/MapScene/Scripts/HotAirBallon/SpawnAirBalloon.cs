using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAirBalloon : MonoBehaviour
{
    GameObject airBallonPrefab;



    public void CallAirBalloon()
    {
        airBallonPrefab = PhotonNetwork.Instantiate("HotAirBalloon", transform.position, Quaternion.identity);
    }

}
