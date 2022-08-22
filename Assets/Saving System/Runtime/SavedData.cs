using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Saving System/Create SavedData", fileName = "New SavedData")]
public class SavedData : DataHolder
{
#if UNITY_EDITOR

    public Type typeToFind;

#endif

    public Func<string, string> onCipherSerialize;
    public Func<string, string> onCipherDeserialize;

    private string path => $"{(Application.isEditor ? Application.dataPath : Application.persistentDataPath)}/data.sv";

    private bool canCrypt => onCipherSerialize != null && onCipherDeserialize != null;

    public void Save()
    {
        Serialize();

        string json = JsonUtility.ToJson(this);

        using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                if (canCrypt)
                    json = onCipherSerialize(json);

                writer.Write(json);
            }
        }
    }

    public void Load()
    {
        string json = "";

        if (!File.Exists(path))
        {
            Save();
            return;
        }

        using (FileStream FileStream = new FileStream(path, FileMode.Open))
        {
            using (BinaryReader reader = new BinaryReader(FileStream))
            {
                json = reader.ReadString();
                
                if (canCrypt)
                    json = onCipherDeserialize(json);
            }
        }

        JsonUtility.FromJsonOverwrite(json, this);
        Overwrite();
    }
}