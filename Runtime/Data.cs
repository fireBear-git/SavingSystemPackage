using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Data
{
    [SerializeField] private ScriptableObject _dataObject;
    [SerializeField] private string _json;

    public Data(DataObject dataObject)
    {
        _dataObject = dataObject;
        _json = JsonUtility.ToJson(_dataObject);
    }

    public void Serialize()
    {
        _json = JsonUtility.ToJson(_dataObject);
    }

    public void Overwrite()
    {
        JsonUtility.FromJsonOverwrite(_json, _dataObject);
    }

    public bool Equals(DataObject dataObject)
    {
        return _dataObject == dataObject;
    }
}
