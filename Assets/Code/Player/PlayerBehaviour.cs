using System;
using UnityEngine;

//Player's main class
internal class PlayerBehaviour : MonoBehaviour
{
    private PlayerHealth _health; 
    internal bool isAlive => _health.IsAlive;
    
    private PlayerMovement _movement; 
    internal EventHandler<float> OnAttack;
    
    private Animator _animator;
    internal Animator Animator => _animator;
    
    [SerializeField] private PlayerPanelHierarchy _playerPanel;

    private PlayerInfo _playerInfo;
    internal PlayerInfo PlayerInfo => _playerInfo;

    #region Cross players communication
    internal void SetDamageTarget(PlayerBehaviour player) => 
        _movement.SetDamageTarget(player);
    
    //This player start Attack
    internal void Attack()
    {
        _movement.Attack(out float damageSet);
        OnAttack.Invoke(this, damageSet);
    }
    
    //This player take Damage
    internal void Damage(float damage, out float damageTaken) =>
        _health.TakeDamage(damage, out damageTaken);
    #endregion

    #region PlayerInitialization
    private void Start()
    {
        _playerInfo = new PlayerInfo();

        _animator = GetComponentInChildren<Animator>();
        
        _health = GetComponentInChildren<PlayerHealth>();
        if (_health)
        {
            _health.Init(this);
        }
        else
        {
            Debug.LogError("Add player health to object");
        }

        _movement = GetComponentInChildren<PlayerMovement>();
        if (_movement)
        {
            _movement.Init(this);
        }
        else
        {
            Debug.LogError("Add player movement to object");
        }
        
        Stat[] stats = GameManager.Instance.SettingsData.GetPlayerStats();
        InitInfo(stats, new Buff[0]);
        
        _playerPanel.Init(_playerInfo);
        _playerPanel.OnAttack.AddListener(Attack);
    }
    internal void InitInfo(Stat[] stats, Buff[] buffs) => _playerInfo.Init(stats, buffs);
    #endregion
}
