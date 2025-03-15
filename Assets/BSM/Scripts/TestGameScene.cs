using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameScene : MonoBehaviourPunCallbacks
{
    private int temp = 0;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.ConnectUsingSettings();
    }
    
    //TODO: 테스트용 
    private void Update()
    {
        if (temp != DataManager.Instance.PlayerObjects.Count)
        {
            DataManager.Instance.PlayerObjects.Clear();
            DataManager.Instance.PlayerObjects = FindObjectsOfType<PlayerController>().ToList();
        } 
         
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        
        PhotonNetwork.JoinOrCreateRoom("테스트방", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(SpawnRoutine());
    }
  
    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        Spawn();
    }
    
    private void Spawn()
    {
       GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
       DataManager.Instance.PlayerObjects = FindObjectsOfType<PlayerController>().ToList();
    } 
}
