using System;
using System.Collections.Generic;
using UnityEngine;

internal static class JsonHelper
{
    internal static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }
    internal static List<T> FromJsonList<T>(string json)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        List<T> list = new List<T>();
        
        if (wrapper == null) return list;
        
        for (int i = 0; i < wrapper.Items.Length; i++)
            list.Add(wrapper.Items[i]);
        return list;
    }

    internal static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    internal static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    internal static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        T[] array = new T[list.Count];
        for (int i = 0; i < list.Count; i++)
            array[i] = list[i];
        
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
