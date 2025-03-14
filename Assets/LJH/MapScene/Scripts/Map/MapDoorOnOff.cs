using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoorOnOff : MonoBehaviour
{
    [SerializeField] List<GameObject> lockDoorList;
    [SerializeField] List<GameObject> openDoorList;

    private void Start()
    {

        if(openDoorList.Count > 0)
            CreateDoor(lockDoorList, openDoorList);
        else
            CreateDoor(lockDoorList);
    }

    static void CreateDoor(List<GameObject> lockList, List<GameObject> openList)
    {
        //문 한쪽만 있게 테스트용
        //for (int i = 0; i < lockList.Count; i++)
        for (int i = 0; i < 2; i++)
        {
            if (!lockList[i].activeSelf)
                lockList[i].SetActive(Random.Range(0, 3) < 0.5f);

            openList[i].SetActive(!lockList[i].activeSelf);
        }
    }

    static void CreateDoor(List<GameObject> lockList)
    {
        for (int i = 0; i < lockList.Count; i++)
        {
            if (!lockList[i].activeSelf)
                lockList[i].SetActive(Random.Range(0, 3) < 0.5f);
        }
    }
}
