using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SavedData))]
public class SavedDataEditor : Editor
{
    private int _dataIndex = 0;
    private bool _thereAreDataObject;
    private string[] _datasName;
    private List<Type> _types = new List<Type>();
    private SavedData _savedData;
    private SerializedProperty _datas;

    private void OnEnable()
    {
        _savedData = target as SavedData;
        _datas = serializedObject.FindProperty(nameof(_datas));
        _types = AppDomain.CurrentDomain.GetAllDerivedTypes<ScriptableObject>();

        if (_types.Count > 0)
        {
            _datasName = _types.Where(type => type != typeof(SavedData)).ToList().ConvertAll(type => type.Name).ToArray();

            if (_savedData.typeToFind != null && _datasName.Contains(_savedData.typeToFind.Name))
                _dataIndex = _types.IndexOf(_savedData.typeToFind);

            _thereAreDataObject = true;
        }
        else
            _thereAreDataObject = false;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        if (!_thereAreDataObject)
            EditorGUILayout.LabelField("Project not contains Data Objects");
        else
        {
            _dataIndex = EditorGUILayout.Popup("Seach Data of Type", _dataIndex, _datasName);
            _savedData.typeToFind = _types[_dataIndex];
        }

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        if (GUILayout.Button("Find all Savable"))
        {
            DataObject[] dataObjects = GetAllScriptables();
            _savedData.data = new Data[dataObjects.Length];

            for (int i = 0; i < dataObjects.Length; i++)
                _savedData.data[i] = new Data(dataObjects[i]);
        }

        if (GUILayout.Button("Clear Savable"))
            _savedData.data = new Data[0];

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
        EditorGUILayout.PropertyField(_datas);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        if (GUILayout.Button("Serialize"))
            _savedData.Serialize();

        if (GUILayout.Button("Overwrite"))
            _savedData.Overwrite();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

        if (GUILayout.Button("Save"))
            _savedData.Save();

        if (GUILayout.Button("Load"))
            _savedData.Load();

        serializedObject.ApplyModifiedProperties();
    }

    private DataObject[] GetAllScriptables()
    {
        string[] typeIDs = AssetDatabase.FindAssets($"t:{_savedData.typeToFind.Name}", new string[] { "Assets" });

        DataObject[] result = new DataObject[typeIDs.Length];

        for (int i = 0; i < typeIDs.Length; i++)
            result[i] = AssetDatabase.LoadAssetAtPath<DataObject>(AssetDatabase.GUIDToAssetPath(typeIDs[i]));

        return result;
    }
}