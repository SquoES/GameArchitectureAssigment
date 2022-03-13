using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

internal class PlayerPanelHierarchy : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    internal UnityEvent OnAttack = new UnityEvent(); //On attack button pressed
    
    [SerializeField] private Transform root_Info;

    [SerializeField] private AssetReference ref_InfoPanel; //Ref to stat/buff panel info
    private const string path_Icons = "Icons/"; //Path to icons folder in Resources

    private Dictionary<int, PlayerPanelStat> _statsCollection; //Current stat panels 
    private Dictionary<int, PlayerPanelStat> _buffsCollection;//Current buff panel

    private PlayerInfo _playerInfo;

    private void Start()
    {
        attackButton.onClick.AddListener(() => OnAttack.Invoke());
    }

    internal void Init(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;
        UpdateStats();
        _playerInfo.OnStatUpdated += UpdateStat;
        _playerInfo.BuffsUpdated += UpdateBuff;
    }
    //Get stats on Init
    private void UpdateStats()
    {
        _playerInfo.GetStatValuesKeys(out Dictionary<int, StatValue>.KeyCollection keys);
        foreach (var key in keys)
        {
            UpdateStat(this, key);
        }
    }
    //Add/change Stat panel
    private void UpdateStat(object sender, int id)
    {
        if (_statsCollection == null)
        {
            _statsCollection = new Dictionary<int, PlayerPanelStat>();
        }

        float value = _playerInfo.GetStatValue(id).Current;
        int intValue = (int)Math.Ceiling(value);
        
        bool exists = _statsCollection.TryGetValue(id, out PlayerPanelStat playerStat);
        if (!exists)
        {
            CoroutineHandler.Start(Loader.ADRLoadGameObject(ref_InfoPanel, handle =>
            {
                playerStat = (handle.Result as GameObject).GetComponent<PlayerPanelStat>();
                _statsCollection.Add(id, playerStat);
                
                GameManager.Instance.SettingsData.GetPlayerStat(id, out Stat stat);
                string iconPath = path_Icons + stat.icon;
                playerStat.Init(handle, iconPath, intValue.ToString());
            }, root_Info));
        }
        else
        {
            playerStat.SetData(intValue.ToString());
        }
    }
    
    //Add/remove/change Buff panel
    private void UpdateBuff(object sender, PlayerInfo.BuffsEventArgs args)
    {
        if (_buffsCollection == null)
        {
            _buffsCollection = new Dictionary<int, PlayerPanelStat>();
        }
        bool exists = _buffsCollection.TryGetValue(args.Id, out PlayerPanelStat panel);
        switch (args.Enable)
        {
            case true:
                _playerInfo.GetBuff(args.Id, out Buff buff);
                if (buff == null) return;
                if (exists)
                {
                    panel.SetIcon(path_Icons + buff.icon);
                    panel.SetData(buff.title);
                }
                else
                {
                    CoroutineHandler.Start(Loader.ADRLoadGameObject(ref_InfoPanel, handle =>
                    {
                        panel = (handle.Result as GameObject).GetComponent<PlayerPanelStat>();
                        _buffsCollection.Add(args.Id, panel);

                        string iconPath = path_Icons + buff.icon;
                        panel.Init(handle, iconPath, buff.title);
                    }, root_Info));
                }
                break;
            case false:
                if (exists)
                {
                    _buffsCollection.Remove(args.Id);
                    panel.Release();
                }
                break;
        }
    }
}
