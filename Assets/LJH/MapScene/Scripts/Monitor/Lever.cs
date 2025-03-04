using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Stage
{
    startLand,
    middleLand,
    endLand
}
public class Lever : MonoBehaviourPun
{
    public Stage Stage;



    private void Update()
    {
        GoingMap();
    }

    void GoingMap()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            switch(Stage)
            {
                case Stage.startLand:
                    //Todo : 스테이지 채워야함 SceneManager.LoadScene("");
                    Debug.Log("시작의 섬으로 갑니다");
                    break;

                case Stage.middleLand:
                    //Todo : 스테이지 채워야함 SceneManager.LoadScene("");
                    Debug.Log("중간 섬으로 갑니다");
                    break;

                case Stage.endLand:
                    //Todo : 스테이지 채워야함 SceneManager.LoadScene("");
                    Debug.Log("끝의 섬으로 갑니다");
                    break;
            }
        }
    }
}
