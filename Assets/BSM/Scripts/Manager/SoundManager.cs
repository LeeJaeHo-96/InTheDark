using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public SoundData SoundDatas;

    
    [SerializeField] private AudioMixer _audioMixer;
    
    private AudioSource _sfxSource;
    private AudioSource _bgmSource;

    private float _masterVolume;
    private float _sfxVolume;
    private float _bgmVolume;
    
    
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
 
    }

    private void Start()
    {
        _masterVolume = DataManager.Instance.UserSettingData.MasterVolume;
        SetVolumeMaster(Mathf.Pow(10, _masterVolume / 20f));

        _bgmVolume = DataManager.Instance.UserSettingData.BgmVolume;
        SetVolumeBGM(_bgmVolume / 20f);

        _sfxVolume = DataManager.Instance.UserSettingData.SfxVolume;
        SetVolumeSFX(_sfxVolume / 20f);
         
    }


    /// <summary>
    /// 전체 사운드 조절
    /// </summary>
    /// <param name="volume">볼륨값</param>
    public void SetVolumeMaster(float volume)
    {
        _audioMixer.SetFloat("MASTER", Mathf.Log10(volume)* 20f);
        _audioMixer.GetFloat("MASTER", out _masterVolume);
    }
    
    /// <summary>
    /// 효과음 소리 조절
    /// </summary>
    /// <param name="volume">볼륨값</param>
    public void SetVolumeSFX(float volume)
    {
        _audioMixer.SetFloat("SFX", volume* 20f);
        _audioMixer.GetFloat("SFX", out _sfxVolume);

    }
    
    /// <summary>
    /// 배경음 소리 조절
    /// </summary>
    /// <param name="volume">볼륨 값</param>
    public void SetVolumeBGM(float volume)
    {
        _audioMixer.SetFloat("BGM", volume* 20f);
        _audioMixer.GetFloat("BGM", out _bgmVolume);
    }

    public float GetVolumeMaster() => _masterVolume;
    public float GetVolumeSfx() => _sfxVolume;
    public float GetVolumeBgm() => _bgmVolume;
     
    /// <summary>
    /// 효과음 재생
    /// </summary>
    /// <param name="clip">효과음 사운드 파일</param>
    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
    
    /// <summary>
    /// 배경음 재생
    /// </summary>
    /// <param name="clip">배경음 사운드 파일</param>
    public void PlayBGM(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }
    
}
