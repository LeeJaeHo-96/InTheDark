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
    [SerializeField] ItemSave itemSave;

    private void Start()
    {
        popUp = GetComponent<PopUp>();
        posNameisWhat(Stage.startLand);
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
                posName.text = "������ ��";
                break;

            case Stage.middleLand:
                posName.text = "�߰���";
                break;

            case Stage.endLand:
                posName.text = "���� ��";
                break;

            case Stage.sellLand:
                posName.text = "������ ����";
                break;
        }
    }

    void GoingMap()
    {
        if(popUp.HitMe && Input.GetKeyDown(KeyCode.E))
        {
            //itemSave.SaveItems();

            switch(Stage)
            {
                case Stage.startLand:
                    IngameManager.Instance.PlayerCheck();
                    IngameManager.Instance.days++;
                    PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("MapScene1"));
                    break;

                case Stage.middleLand:
                    IngameManager.Instance.PlayerCheck();
                    IngameManager.Instance.days++;
                    //Todo : �������� ä������ SceneManager.LoadScene("");
                    Debug.Log("�߰� ������ ���ϴ�");
                    break;

                case Stage.endLand:
                    IngameManager.Instance.PlayerCheck();
                    IngameManager.Instance.days++;
                    //Todo : �������� ä������ SceneManager.LoadScene("");
                    Debug.Log("���� ������ ���ϴ�");
                    break;

                case Stage.sellLand:
                    IngameManager.Instance.PlayerCheck();
                    IngameManager.Instance.days++;
                    PhotonNetwork.LoadLevel(SceneUtility.GetBuildIndexByScenePath("StoreScene"));
                    Debug.Log("���� ������ ���ϴ�");
                    break;
            }
        }
    }

}
