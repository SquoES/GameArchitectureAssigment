using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour, IUpdate
{
    [Header("Health bar")]
    [SerializeField] private Image i_healthBar;
    [SerializeField] private Gradient grad_Health;

    [Header("Pop up text")]
    [SerializeField] private AssetReference ref_TextInfo;
    [SerializeField] private Gradient grad_DeltaHealth;    
    [SerializeField] private Vector2 textStartPos;
    [SerializeField] private Vector2 textEndPos;
    [SerializeField] private float textMoveTime = 1f;

    
    [SerializeField] private Vector3 pos_Bias;
    private Transform tr_Player;

    private Camera camera;

    internal void Init(Transform player, float currentHealth, float maxHealth)
    {
        camera = Camera.main;
        tr_Player = player;
        HealthChanged(0f, currentHealth, maxHealth);
    }
    internal void HealthChanged(float deltaHealth, float currentHealth, float maxHealth)
    {
        #region Set helth bar
        float perHealth = currentHealth / maxHealth;
        Color setColor = grad_Health.Evaluate(perHealth);
        i_healthBar.SetColorLerp(setColor);
        i_healthBar.FillAmountLerp(perHealth);
        #endregion

        #region Set pop up text
        int intDelta = (int) Math.Round(deltaHealth);
        if (intDelta != 0)
        {
            CoroutineHandler.Start(Loader.ADRLoadGameObject(ref_TextInfo, handle =>
            {
                Text t_DeltaHealth = (handle.Result as GameObject).GetComponent<Text>();
                if (intDelta > 0)
                {
                    t_DeltaHealth.color = grad_DeltaHealth.Evaluate(1f);
                }
                else
                {
                    t_DeltaHealth.color = grad_DeltaHealth.Evaluate(0f);
                }

                t_DeltaHealth.text = intDelta.ToString();
                
                CoroutineHandler.Start(t_DeltaHealth.MoveLerp(textStartPos, textEndPos, textMoveTime,
                    b =>
                    {
                        Addressables.Release(handle);
                    }));
            }, transform));
        }
        #endregion
    }

    public void GUpdate()
    {
        if (tr_Player == null) return;

        Vector3 screenPos = camera.WorldToScreenPoint(tr_Player.position + pos_Bias);
        screenPos = new Vector3(screenPos.x, screenPos.y, 0f);
        transform.position = screenPos;
    }

    private void OnEnable()
    {
        GlobalUpdate.AddToUpdate(this);
    }

    private void OnDisable()
    {
        GlobalUpdate.RemoveFromUpdate(this);
    }
}
