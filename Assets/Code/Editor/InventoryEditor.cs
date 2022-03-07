#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomInventory))]
public class InventoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomInventory inventory = (CustomInventory)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Fill inventory"))
        {
            inventory.FillInventory();
            inventory.ShowInventory();
        }
        if (GUILayout.Button("Fill ui"))
        {
            InventoryDraft.GetInstance().Reset();
            inventory.FillUIInventory();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Add Items to inventory"))
        {
            inventory.UpdateInventory();
            inventory.ShowInventory();
        }

        if (GUILayout.Button("Show inventory"))
        {
            inventory.ShowInventory();
        }

        if (GUILayout.Button("Clear inventory"))
        {
            inventory.Clear();
            inventory.ShowInventory();
        }

        if (GUILayout.Button("Light ON/OFF"))
        {
            inventory.LightOnOFF();
        }

        if (GUILayout.Button("Reset draft"))
        {
            InventoryDraft.GetInstance().Reset();
        }

        if (GUILayout.Button("Show crafting list"))
        {
            inventory.ShowCraftingList();
        }

        if (GUILayout.Button("Show inventory draft"))
        {
            inventory.ShowInventoryDraft();
        }

    }
}
#endif
