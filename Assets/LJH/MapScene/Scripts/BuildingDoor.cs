using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuildingDoor : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Image progressBar;

    bool isClosed = false;

    Coroutine doorIncreaseCo;

    GameObject player;
    [SerializeField] GameObject buildingSpawner;
    Vector3 buildingSpawnerPos;


    void Start()
    {
        buildingSpawnerPos = buildingSpawner.transform.position;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        OpenDoor();
    }

    void OpenDoor()
    {
        if (isClosed && Input.GetKeyDown(KeyCode.E) && doorIncreaseCo == null)
        {
            doorIncreaseCo = StartCoroutine(DoorIncreaseCoRoutine());
            player = GameObject.FindWithTag(Tag.Player);
        }

        if (isClosed && Input.GetKeyUp(KeyCode.E) && doorIncreaseCo != null)
        {
            StopCoroutine(doorIncreaseCo);
            doorIncreaseCo = null;
            progressBar.fillAmount = 0f;
        }
    }

    IEnumerator DoorIncreaseCoRoutine()
    {
        while (true)
        {
            progressBar.fillAmount += 0.5f * Time.deltaTime;
            yield return null;

            if (progressBar.fillAmount >= 1)
                player.transform.position = buildingSpawnerPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Player))
        {
            player = other.gameObject;
            isClosed = true;
            player.transform.position = buildingSpawnerPos;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.Player))
        {
            isClosed = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isClosed);
        }
        else
        {
            isClosed = (bool)stream.ReceiveNext();
        }
    }
}
