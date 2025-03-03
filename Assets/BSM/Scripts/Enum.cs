using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: 감도, 키바인드, 마우스 반전 추가
[System.Serializable]
public enum Settings
{
    FRAME, WINDOWMODE, GAMMA, MASTER_VOLUME, BGM_VOLUME, SFX_VOLUME, SIZE
}


[System.Serializable]
public enum PState
{
    IDLE, WALK, RUN, JUMP , SIZE
}