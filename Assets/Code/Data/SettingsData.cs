#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create game data/Settings data", fileName = "Game settings")]
internal class SettingsData : ScriptableObject
{
    [SerializeField] private string file_Name = "data";
    [SerializeField] private string file_Extension = "txt";
    [SerializeField] private Data _data;
    internal void Init()
    {
        CollectData();
    }

    internal void GetCameraSettings(out CameraModel model)
    {
        if (_data == null) CollectData();
        model = _data.cameraSettings;
    }

    internal Stat[] GetPlayerStats()
    {
        return _data.stats;
    }
    internal void GetPlayerStat(int statId, out Stat stat)
    {
        stat = null;
        for (int i = 0; i < _data.stats.Length; i++)
        {
            if (_data.stats[i].id == statId)
            {
                stat = _data.stats[i];
                return;
            }
        }
    }

    internal Buff[] GetPlayerBuffs() => GenerateBuffs();

    private Buff[] GenerateBuffs()
    {
        Buff[] buffs = null;
        int uniqueBuffsCount = _data.buffs.Length;
        if (uniqueBuffsCount < 1)
        {
            Debug.LogError("No buffs added to settings");
            return buffs;
        }

        int maxBuffs = _data.settings.allowDuplicateBuffs
            ? _data.settings.buffCountMax
            : Mathf.Min(uniqueBuffsCount, _data.settings.buffCountMax);
        int minBuffs = _data.settings.buffCountMin;

        int buffsCount = Random.Range(minBuffs, maxBuffs);
        buffs = new Buff[buffsCount];
        
        List<int> buffsList = new List<int>();
        for (int i = 0; i < uniqueBuffsCount; i++)
        {
            buffsList.Add(i);
        }

        for (int i = 0; i < buffsCount; i++)
        {
            int listId = Random.Range(0, buffsList.Count);
            int buffId = buffsList[listId];
            buffs[i] = _data.buffs[buffId];
            
            if (_data.settings.allowDuplicateBuffs) continue;
            buffsList.RemoveAt(listId);
        }
        return buffs;
    }
    
    private void CollectData()
    {
        TextAsset textAsset = (TextAsset) Resources.Load(file_Name, typeof(TextAsset));
        _data = JsonUtility.FromJson<Data>(textAsset.text);
    }

#if UNITY_EDITOR
    private bool _hasBeenInitialised = false;
    internal void BuildJsonFile()
    {
        string newText = JsonUtility.ToJson(_data);
        
        string path = System.String.Format("Assets/Resources/{0}.{1}", file_Name, file_Extension);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(newText);
        writer.Close();

        AssetDatabase.ImportAsset(path);
    }
    internal void OnValidate()
    {
        if (!_hasBeenInitialised)
        {
            _hasBeenInitialised = true;
            
            CollectData();
        }
    }
    internal void RefreshDataFromFile() => CollectData();
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SettingsData))]
internal class SettingsDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SettingsData targetScript = (SettingsData)target;
        
        if(GUILayout.Button("Build file"))
        {
            targetScript.BuildJsonFile();
        }
        
        if(GUILayout.Button("Refresh data from file"))
        {
            targetScript.RefreshDataFromFile();
        }
    }
}
#endif
