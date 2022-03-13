using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerBehaviour _player;
    private PlayerInfo _info;
    private Animator _animator;

    private PlayerBehaviour _damageTarget;
    
    internal void Init(PlayerBehaviour player)
    {
        _player = player;
        _info = _player.PlayerInfo;
        _animator = _player.Animator;
    }

    internal void SetDamageTarget(PlayerBehaviour playerBehaviour)
    {
        _damageTarget = playerBehaviour;
    }

    internal void Attack(out float damageSet)
    {
        damageSet = 0f;
        if (!_player.isAlive) return;
        if (_damageTarget == null || !_damageTarget.isAlive) return;
        if (_animator) _animator.SetTrigger("Attack");
        
        _damageTarget.Damage(_info.GetStatValue(StatsId.DAMAGE_ID).Current, out damageSet);
    }
}
