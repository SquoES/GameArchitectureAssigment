using UnityEngine;
using UnityEngine.UI;

internal class StartPanelUI : MonoBehaviour
{
    [SerializeField] private Button b_Buffs;
    [SerializeField] private Button b_NoBuffs;
    private void Start()
    {
        b_Buffs.onClick.AddListener(() => GameManager.Instance.InitGame(true));
        b_NoBuffs.onClick.AddListener(() => GameManager.Instance.InitGame(false));
    }
}
