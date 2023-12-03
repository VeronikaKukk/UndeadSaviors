using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow
{
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/DEBUG WINDOW")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(DebugWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("In-Game Commands", EditorStyles.boldLabel);
        
        if (GUILayout.Button("+50 Courage"))
        {
            Events.SetMoney(Events.GetMoney() + 50);
        }
        if (GUILayout.Button("+15 seconds"))
        {
            CountdownTimer.Instance.currentTime += 15f;
        }
        if (GUILayout.Button("Reset Level"))
        {
            ScenarioController.Instance.ResetLevel();
        }
        /*
        GUILayout.Label("Spawn Potions", EditorStyles.boldLabel);

        if (GUILayout.Button("Spawn Attack Potion"))
        {
            Health.Instance.ManualPotionSpawn(0);
        }
        if (GUILayout.Button("Spawn Health Potion"))
        {
            Health.Instance.ManualPotionSpawn(1);
        }
        if (GUILayout.Button("Spawn Speed Potion"))
        {
            Health.Instance.ManualPotionSpawn(2);
        }
        */
    }

}