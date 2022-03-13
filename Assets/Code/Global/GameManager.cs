using UnityEngine;

internal class GameManager : Singleton<GameManager>
{
    [SerializeField] private SettingsData settingsData;
    internal SettingsData SettingsData => settingsData;

    [SerializeField] private CameraBehaviour _camera;

    [SerializeField] private PlayerBehaviour[] players;

    [SerializeField] private Transform tr_PlayersCanvas;
    internal Transform Tr_PlayersCanvas => tr_PlayersCanvas;

    protected override void Awake()
    {
        base.Awake();

        if (settingsData == null)
        {
            Debug.LogError("Settings data doesn't exists");
            return;
        }

        settingsData.Init();

        if (_camera == null)
        {
            Debug.LogError("CameraBehaviour doesn't exists");
        }

        settingsData.GetCameraSettings(out CameraModel cameraModel);
        _camera.Init(cameraModel);
    }

    internal void InitGame(bool withBuffs)
    {
        for (int i = 0; i < players.Length; i++)
        {
            Buff[] buffs = null;
            
            Stat[] stats = settingsData.GetPlayerStats();
            if (withBuffs)
            {
                buffs = settingsData.GetPlayerBuffs();
            }
            
            players[i].InitInfo(stats, buffs);

            //temp set target to player instead of pick target
            int playerTarget = i + 1;
            playerTarget = playerTarget > (players.Length - 1) ? 0 : playerTarget;
            players[i].SetDamageTarget(players[playerTarget]);
        }
    }
}
