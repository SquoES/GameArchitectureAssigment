using System;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerInfo
{
    private readonly Dictionary<int, StatValue> _statValues = new Dictionary<int, StatValue>();
    internal void GetStatValuesKeys(out Dictionary<int, StatValue>.KeyCollection keys)
    {
        keys = _statValues.Keys;
    }
    internal EventHandler<int> OnStatUpdated;

    private List<Buff> _buffs; 
    internal EventHandler<BuffsEventArgs> BuffsUpdated;
    internal class BuffsEventArgs : EventArgs
    {
        internal bool Enable;
        internal int Id;

        internal BuffsEventArgs(bool enable, int setId)
        {
            Enable = enable;
            Id = setId;
        }
    }

    internal void Init(Stat[] stats, Buff[] buffs)
    {
        List<int> setId = new List<int>();
        for (int i = 0; i < stats.Length; i++)
        {
            if (!setId.Contains(stats[i].id)) setId.Add(stats[i].id);
            SetupStat(stats[i], false);
        }
        
        if (_buffs != null)
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                BuffsUpdated.Invoke(this, new BuffsEventArgs(false, _buffs[i].id));
            }

            _buffs.Clear();
        }
        else
        {
            _buffs = new List<Buff>();
        }
        if (buffs != null && buffs.Length > 0)
        {
            for (int i = 0; i < buffs.Length; i++)
            {
                SetupBuff(buffs[i], false);
            }
        }

        for (int i = 0; i < setId.Count; i++)
        {
            OnStatUpdated.Invoke(this, setId[i]);
        }
    }

    #region Stat
    internal void SetupStat(Stat stat, bool notify = true)
    {
        bool exists = _statValues.TryGetValue(stat.id, out StatValue statValue);
        if (exists) statValue.Set(stat.value);
        else _statValues.Add(stat.id, new StatValue(stat.value));
        
        if (notify) OnStatUpdated.Invoke(this, stat.id);
    }
    internal StatValue GetStatValue(int id)
    {
        _statValues.TryGetValue(id, out StatValue value);
        return value;
    }

    internal void ChangeMaxValue(int id, float deltaValue, bool notify = true)
    {
        bool exists = _statValues.TryGetValue(id, out StatValue statValue);
        if (exists)
        {
            statValue.ChangeMax(deltaValue);
            if (notify) OnStatUpdated.Invoke(this, id);
        }
    }
    //Control current stat's value
    internal void ChangeCurrentValue(int id, float deltaValue)
    {
        bool exists = _statValues.TryGetValue(id, out StatValue statValue);
        if (!exists) return;
        statValue.ChangeCurrent(deltaValue);
        OnStatUpdated.Invoke(this, id);
    }
    #endregion
    
    #region Buff
    internal void SetupBuff(Buff buff, bool notify = true)
    {
        bool addBuff = false;
        for (int i = 0; i < buff.stats.Length; i++)
        {
            bool exists = _statValues.ContainsKey(buff.stats[i].statId);
            if (exists) ChangeMaxValue(buff.stats[i].statId, buff.stats[i].value, notify);
            else continue;
                    
            addBuff = true;
        }
        if (!addBuff) return;
        _buffs.Add(buff);
        BuffsUpdated.Invoke(this, new BuffsEventArgs(true, buff.id));
    }
    internal void GetBuff(int id, out Buff buff)
    {
        buff = null;
        if (_buffs.Count > id && _buffs[id] != null)
        {
            buff = _buffs[id];
        }
    }
    #endregion

}

internal class StatValue
{
    private float max;
    internal float Max => max;
    private float current;
    internal float Current => current;

    internal StatValue(float maxValue)
    {
        max = maxValue;
        current = max;
    }

    internal void Set(float value)
    {
        max = value;
        current = max;
    }

    internal void ChangeMax(float deltaValue)
    {
        max += deltaValue;
        current += deltaValue;
        current = Mathf.Min(current, max);
    }
    internal void ChangeCurrent(float deltaValue)
    {
        current += deltaValue;
        current = Mathf.Clamp(current, 0f, max);
    }
}
