using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataHolder : ScriptableObject 
{
    [SerializeField] protected Data[] _datas;

    public Data[] data
    {
        get => _datas;

#if UNITY_EDITOR
        set => _datas = value;
#endif
    }

    public void Serialize()
    {
        for (int i = 0; i < _datas.Length; i++)
            _datas[i].Serialize();
    }

    public void Overwrite()
    {
        for (int i = 0; i < _datas.Length; i++)
            _datas[i].Overwrite();
    }
}
