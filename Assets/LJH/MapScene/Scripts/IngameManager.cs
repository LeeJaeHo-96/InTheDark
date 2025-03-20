using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager Instance;

   public float time { get; set; }
   public float minute { get; set; }

    float randomMinute;

    float elapsedTime = 1f;

    float money;

    private void Awake()
    {
        SingletonInit();
    }


    //시간의 경우 10초마다 5 ~10분이 흐르고
    //am8시로 시작해서
    //pm8시되면 배 침몰

    /// <summary>
    /// 타이머 리셋 - 맵씬 시작될 때 호출
    /// </summary>
    public void TimerReset()
    {
        Debug.Log("시간 초기화되었음");
        time = 8;
        minute = 0;
    }

    /// <summary>
    /// 시간 계산기 - 맵씬 시작될 때 호출
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimerCount()
    {
        Debug.Log("타이머 카운트 시작");
        while (true) 
        {
            Debug.Log("시간 흐름");
            randomMinute = Random.Range(5, 10);
            minute += randomMinute;

            if (minute >= 60)
            {
                time++;
                minute = minute - 60;
            }

            if(time >= 20)
            {
                TimeOver();
            }
            yield return new WaitForSeconds(elapsedTime);
        }
    }

    void TimeOver()
    {
        //Todo: 배출발시킴
    }    



    void SingletonInit()
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
    }
}
