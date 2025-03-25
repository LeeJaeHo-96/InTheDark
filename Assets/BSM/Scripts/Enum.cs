using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: 키바인드, 마우스 반전 추가
[System.Serializable]
public enum Settings
{
    FRAME, WINDOWMODE, GAMMA, MASTER_VOLUME, BGM_VOLUME, SFX_VOLUME, SENSITIVITY, SIZE
}


[System.Serializable]
public enum PState
{
    IDLE, WALK, RUN, JUMP , HURT, DEATH, ATTACK, SIZE
}

[System.Serializable]
public enum ItemHoldingType
{
    ONEHANDED, TWOHANDED
}

[System.Serializable]
public enum SceneType
{
    MAIN, INGAME
}