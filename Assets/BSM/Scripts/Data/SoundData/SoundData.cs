using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SoundData")]
public class SoundData : ScriptableObject
{
    [Header("SFX 클립 리스트")]
    public List<AudioClip> SfxClips = new List<AudioClip>();
    
    [Header("BGM 클립 리스트")]
    public List<AudioClip> BgmClips = new List<AudioClip>();
    
}
