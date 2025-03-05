using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(InteractableItemData))]
public class GUITest : Editor
{
    private InteractableItemData value;

    private void OnEnable()
    {
        value = (InteractableItemData)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        value.ItemIcon = (Sprite)EditorGUILayout.ObjectField("아이콘 이미지", value.ItemIcon, typeof(Sprite), true);

        EditorGUILayout.Space();
        GUILine(4);
        EditorGUILayout.Space();
        base.OnInspectorGUI();
        EditorGUILayout.EndVertical();
    }

    private void GUILine(int lineHeight = 1)
    {
        EditorGUILayout.Space();
        Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
        rect.height = lineHeight;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space();
    } 
}

#endif

[CreateAssetMenu(menuName = "Data/CreateInteractableItem")]
public class InteractableItemData : ScriptableObject
{
    [Header("아이템 ID")] 
    public int ItemID;
    
    [Header("아이템 무게")]
    public float ItemWeight;
    
    [Header("한손/양손 타입")]
    public ItemHoldingType ItemHoldingType;

    [Header("아이템 이름")] 
    public string ItemName;

    [Header("아이템 아이콘 이미지")] 
    public Sprite ItemIcon;

    [Header("공격 가능 여부")] 
    public bool IsAttack;

    [Header("소모품 여부")] 
    public bool IsConsumable;

    [Header("소모품의 내구도")] 
    public float MaxDurability;

    [Header("공격 속도")] 
    public float AttackSpeed;

    [Header("공격 범위")] 
    public float AttackRange;

}
