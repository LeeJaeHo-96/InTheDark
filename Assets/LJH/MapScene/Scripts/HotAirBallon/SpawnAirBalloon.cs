using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAirBalloon : MonoBehaviourPun
{
    GameObject airBallonPrefab;


    /// <summary>
    /// 컴퓨터에서 물건 구매 완료시 해당 함수 호출하여 열기구 생성
    /// </summary>
    public void CallAirBalloon()
    {
        airBallonPrefab = PhotonNetwork.Instantiate("HotAirBalloon", transform.position, Quaternion.identity);
    }

}
