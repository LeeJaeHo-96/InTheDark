using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// 파이어베이스 관리하는 메니저
/// </summary>
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    private FirebaseApp app;
    private FirebaseAuth auth;
    private FirebaseDatabase database;

    public static FirebaseApp App => Instance.app;
    public static FirebaseAuth Auth => Instance.auth;
    public static FirebaseDatabase Database => Instance.database;

    private void Awake()
    {
        // 싱글톤 사용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 호환성 점검
        CheckDependency();
    }

    private void CheckDependency()
    {
        // 호환성에 대해서 점검을 해주는 부분
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase 의존성 확인 성공!");
            }
            else
            {
                Debug.LogError($"Firebase 의존성 확인 실패! 이유: {task.Result}");
                // Firebase Unity SDK is not safe to use here.
                app = null;
                auth = null;
                database = null;
            }
        });
    }

}
