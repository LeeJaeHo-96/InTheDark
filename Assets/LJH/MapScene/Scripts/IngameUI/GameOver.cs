using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void Update()
    {
        if (PhotonNetwork.InRoom && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(GameObject.Find(nameof(Camera)).gameObject);

            PhotonNetwork.LeaveRoom();
            //인게임 매니저 삭제
            Destroy(transform.parent.parent.gameObject);

            //게임 오버시 데이터 초기화
            Database.instance.ResetData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_1");

        }
    }
}
