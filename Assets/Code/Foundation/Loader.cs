using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

internal static class Loader
{
    #region Addressables
    
    internal static IEnumerator ADRLoadGameObject(AssetReference reference, Action<AsyncOperationHandle> outHandle, Transform parent = null, bool activeAtStart = true)
    {
        AsyncOperationHandle newHandle = reference.InstantiateAsync(parent);
        yield return newHandle;
        if (newHandle.IsValid())
        {
            (newHandle.Result as GameObject).SetActive(activeAtStart);
        }
        outHandle.Invoke(newHandle);
    }
    internal static IEnumerator ADRLoadGameObject(AssetReference reference, AsyncOperationHandle currentHandle, Action<AsyncOperationHandle> outHandle, Transform parent = null, bool activeAtStart = true)
    {
        AsyncOperationHandle newHandle = reference.InstantiateAsync(parent);
        yield return newHandle;

        if (newHandle.IsValid())
        {
            (newHandle.Result as GameObject).SetActive(activeAtStart);
        }
        outHandle.Invoke(newHandle);
        
        if (currentHandle.IsValid())
        {
            Addressables.ReleaseInstance(currentHandle);
        }
    }

    internal static IEnumerator ADRLoadAsset<T>(AssetReference reference, Action<AsyncOperationHandle<T>> outHandle)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
        yield return handle;
        outHandle.Invoke(handle);
    }
    internal static IEnumerator ADRLoadAsset<T>(AssetReference reference, AsyncOperationHandle currentHandle, Action<AsyncOperationHandle<T>> outHandle)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
        yield return handle;
        
        if (currentHandle.IsValid()) Addressables.Release(currentHandle);
        
        outHandle.Invoke(handle);
    }

    #endregion
}
