using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Settings
{
    FRAME, WINDOWMODE, GAMMA, MASTER_VOLUME, BGM_VOLUME, SFX_VOLUME, SIZE
}

[System.Serializable]
public enum RoomState
{
    PUBLIC, FRIENDS_ONLY
}