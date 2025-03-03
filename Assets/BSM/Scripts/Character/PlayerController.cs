using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    public PlayerStats PlayerStats => _playerStats;
    public Rigidbody PlayerRb => _playerRb;
    
    private PlayerState[] _playerStates = new PlayerState[(int)PState.SIZE];
    private PlayerStats _playerStats;
    private Rigidbody _playerRb;
    
    
    public Vector3 MoveDir = Vector3.zero;
    private PState _curState = PState.IDLE;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody>();
        
        _playerStates[(int)PState.IDLE] = new PlayerIdle(this);
        _playerStates[(int)PState.WALK] = new PlayerWalk(this);
        _playerStates[(int)PState.RUN] = new PlayerRun(this);
        _playerStates[(int)PState.JUMP] = new PlayerJump(this);
    }

    private void Start()
    {
        _playerStates[(int)_curState].Enter();
    }

    private void Update()
    {
        _playerStates[(int)_curState].Update();
        InputKey();
    }

    private void FixedUpdate()
    {
        _playerStates[(int)_curState].FixedUpdate();
    }


    private void InputKey()
    {
        MoveDir.x = Input.GetAxis("Horizontal");
        MoveDir.z = Input.GetAxis("Vertical");
    }
    
    /// <summary>
    /// 현재 플레이어의 상태 전환
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(PState newState)
    {
        if (!photonView.IsMine) return;
        
        _playerStates[(int)_curState].Exit();
        _curState = newState;
        _playerStates[(int)_curState].Enter();
    }
    
}
