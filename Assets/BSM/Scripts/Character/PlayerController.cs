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
    private Transform _eyePos;
    
    public Vector3 MoveDir = Vector3.zero;
    private PState _curState = PState.IDLE;
    
    private void Awake() => Init();
    
    private void Init()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody>();
        _eyePos = transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        
        _playerStates[(int)PState.IDLE] = new PlayerIdle(this);
        _playerStates[(int)PState.WALK] = new PlayerWalk(this);
        _playerStates[(int)PState.RUN] = new PlayerRun(this);
        _playerStates[(int)PState.JUMP] = new PlayerJump(this);
    }

    private void Start()
    {
        Camera.main.transform.SetParent(_eyePos);
        Camera.main.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        _playerStates[(int)_curState].Enter();
    }

    private void Update()
    {
        _playerStates[(int)_curState].Update();
        InputKey();
        InputRotate();
    }

    private void FixedUpdate()
    {
        _playerStates[(int)_curState].FixedUpdate();
    }


    private void InputKey()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.z = Input.GetAxisRaw("Vertical");
    }

    private float _mouseX;
    private float _mouseY;
    
    private void InputRotate()
    {
        
        _mouseX += Input.GetAxis("Mouse X");
        _mouseY += Input.GetAxis("Mouse Y");

        _mouseY = Mathf.Clamp(_mouseY, 0f, 180f);
        
        transform.rotation = Quaternion.Euler(0, _mouseX, 0f);
        
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
