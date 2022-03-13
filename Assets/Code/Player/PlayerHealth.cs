using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

internal class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Transform tr_Head;
    [SerializeField] private AssetReference ref_HealthView;
    private PlayerHealthUI healthView;

    private float health;
    private bool isAlive;
    internal bool IsAlive => isAlive;
    
    private PlayerBehaviour _player;
    private PlayerInfo _info;
    private Animator _animator;
    internal void Init(PlayerBehaviour player)
    {
        _player = player;
        _animator = _player.Animator;
        _info = _player.PlayerInfo;

        _info.OnStatUpdated += OnModelHealthChanged;

        Transform tr_Canvas = GameManager.Instance.Tr_PlayersCanvas;
        CoroutineHandler.Start(Loader.ADRLoadGameObject(ref_HealthView, handle =>
        {
            healthView = (handle.Result as GameObject).GetComponent<PlayerHealthUI>();
            
            StatValue healthValue = _info.GetStatValue(StatsId.LIFE_ID);
            float currentHealth = healthValue.Current;
            float maxHealth = healthValue.Max;
            healthView.Init(tr_Head, currentHealth, maxHealth);
        }, tr_Canvas));

        _player.OnAttack += OnAttack;
    }
    

    internal void TakeDamage(float setDamage, out float damageTaken)
    {
        float currentHealth = _info.GetStatValue(StatsId.LIFE_ID).Current;
        float armor = _info.GetStatValue(StatsId.ARMOR_ID).Current;
        damageTaken = setDamage * (1 - armor / 100f);
        damageTaken = Mathf.Min(damageTaken, currentHealth);
        
        _info.ChangeCurrentValue(StatsId.LIFE_ID, -damageTaken);
    }

    private void OnAttack(object sender, float damageValue)
    {
        float lifeSteal = damageValue * _info.GetStatValue(StatsId.LIFE_STEAL_ID).Current / 100f;
        _info.ChangeCurrentValue(StatsId.LIFE_ID, lifeSteal);
    }

    private void OnModelHealthChanged(object sender, int statId)
    {
        if (statId == StatsId.LIFE_ID)
        {
            StatValue healthValue = _info.GetStatValue(StatsId.LIFE_ID);
            float currentHealth = healthValue.Current;
            float maxHealth = healthValue.Max;
            float deltaHealth = currentHealth - health;
            health = currentHealth;
            
            healthView?.HealthChanged(deltaHealth, currentHealth, maxHealth);
            _animator.SetInteger("Health", (int) Math.Ceiling(currentHealth));
            if (currentHealth <= 0) SetDead();
            else SetAlive();
        }
    }

    private void SetAlive()
    {
        isAlive = true;
    }
    private void SetDead()
    {
        isAlive = false;
    }
}
