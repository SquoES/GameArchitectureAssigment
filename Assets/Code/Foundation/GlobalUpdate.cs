using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUpdate : MonoBehaviour
{
    private static GlobalUpdate instance;
    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeType ()
    {
        instance = new GameObject($"#{nameof(GlobalUpdate)}").AddComponent<GlobalUpdate>();
        DontDestroyOnLoad(instance);
    }
    
    [SerializeField] private List<IUpdate> updateScripts = new List<IUpdate>();
    [SerializeField] private List<IFixedUpdate> fixedUpdateScripts = new List<IFixedUpdate>();
    [SerializeField] private List<IOneSecUpdate> oneSecUpdateScripts = new List<IOneSecUpdate>();
    private float secondsCounter;

    internal static void AddToUpdate(IGlobalUpdate script)
    {
        if (script is IUpdate uScript)
        {
            if (instance.updateScripts.Contains(uScript)) return;
            instance.updateScripts.Add(uScript);
        }
        if (script is IFixedUpdate fuScript)
        {
            if (instance.fixedUpdateScripts.Contains(fuScript)) return;
            instance.fixedUpdateScripts.Add(fuScript);
        }
        if (script is IOneSecUpdate osScript)
        {
            if (instance.oneSecUpdateScripts.Contains(osScript)) return;
            instance.oneSecUpdateScripts.Add(osScript);
        }
    }

    internal static void RemoveFromUpdate(IGlobalUpdate script)
    {
        if (script is IUpdate uScript)
        {
            if (!instance.updateScripts.Contains(uScript)) return;
            instance.updateScripts.Remove(uScript);
        }
        if (script is IFixedUpdate fuScript)
        {
            if (!instance.fixedUpdateScripts.Contains(fuScript)) return;
            instance.fixedUpdateScripts.Remove(fuScript);
        }
        if (script is IOneSecUpdate osScript)
        {
            if (!instance.oneSecUpdateScripts.Contains(osScript)) return;
            instance.oneSecUpdateScripts.Remove(osScript);
        }
    }

    private void Update()
    {
        for (int i = 0; i < updateScripts.Count; i++)
        {
            if (updateScripts[i] == null)
            {
                updateScripts.Remove(updateScripts[i]);
                continue;
            }

            updateScripts[i].GUpdate();
        }
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < fixedUpdateScripts.Count; i++)
        {
            if (fixedUpdateScripts[i] == null)
            {
                fixedUpdateScripts.Remove(fixedUpdateScripts[i]);
                continue;
            }

            fixedUpdateScripts[i].GFixedUpdate();
        }

        secondsCounter += Time.fixedUnscaledDeltaTime;
        if (secondsCounter >= 1f)
        {
            secondsCounter = 0f;
            for (int i = 0; i < oneSecUpdateScripts.Count; i++)
            {
                if (oneSecUpdateScripts[i] == null)
                {
                    oneSecUpdateScripts.Remove(oneSecUpdateScripts[i]);
                    continue;
                }

                oneSecUpdateScripts[i].GOneSecUpdate();
            }
        }
    }
}

interface IGlobalUpdate
{
    
}
interface IUpdate : IGlobalUpdate
{
    void GUpdate();
}
interface IFixedUpdate : IGlobalUpdate
{
    void GFixedUpdate();
}
interface IOneSecUpdate : IGlobalUpdate
{
    void GOneSecUpdate();
}

internal enum UpdatePeriod
{
    Update,
    FixedUpdate,
    OneSecond
}
