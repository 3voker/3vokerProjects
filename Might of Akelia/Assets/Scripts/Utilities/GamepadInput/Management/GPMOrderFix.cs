using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class GPMOrderFix : Editor
{
    static GPMOrderFix()
    {
       // SetExecutionOrderToBeFirst();
    }

    private static void SetExecutionOrderToBeFirst()
    {
        string[] gamepadManagerAssets = AssetDatabase.FindAssets("GamepadManager");
        string gamepadManager = AssetDatabase.GUIDToAssetPath(gamepadManagerAssets[0]);
        MonoScript monoScript =  AssetDatabase.LoadAssetAtPath(gamepadManager, typeof(MonoScript)) as MonoScript;

        int currentExecutionOrder = MonoImporter.GetExecutionOrder(monoScript);

        if (currentExecutionOrder != -1)  {  MonoImporter.SetExecutionOrder(monoScript, -1); }
    }

}
