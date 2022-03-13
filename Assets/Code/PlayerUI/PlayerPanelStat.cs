using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PlayerPanelStat : MonoBehaviour
{
    [SerializeField] private Image i_Icon;
    [SerializeField] private Text t_Value;

    private string path_Icon;

    private AsyncOperationHandle myHandle;

    internal void Init(AsyncOperationHandle handle, string iconPath, string setValue)
    {
        myHandle = handle;
        
        SetData(setValue);
        SetIcon(iconPath);
    }

    internal void SetIcon(string icon)
    {
        if (path_Icon == icon) return;
        path_Icon = icon;
        Sprite setSprite = Resources.Load<Sprite>(path_Icon);
        if (setSprite != null)
        {
            i_Icon.sprite = setSprite;
        }
    }

    internal void SetData(string value)
    {
        t_Value.text = value;
    }
    
    internal void Release()
    {
        Addressables.Release(myHandle);
    }
}
