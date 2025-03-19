using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Item_FlashLight : Item
{
    private Coroutine _flashCo;
    
    private Light _flashLight;
    
    protected override void Awake()
    {
        base.Awake();
        _flashLight = GetComponentInChildren<Light>(); 
    }
    
    /// <summary>
    /// 아이템 사용
    /// </summary>
    public override void ItemUse()
    {
        isPower = !isPower;
        photonView.RPC(nameof(SyncLight),RpcTarget.AllViaServer, isPower && _battery >= 0f);
        
        if (_flashCo == null)
        {
            _flashCo = StartCoroutine(FlashLightRoutine());
        }
        else
        { 
            //배터리 공유
            photonView.RPC(nameof(SyncBatteryRPC), RpcTarget.AllViaServer, _battery);
            
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
    }

    [PunRPC]
    private void SyncLight(bool isOn)
    {
        _flashLight.enabled = isOn;
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
        
        if (_flashCo != null)
        {
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
        
    }
    
}
