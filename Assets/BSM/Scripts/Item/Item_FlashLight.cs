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
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
    }

    [PunRPC]
    private void SyncLight(bool isOn)
    {
        _flashLight.enabled = isOn;
    }
    
    //TODO: 슬롯 체인지 하면 배터리가 초기화 되는데 이거 수정해야함
    
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
    
    protected override void ItemRestore()
    {
        _battery = _itemData.MaxDurability;
    }

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
