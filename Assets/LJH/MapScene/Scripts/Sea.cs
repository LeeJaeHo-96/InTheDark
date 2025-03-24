using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tag.Player))
        {
            collision.gameObject.GetComponent<PlayerController>().ChangeState(PState.DEATH);
        }
    }
}
