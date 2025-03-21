using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    
    [Header("아이템 데이터 할당")]
    [SerializeField] public List<InteractableItemData> _itemDataList;
    
    private Dictionary<int, InteractableItemData> _itemDict = new Dictionary<int, InteractableItemData>();
     
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (InteractableItemData item in _itemDataList)
        {
            if(_itemDict.ContainsKey(item.ItemID))
            {
                item.ItemID = NewItemID();
            }
            
            _itemDict.Add(item.ItemID, item);
        }
    }

    private void OnValidate()
    {
        if (_itemDataList.Count == 0)
        {
            Debug.LogError("아이템 데이터를 할당하세요.");
        }
    }

    /// <summary>
    /// 중복 ID 방지
    /// </summary>
    /// <returns></returns>
    private int NewItemID()
    {
        return _itemDict.Keys.Max() + 1;
    }
    
    /// <summary>
    /// ItemID에 맞는 ItemData 전달
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public InteractableItemData GetItemData(int itemID)
    { 
        if (_itemDict.TryGetValue(itemID, out InteractableItemData itemData))
        {
            return itemData;
        }
        
        return null;
    }
    
}
