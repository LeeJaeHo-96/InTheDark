using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuildingDoor : MonoBehaviourPun, IHitMe
{
    [SerializeField] Image progressBar;

    Coroutine doorIncreaseCo;

    GameObject player;
    GameObject[] players = new GameObject[4];
    [SerializeField] GameObject buildingSpawner;
    Vector3 buildingSpawnerPos;

    [SerializeField] GameObject sun;
    PopUp popUp;

    public bool HitMe { get; set; }

    void Start()
    {
        popUp = GetComponent<PopUp>();
        buildingSpawnerPos = buildingSpawner.transform.position;

    }

    private void Update()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (popUp.HitMe && Input.GetKeyDown(KeyCode.E) && doorIncreaseCo == null)
        {
            doorIncreaseCo = StartCoroutine(DoorIncreaseCoRoutine());
            player = GameObject.FindWithTag(Tag.Player);
        }

        if ((!popUp.HitMe || Input.GetKeyUp(KeyCode.E)) && doorIncreaseCo != null)
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
}
