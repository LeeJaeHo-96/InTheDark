using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] DoorController doorCon;
    [SerializeField] GameObject leftPill;
    [SerializeField] GameObject rightPill;

    Collider leftColl;
    Collider rightColl;

    //문 열닫 속도용 변수
    Vector3 doorSpeed = new Vector3(0, 0, 0.1f);

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
                gameObject.transform.position += doorSpeed;
            if(gameObject.name == "RightDoor")
                gameObject.transform.position -= doorSpeed;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (gameObject.name == "LeftDoor")
        {
            if (collision.collider == leftColl)
            {
                Debug.Log("충돌남왼쪽");
                doorCon.StopCoroutine(doorCon.DoorCoL);
                doorCon.DoorCoL = null;
            }
        }
        if (gameObject.name == "RightDoor")
        {
            if (collision.collider == rightColl)
            {
                Debug.Log("충돌남오른쪽");
                doorCon.StopCoroutine(doorCon.DoorCoR);
                doorCon.DoorCoR = null;
            }
        }
    }
}
