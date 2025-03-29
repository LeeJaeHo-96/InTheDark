using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class Item_FlashLight : Item
{
    public Light FlashLight;
    public GameObject FlashLightObject;
    
    private Coroutine _flashCo;

    private void OnDisable()
    {
        ItemDrop();
    }

    public override void SetItemHoldPosition(Transform holdPos, float mouseX, float mouseY)
    {
        transform.position = holdPos.position; 
        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }
    
    /// <summary>
    /// 아이템 사용
    /// </summary>
    public override void ItemUse()
    {
        isPower = !isPower;
        photonView.RPC(nameof(SyncLight),RpcTarget.AllViaServer, isPower && _battery >= 0f);

        string onOff = isPower switch
        {
            true => "FlashLightOnSFX",
            false => "FlashLightOffSFX", 
        };
        
        _soundManager.PlaySfx(_soundManager.SoundDatas.SoundDict[onOff]);
        
        if (_flashCo == null)
        {
            _flashCo = StartCoroutine(FlashLightRoutine());
        }
        else
        { 
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
    }

    [PunRPC]
    private void SyncLight(bool isOn)
    {
        FlashLight.enabled = isOn;
        FlashLightObject.SetActive(isOn);
    }
     
    private IEnumerator FlashLightRoutine()
    {
        while (isPower && _battery >= 0)
        {
            _battery -= Time.deltaTime; 
            if (!isPower || _battery < 0f)
            {
                photonView.RPC(nameof(SyncLight),RpcTarget.AllViaServer, false);
                yield break;
            }

            yield return null;
        } 
    }

    /// <summary>
    /// 남은 배터리 동기화
    /// </summary>
    /// <param name="battery"></param>
    [PunRPC]
    private void SyncBatteryRPC(float battery)
    {
        _battery = battery;
    }
    
    /// <summary>
    /// 손전등 아이템 충전
    /// </summary>
    protected override void ItemRestore()
    {
        _battery = _itemData.MaxDurability;
    }

    /// <summary>
    /// 아이템 버렸을 때 동작
    /// </summary>
    protected override void ItemDrop()
    {
        isPower = false; 
        photonView.RPC(nameof(SyncLight),RpcTarget.AllViaServer, false);
        
        //배터리 공유
        photonView.RPC(nameof(SyncBatteryRPC), RpcTarget.AllViaServer, _battery);
        
        if (_flashCo != null)
        {
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
        
    }
    
}
