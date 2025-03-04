using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerStats : MonoBehaviourPun
{
    private float _walkSpeed = 5f;

    public float WalkSpeed
    {
        get => _walkSpeed;
    }

    private float _runSpeed = 10f;

    public float RunSpeed
    {
        get => _runSpeed;
        
    }

    private float _jumpPower = 3f;

    public float JumpPower
    {
        get => _jumpPower;
    }
    


}
