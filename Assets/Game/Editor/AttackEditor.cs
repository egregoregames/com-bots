using Game.Battles;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BotAttack))]
public class AttackEditor : Editor
{
    // public override void OnInspectorGUI()
    // {
    //     BotAttack myTarget = (BotAttack)target;
    //
    //     // Draw the default Inspector GUI but with control over visibility
    //     myTarget.showAdvancedSettings = EditorGUILayout.Toggle("Show Advanced Settings", myTarget.showAdvancedSettings);
    //
    //     // Always visible field
    //     EditorGUILayout.PropertyField(serializedObject.FindProperty("alwaysVisibleField"));
    //
    //     // Conditionally visible fields
    //     if (myTarget.showAdvancedSettings)
    //     {
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("hiddenField"));
    //         EditorGUILayout.PropertyField(serializedObject.FindProperty("anotherHiddenField"));
    //     }
    //
    //     serializedObject.ApplyModifiedProperties();
    //
    //     
    //     
    //     
    // }
}