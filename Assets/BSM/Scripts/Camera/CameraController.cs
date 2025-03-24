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
        if (_playerController.CurState == PState.DEATH)
        {
            _camPos = _playerController.ObserverPos - transform.forward * _camDistance;

            _mouseX += Input.GetAxisRaw("Mouse X");
            _mouseY -= Input.GetAxisRaw("Mouse Y");
            _mouseY = Mathf.Clamp(_mouseY, -20f, 50f);
            _rot = Quaternion.Euler(_mouseY, _mouseX, 0f);
            
            transform.LookAt(_playerController.ObserverPos);
            
            Vector3 _camDir = transform.position - _playerController.ObserverPos;
            Debug.DrawRay(_playerController.ObserverPos, _camDir.normalized * _camDir.magnitude, Color.red);

            if (Physics.Raycast(_playerController.ObserverPos, _camDir.normalized, out _hit, _camDir.magnitude,
                    _layerMask))
            {
                Vector3 distance = _hit.point - _playerController.ObserverPos;
                _camDistance = (distance.magnitude * 1f);
            }
            else
            {
                _camDistance = 2f;
            }
            
        }
        
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
    /// 3인칭 시점 카메라 동작
    /// </summary>
    private void TPS()
    {
        if (_playerController.CurState != PState.DEATH) return; 

        transform.position = Vector3.Lerp(transform.position, _camPos, 5f * Time.deltaTime);
        transform.rotation = _rot; 
    }
}
