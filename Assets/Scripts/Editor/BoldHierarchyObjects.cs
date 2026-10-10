using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class BoldHierarchyObjects
{
    static BoldHierarchyObjects()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.EntityIdToObject(instanceID) as GameObject;
        if (obj == null) return;

        Color color;
        var alignement = TextAnchor.MiddleCenter;
        switch (obj.tag)
        {
            case "Header":
                color = Color.black;
                alignement = TextAnchor.MiddleCenter;
                break;
            case "Important":
                color = Color.blue;
                alignement = TextAnchor.MiddleLeft;
                
                break;
            default:
                return;
        }

        EditorGUI.DrawRect(selectionRect, color);

        var style = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            alignment = alignement,
        };

        EditorGUI.LabelField(selectionRect, $"{obj.name}", style);
    }
}
