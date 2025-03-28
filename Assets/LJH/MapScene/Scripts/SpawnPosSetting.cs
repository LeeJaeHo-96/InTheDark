using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosSetting : MonoBehaviour
{
    [SerializeField] GameObject pos;
    private void Start()
    {
        PlayerPosSetting();
    }
    void PlayerPosSetting()
    {
        for (int i = 0; i < GameManager.Instance.PlayerObjects.Count; i++)
        {
            //이따수정해
            GameManager.Instance.PlayerObjects[i].gameObject.transform.position = pos.transform.position;
        }
    }
}
