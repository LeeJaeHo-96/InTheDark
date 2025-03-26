using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        popUp = GetComponent<PopUp>();
    }
    private void Update()
    {
        GoingMap();
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
