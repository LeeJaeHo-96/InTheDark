using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform HeadPos;

    private Camera _camera;
    private PlayerController _playerController;
    private Ray _ray;
    private RaycastHit _hit;
    private Vector3 _camPos;
    private Quaternion _rot;

    private int _layerMask;
    private float _mouseX;
    private float _mouseY;
    private float _camDistance = 2f;

    private void Awake()
    {
        _layerMask = 1 << 17 | 1 << 18;
        _layerMask = ~_layerMask;

        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        TPSInput();
    }

    private void LateUpdate()
    {
        FPS();
        TPS();
    }

    /// <summary>
    /// 1인칭 시점 카메라 동작
    /// </summary>
    private void FPS()
    {
        if (_playerController.CurState == PState.DEATH) return;

        if (_camera.nearClipPlane > 0.01f)
        {
            _camera.nearClipPlane = 0.01f;
        }

        transform.position = Vector3.Lerp(transform.position, HeadPos.transform.position, 5f * Time.deltaTime);
    }

    /// <summary>
    /// 3인칭 카메라 시점 조작
    /// </summary>
    private void TPSInput()
    {
        if (_playerController.CurState != PState.DEATH) return;


        //카메라가 캐릭터를 비출 위치
        _camPos = _playerController.ObserverPos - transform.forward * _camDistance;

        //마우스 회전 값 
        _mouseX += Input.GetAxisRaw("Mouse X");
        _mouseY -= Input.GetAxisRaw("Mouse Y");

        //상,하 회전 값 제한
        _mouseY = Mathf.Clamp(_mouseY, -20f, 50f);

        //카메라 회전시킬 Quaternion
        _rot = Quaternion.Euler(_mouseY, _mouseX, 0f);

        //카메라는 캐릭터를 바라보도록
        transform.LookAt(_playerController.ObserverPos);

        //캐릭터 -> 카메라 방향 벡터
        Vector3 _camDir = transform.position - _playerController.ObserverPos;

        //캐릭터 위치 -> 카메라 위치로 Ray를 쏨
        if (Physics.Raycast(_playerController.ObserverPos, _camDir.normalized, out _hit, _camDir.magnitude,
                _layerMask))
        {
            //레이가 충돌한 위치에서 캐릭터 위치를 뺀 후 카메라의 거리를 재조정
            Vector3 distance = _hit.point - _playerController.ObserverPos;
            _camDistance = (distance.magnitude * 1f);
        }
        else
        {
            //충돌 감지가 안됐다면 원래 거리로 변경
            _camDistance = 2f;
        }
    }

    /// <summary>
    /// 3인칭 시점 카메라 동작
    /// </summary>
    private void TPS()
    {
        if (_playerController.CurState != PState.DEATH) return;

        transform.position = Vector3.Lerp(transform.position, _camPos, 5f * Time.deltaTime);
        transform.rotation = _rot;
    }
}