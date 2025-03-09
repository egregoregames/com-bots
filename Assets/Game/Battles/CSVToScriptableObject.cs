using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Game.Battles;
using UnityEditor;
public static class CSVToScriptableObject
{
    public static string csvFileName = "Game/Battles/Attacks.csv"; // Place this file in StreamingAssets or another known location
    public static string outputPath = "Game/Battles/Attacks"; // Folder to save generated SOs
    
    [MenuItem("Tools/Convert CSV to Bot Attacks")]
    static void DoIt()
    {
        CreateScriptableObjectsFromCSV();
    }
    static void CreateScriptableObjectsFromCSV()
    {
        string path = Path.Combine(Application.dataPath, csvFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("CSV file not found at: " + path);
            return;
        }
        string[] lines = File.ReadAllLines(path);
        if (lines.Length < 2) // Ensure there is data
        {
            Debug.LogError("CSV file is empty or only has headers.");
            return;
        }
        // Make sure the output directory exists
        string fullPath = Path.Combine(Application.dataPath, outputPath);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        for (int i = 1; i < lines.Length; i++) // Skip header
        {
            string[] values = lines[i].Split(',');

            try
            {
                // Adjust this based on your ScriptableObject properties
                BotAttack obj = ScriptableObject.CreateInstance<BotAttack>();
                obj.name = values[0]; // Assuming the first column is the name
                // i is immage
                obj.Aura = (AuraType)Enum.Parse(typeof(AuraType), values[2]);
                obj.RamCost = float.Parse(values[3]);
                obj.Power = int.Parse(values[4]);
                var accParse = float.TryParse(values[5], out float objAccuracy);
                if (accParse)
                    obj.Accuracy = objAccuracy;

                obj.EnergyCost = int.Parse(values[6]);
                obj.Priority = (Priority)Enum.Parse(typeof(Priority), values[7]);
                obj.TargetType = (TargetType)Enum.Parse(typeof(TargetType), values[8]);
                obj.EffectDescription = values[9];
                obj.MethodOfObtaining = values[10];

                // Save as asset
                string assetPath = $"Assets/{outputPath}/{obj.name}.asset";
                AssetDatabase.CreateAsset(obj, assetPath);
            }
            catch
            {
                Debug.Log($"Failed on: {values[0]}");
            }
            
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Scriptable Objects created successfully!");
    }
}