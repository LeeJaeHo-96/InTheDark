using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SoundData")]
public class SoundData : ScriptableObject
{
    [Header("BGM 딕셔너리")]
    public SerializedDictionary<string, AudioClip> SoundDict = new SerializedDictionary<string, AudioClip>();
}
  