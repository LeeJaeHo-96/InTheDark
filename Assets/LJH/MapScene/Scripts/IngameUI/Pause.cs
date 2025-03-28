using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : BaseUI
{
    GameObject pausePannel;
    GameObject optionPannel;

    GameObject exitPopUp;

    Button continueButton;
    Button optionButton;
    Button exitButton;

    Button yesButton;
    Button noButton;


    private void Awake()
    {
        Bind();
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if(Time.timeScale > 0)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;


            pausePannel.SetActive(!pausePannel.activeSelf);
        }
    }

    /// <summary>
    /// 일시정지 닫기
    /// </summary>
    void ContinueButton()
    {
        //임시
        Time.timeScale = 1f;
        pausePannel.SetActive(false);
    }

    void OptionButton()
    {
        optionPannel.SetActive(true);
    }

    /// <summary>
    /// 게임종료 팝업 호출
    /// </summary>
    void ExitPopUp()
    {
        exitPopUp.SetActive(true);
    }
    /// <summary>
    /// 게임씬일경우 > 대기씬으로 이동
    /// </summary>
    void ReturnGame()
    {
        if (SceneManager.GetActiveScene().name == "WaitingScene")
            return;

        SceneManager.LoadScene("WaitingScene");
    }

    /// <summary>
    /// 대기씬일경우 > 게임종료
    /// </summary>
    void ExitGame()
    {
        if (SceneManager.GetActiveScene().name != "WaitingScene")
            return;

        Debug.Log(IngameManager.Instance.money);
        Database.instance.SaveData(FirebaseManager.Auth.CurrentUser.UserId, "Slot_1", IngameManager.Instance.money, IngameManager.Instance.days);

#if UNITY_EDITOR
        //Comment : 유니티 에디터상에서 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //Comment : 빌드 상에서 종료
        Application.Quit();
#endif
    }

    void ExitNo()
    {
        exitPopUp.SetActive(false);
    }


    void Init()
    {
        pausePannel = GetUI("PausePannel");
        optionPannel = GetUI("SettingCanvas");

        exitPopUp = GetUI("ExitPopUp");

        continueButton = GetUI<Button>("ContinueButton");
        optionButton = GetUI<Button>("OptionButton");
        exitButton = GetUI<Button>("ExitButton");

        yesButton = GetUI<Button>("YesButton");
        noButton = GetUI<Button>("NoButton");

        continueButton.onClick.AddListener(() => ContinueButton());
        optionButton.onClick.AddListener(() => OptionButton());
        exitButton.onClick.AddListener(() => ExitPopUp());

        noButton.onClick.AddListener(() => ExitNo());
        yesButton.onClick.AddListener
        (() =>
            {
                ReturnGame();
                ExitGame();
            }
        );
    }



}
