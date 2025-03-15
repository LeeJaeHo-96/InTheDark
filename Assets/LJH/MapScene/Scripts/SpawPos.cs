using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawPos : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag(Tag.Player);

        player.transform.position = transform.position;
    }

}
