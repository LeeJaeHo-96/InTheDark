using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public string email;
    public Dictionary<string, SlotData> slots;

    public PlayerData()
    {
        slots = new Dictionary<string, SlotData>(); 
    }
}

[System.Serializable]
public class SlotData
{
    public int money;
    public int day;

    public SlotData(int money, int day)
    {
        this.money = money;
        this.day = day;
    }
}

