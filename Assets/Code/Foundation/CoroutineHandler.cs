using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CoroutineHandler : MonoBehaviour
{
    private static CoroutineHandler instance;
    
    private static Dictionary<string, IEnumerator> _routines = new Dictionary<string, IEnumerator>();
    
    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeType ()
    {
        instance = new GameObject($"#{nameof(CoroutineHandler)}").AddComponent<CoroutineHandler>();
        DontDestroyOnLoad(instance);
    }

    internal static Coroutine Start (IEnumerator routine) => instance.StartCoroutine(routine);
    internal static Coroutine Start (string id, IEnumerator routine)
    {
        var coroutine = instance.StartCoroutine(routine);
        if( !_routines.ContainsKey(id) ) _routines.Add(id , routine);
        else
        {
            instance.StopCoroutine(_routines[id]);
            _routines[id] = routine;
        }
        return coroutine;
    }

    internal static void Stop (IEnumerator routine) => instance.StopCoroutine(routine);
    internal static void Stop (string id)
    {
        if( _routines.TryGetValue(id, out var routine))
        {
            instance.StopCoroutine(routine);
            _routines.Remove(id);
        }
        else Debug.LogError($"Coroutine '{id}' not found");
    }

    internal static void StopAll () => instance.StopAllCoroutines();
}
