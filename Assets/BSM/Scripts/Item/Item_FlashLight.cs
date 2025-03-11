using System.Collections;
using System.Collections.Generic;
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
        _flashLight.enabled = isPower && _battery >= 0f;
        
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
    
    //TODO: 슬롯 체인지 하면 배터리가 초기화 되는데 이거 수정해야함
    
    private IEnumerator FlashLightRoutine()
    {
        while (isPower && _battery >= 0)
        {
            _battery -= Time.deltaTime; 
            Debug.Log(_battery);
            if (!isPower || _battery < 0f)
            {
                _flashLight.enabled = false;
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
        _flashLight.enabled = isPower;

        if (_flashCo != null)
        {
            StopCoroutine(_flashCo);
            _flashCo = null;
        }
        
    }
    
}
