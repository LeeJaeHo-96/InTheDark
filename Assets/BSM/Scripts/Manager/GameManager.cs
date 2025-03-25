using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExitGames.Client.Photon;
using Firebase.Auth;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Screen = UnityEngine.Device.Screen;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int CurFrame;
    [HideInInspector] public int CurVSyncCount;
    [HideInInspector] public int CurWindowMode;
    [HideInInspector] public float CurGammaBrightness;
    
    public List<PlayerController> PlayerObjects = new List<PlayerController>();
    public static GameManager Instance;
    public FirebaseUser CurUser;
    public Coroutine PlayerSearchCo;
    public int ItemLayerIndexValue => LayerMask.NameToLayer("Item");
    private int _indoorLayerIndexValue => LayerMask.NameToLayer("InDoor");
    private static int _refreshRate;
    private int _itemLayer;
    private int _playerCount = 0;
    
    private SoundManager _soundManager;
    private ColorGrading PostVolume; 
    private PostProcessProfile _postProfile;
    private Dictionary<int, int> _frameDict = new Dictionary<int, int>()
    {
        {0, _refreshRate},
        {1, -1},
        {2, 144},
        {3, 120},
        {4, 60},
        {5, 30}
    };
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    private void Start()
    {
        //TODO: 임시로 가져오는 user id
        CurUser = FirebaseManager.Auth.CurrentUser; 
        _soundManager = SoundManager.Instance;
        StartFrame();
        StartWindowMode();
        StartGammaBrightness(); 
    }
 
    /// <summary>
    /// 게임 매니저 초기화 작업
    /// </summary>
    private void Init()
    { 
        Physics.IgnoreLayerCollision(ItemLayerIndexValue,ItemLayerIndexValue); 
        Physics.IgnoreLayerCollision(ItemLayerIndexValue, _indoorLayerIndexValue);
        _postProfile = transform.GetChild(0).GetComponent<PostProcessVolume>().profile;
        _postProfile.TryGetSettings<ColorGrading>(out PostVolume);
        _postProfile = transform.GetChild(0).GetComponent<PostProcessVolume>().profile; 
    }
    
    /// <summary>
    /// 접속 유저 확인
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayerSearchRoutine()
    {
        while (true)
        {  
            if (FindObjectsOfType<PlayerController>().Count() != _playerCount)
            { 
                PlayerObjects.Clear(); 
                PlayerObjects = FindObjectsOfType<PlayerController>().ToList();
                _playerCount = PlayerObjects.Count();
            } 
            yield return new WaitForSeconds(0.5f);
        }
    }    
    
    
    //TODO: 추후 게임 도중 나가기 옵션에 달아놓을 것
    /// <summary>
    /// 게임 도중 유저 나갔을 때 리스트 삭제
    /// </summary>
    /// <param name="player"></param>
    public void PlayerRemove(PlayerController player)
    {
        if (PlayerObjects.Contains(player))
        {
            PlayerObjects.Remove(player);
        }
    }
    
    /// <summary>
    /// 게임 시작 시 설정한 프레임으로 변경
    /// </summary>
    private void StartFrame()
    {
        CurFrame = _frameDict[DataManager.Instance.UserSettingData.Frame];
        CurVSyncCount = DataManager.Instance.UserSettingData.Vsync;
        Application.targetFrameRate = CurFrame;
        QualitySettings.vSyncCount = CurVSyncCount;
         
    }
    
    /// <summary>
    /// 게임 시작 시 설정한 윈도우 모드로 변경
    /// </summary>
    private void StartWindowMode()
    {
        CurWindowMode = DataManager.Instance.UserSettingData.WindowMode;
        Screen.fullScreenMode = (FullScreenMode)CurWindowMode;
 
    }
    
    /// <summary>
    /// 게임 시작 시 감마, 밝기 설정
    /// </summary>
    private void StartGammaBrightness()
    {
        CurGammaBrightness = DataManager.Instance.UserSettingData.GammaBrightness;
        PostVolume.postExposure.value = CurGammaBrightness;
        PostVolume.gamma.value = new Vector4(CurGammaBrightness, CurGammaBrightness, CurGammaBrightness, CurGammaBrightness); 
    }
    
    /// <summary>
    /// 전달 받은 프레임 옵션으로 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetFrames(int value, bool force = false)
    {
        CurFrame = value;
        CurVSyncCount = force ? 1 : 0;
        
        Application.targetFrameRate = value; 
        QualitySettings.vSyncCount = CurVSyncCount; 
    }
    
    /// <summary>
    /// 전달 받은 값으로 윈도우 모드 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetWindowMode(int value)
    {
        CurWindowMode = value;
        
        Screen.fullScreenMode = (FullScreenMode)CurWindowMode; 
    }
    
    /// <summary>
    /// 전달 받은 값으로 감마, 밝기 변경
    /// </summary>
    /// <param name="value"></param>
    public void SetGammaBrightness(float value)
    {
        CurGammaBrightness = value;
        
        PostVolume.postExposure.value = CurGammaBrightness;
        PostVolume.gamma.value = new Vector4(CurGammaBrightness, CurGammaBrightness, CurGammaBrightness, CurGammaBrightness);
    }

    /// <summary>
    /// 현재 씬 위치에 따른 BGM 재생
    /// </summary>
    /// <param name="sceneType"></param>
    public void SceneBGM(SceneType sceneType)
    {
        string sceneName = sceneType switch
        {
            SceneType.MAIN => "LobbyBGM",
            SceneType.INGAME => "MainBGM"
        };
        
        _soundManager.PlayBGM(_soundManager.SoundDatas.SoundDict[sceneName]);
    }
    
}
