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
    private Camera _cam;
    
    public Vector3 MoveDir = Vector3.zero;
    private PState _curState = PState.IDLE;
    
    private float _mouseX;
    private float _mouseY;
    
    private void Awake() => Init();
    
    private void Init()
    {
        //TODO: 임시로 여기서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        
        _cam = Camera.main;
        
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
        _cam.transform.SetParent(_eyePos);
        _cam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
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

    /// <summary>
    /// 키보드 입력
    /// </summary>
    private void InputKey()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.z = Input.GetAxisRaw("Vertical"); 
    }


    /// <summary>
    /// 마우스 회전 입력
    /// </summary>
    private void InputRotate()
    {
        
        //TODO: 추후 마우스 감도가 들어갈 자리 GetAxis * 마우스 감도
        _mouseX += Input.GetAxisRaw("Mouse X");
        _mouseY += Input.GetAxisRaw("Mouse Y");

        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
        
        //캐릭터 몸체 회전
        transform.rotation = Quaternion.Euler(0, _mouseX, 0f);
        
        //카메라 상하/좌우 회전
        _cam.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0f);
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
