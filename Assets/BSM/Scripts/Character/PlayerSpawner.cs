using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject _player;
 
    private void OnEnable()
    {
        PunManager.Instance.OnChangedPlayer += PlayerSpawn;
    }

    private void OnDisable()
    {
        PunManager.Instance.OnChangedPlayer -= PlayerSpawn;
    }

    private void PlayerSpawn()
    {
        StartCoroutine(PlayerSpawnRoutine()); 
    }

    private IEnumerator PlayerSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        _player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
    
}
