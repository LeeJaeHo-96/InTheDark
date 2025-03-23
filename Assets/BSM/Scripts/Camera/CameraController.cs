using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsCameraBlocked;
    public Transform HeadPos;
    
    private Camera _camera;
    private PlayerController _playerController;
    private Ray _ray;
    private RaycastHit _hit;
    private int _layerMask;
    private float _distance = 0.5f;
     
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
        _ray = new Ray(transform.position, transform.forward);
        
        Debug.DrawRay(_ray.origin, _ray.direction * _distance, Color.magenta);

        if (Physics.Raycast(_ray.origin, _ray.direction, out _hit ,_distance, _layerMask))
        {
            if (_hit.collider != null)
            {
                IsCameraBlocked = true; 
            } 
        }
        else
        {
            IsCameraBlocked = false;
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
        
    }
}
