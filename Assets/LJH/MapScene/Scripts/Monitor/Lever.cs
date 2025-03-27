using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Stage
{
    startLand,
    middleLand,
    endLand,
    sellLand
}
public class Lever : MonoBehaviourPun
{
    public Stage Stage;
    PopUp popUp;

    [SerializeField] TMP_Text posName;

    private void Start()
    {
        popUp = GetComponent<PopUp>();
    }
    private void Update()
    {
        GoingMap();
    }

    public void posNameisWhat(Stage stage)
    {
        switch (stage)
        {
            case Stage.startLand:
                posName.text = "시작의 섬";
                break;

            case Stage.middleLand:
                posName.text = "중간섬";
                break;

            case Stage.endLand:
                posName.text = "끝의 섬";
                break;

            case Stage.sellLand:
                posName.text = "만물상 선박";
                break;
        }
    }

    void GoingMap()
    {
        if(popUp.HitMe && Input.GetKeyDown(KeyCode.E))
        {
            switch(Stage)
            {
                case Stage.startLand:
                    GameManager.Instance.SceneBGM(SceneType.INGAME);
                    SceneManager.LoadScene("MapScene1");
                    break;

                case Stage.middleLand:
                    //Todo : 스테이지 채워야함 SceneManager.LoadScene("");
                    Debug.Log("중간 섬으로 갑니다");
                    break;

                case Stage.endLand:
                    //Todo : 스테이지 채워야함 SceneManager.LoadScene("");
                    Debug.Log("끝의 섬으로 갑니다");
                    break;

                case Stage.sellLand:
                    SceneManager.LoadScene("StoreScene");
                    Debug.Log("상점 섬으로 갑니다");
                    break;
            }
        }
    }

}
