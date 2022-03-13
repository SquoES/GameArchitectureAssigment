using System;
using UnityEngine;

[Serializable]
internal class Data
{
    [SerializeField] internal CameraModel cameraSettings;
    [SerializeField] internal GameModel settings;
    [SerializeField] internal Stat[] stats;
    [SerializeField] internal Buff[] buffs;
}

[Serializable]
internal class CameraModel
{
    [SerializeField] internal float roundDuration;
    [SerializeField] internal float roundRadius;
    [SerializeField] internal float height;
    [SerializeField] internal float lookAtHeight;
    [SerializeField] internal float roamingRadius;
    [SerializeField] internal float roamingDuration;
    [SerializeField] internal float fovMin;
    [SerializeField] internal float fovMax;
    [SerializeField] internal float fovDelay;
    [SerializeField] internal float fovDuration;
}

[Serializable]
internal class GameModel
{
    [SerializeField] internal int playersCount;
    [SerializeField] internal int buffCountMin;
    [SerializeField] internal int buffCountMax;
    [SerializeField] internal bool allowDuplicateBuffs;

}

[Serializable]
internal class Stat
{
    [SerializeField] internal int id;
    [SerializeField] internal string title;
    [SerializeField] internal string icon;
    [SerializeField] internal float value;
}

[Serializable]
internal class BuffStat
{
    [SerializeField] internal float value;
    [SerializeField] internal int statId;
}

[Serializable]
internal class Buff
{
    [SerializeField] internal string icon;
    [SerializeField] internal int id;
    [SerializeField] internal string title;
    [SerializeField] internal BuffStat[] stats;
}
