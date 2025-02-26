using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject leftPill;
    [SerializeField] GameObject rightPill;

    Collider leftColl;
    Collider rightColl;

    private void Start()
    {
        leftColl = leftPill.GetComponent<Collider>();
        rightColl = rightPill.GetComponent<Collider>();

    }
    public IEnumerator DoorOpenCoroutine()
    {
        while (true)
        {
            if(gameObject.name == "LeftDoor")
                gameObject.transform.position += new Vector3(0, 0, 0.05f);
            if(gameObject.name == "RightDoor")
                gameObject.transform.position -= new Vector3(0, 0, 0.05f);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name == "LeftDoor")
            if (collision.collider == leftColl)
                StopCoroutine(DoorOpenCoroutine());

        if (gameObject.name == "RightDoor")
            if (collision.collider == rightColl)
                StopCoroutine(DoorOpenCoroutine());
    }
}
