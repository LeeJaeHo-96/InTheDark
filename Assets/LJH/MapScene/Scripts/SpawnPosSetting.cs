using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosSetting : MonoBehaviour
{
    private void Start()
    {
        PlayerPosSetting();
    }
    void PlayerPosSetting()
    {
        for (int i = 0; i < GameManager.Instance.PlayerObjects.Count; i++)
        {
            GameManager.Instance.PlayerObjects[i].gameObject.transform.position = transform.position;
        }
    }
}
