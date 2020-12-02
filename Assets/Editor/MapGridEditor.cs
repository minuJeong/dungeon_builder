using Assets.Scripts.Data.Map;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGrid))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Reset"))
        {
            (target as MapGrid).Reset();
        }
    }
}
