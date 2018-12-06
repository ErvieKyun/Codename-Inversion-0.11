using UnityEditor;
using UnityEngine;
using UnityEditor.UI;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Platform))]
[CanEditMultipleObjects]

public class PlatformEditor : Editor
{
    SerializedProperty pathProp;
    SerializedProperty delayProp;
    SerializedProperty speedProp;
    SerializedProperty rotateProp;
    SerializedProperty easeProp;
    int selectedEase;
    string[] easeOptions = new string[]{};

    void OnEnable()
    {
        pathProp = serializedObject.FindProperty("pathPoint");
        delayProp = serializedObject.FindProperty("delays");
        speedProp = serializedObject.FindProperty("speed");
        easeProp = serializedObject.FindProperty("ease");
        easeOptions = System.Enum.GetNames(typeof(iTween.EaseType));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Label("Platform Settings");
        speedProp.floatValue = EditorGUILayout.FloatField("Speed", speedProp.floatValue);
        selectedEase = EditorGUILayout.Popup("Label", selectedEase, easeOptions);
        easeProp.intValue = selectedEase;
        EditorGUILayout.Separator();
        GUILayout.Label("Waypoint List");
        if (delayProp.arraySize != pathProp.arraySize)
            delayProp.arraySize = pathProp.arraySize;
        for (int i = 0; i < pathProp.arraySize; i++)
        {
            SerializedProperty delay = delayProp.GetArrayElementAtIndex(i);
            SerializedProperty obj = pathProp.GetArrayElementAtIndex(i);
            EditorGUILayout.ObjectField(obj, new GUIContent("Pos " + i.ToString()));
            delay.floatValue = EditorGUILayout.Slider(delay.floatValue, 0, 5f);
            EditorGUILayout.Separator();
        }
        if (GUILayout.Button("Add"))
        {
            int pos = pathProp.arraySize;
            pathProp.InsertArrayElementAtIndex(pos);
            SerializedProperty tProp = pathProp.GetArrayElementAtIndex(pos);
            GameObject point = new GameObject();
            point.name = serializedObject.targetObject.name + " waypoint " + pos;
            tProp.objectReferenceValue = point;
        }
        if (GUILayout.Button("Delete"))
        {
            if (pathProp.arraySize > 0)
            {
                SerializedObject obj = pathProp.GetArrayElementAtIndex(pathProp.arraySize - 1).serializedObject;
                DestroyImmediate(obj.targetObject);
                pathProp.DeleteArrayElementAtIndex(pathProp.arraySize - 1);
                return;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}