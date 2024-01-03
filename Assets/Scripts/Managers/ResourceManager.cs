using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager
{
    private Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, AsyncOperationHandle> resourcesHandle = new Dictionary<string, AsyncOperationHandle>();


    public GameObject Instantiate(string key, Transform parent = null, bool instantiateInWorld = false)
    {
        GameObject go = GetResource<GameObject>(key);
        if (go == null)
        {
            Debug.LogError($"[ResourceManager] Instantiate({key}): Failed to load prefab.");
            return null;
        }

        // ToDo: Ǯ�� ������Ʈ�� ó������ ���� �ۼ� 

        return UnityEngine.Object.Instantiate(go, parent, instantiateInWorld);
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null) return;

        // ToDo : Ǯ�� ������Ʈ ó��

        UnityEngine.Object.Destroy(obj);
    }

    //resources(��ųʸ�)���� �� ��ȯ
    public T GetResource<T>(string key) where T : UnityEngine.Object
    {
        if (!resources.TryGetValue(key, out UnityEngine.Object resource)) return null;
        return resource as T;
    }

    //�޸� ����
    public void ReleaseAsset(string key)
    {
        if (resourcesHandle.TryGetValue(key, out AsyncOperationHandle operationHandle) == false) return;
        Addressables.Release(operationHandle);
        resources.Remove(key);
        resourcesHandle.Remove(key);
    }

    //���� �ε�
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }



        string loadKey = key;

        if (key.Contains(".sprite"))
            loadKey = $"{key}[{key.Replace(".sprite", "")}]";
        // ���ҽ� �񵿱� �ε� ����
        if (key.Contains(".sprite"))
        {
            AsyncOperationHandle<Sprite> asyncOperation = Addressables.LoadAssetAsync<Sprite>(loadKey);
            asyncOperation.Completed += obj =>
            {
                resources.Add(key, obj.Result);
                resourcesHandle.Add(key, obj);
                callback?.Invoke(obj.Result as T);
            };
        }
        else if (key.Contains(".multiSprite"))
        {
            AsyncOperationHandle<IList<Sprite>> handle = Addressables.LoadAssetAsync<IList<Sprite>>(loadKey);
            HandleCallback<Sprite>(key, handle, objs => callback?.Invoke(objs as T));
        }
        else
        {
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            asyncOperation.Completed += obj =>
            {
                resources.Add(key, obj.Result);
                callback?.Invoke(obj.Result as T);
            };
        }
    }
    //���� �ε� && �ν��Ͻ�
    public void InstantiateAssetAsync(string key, Transform parent = null, bool instantiateInWorld = false)
    {
        if (resources.TryGetValue(key, out UnityEngine.Object resource))
        {
            Instantiate(key, parent, instantiateInWorld);
            return;
        }

        AsyncOperationHandle<GameObject> asyncOperation = Addressables.InstantiateAsync(key, parent, instantiateInWorld);
        asyncOperation.Completed += (AsyncOperationHandle<GameObject> obj) => {
            resources.Add(key, obj.Result);
            resourcesHandle.Add(key, obj);
        };
    }

    //�� �ε�
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        AsyncOperationHandle<IList<IResourceLocation>> operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += (AsyncOperationHandle<IList<IResourceLocation>> obj) => {
            int loadCount = 0;
            int totalCount = obj.Result.Count;
            foreach (IResourceLocation location in obj.Result)
            {
                LoadAsync<T>(location.PrimaryKey, obj => {
                    loadCount++;
                    callback?.Invoke(location.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    //�� �ε� && �ν��Ͻ�
    public void InstantialteAllAsync(string label,Transform parent = null, bool instantiateInWorld = false)
    {
        AsyncOperationHandle<IList<IResourceLocation>> operation = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));
        operation.Completed += (AsyncOperationHandle<IList<IResourceLocation>> obj) => {
            foreach (IResourceLocation location in obj.Result)
            {
                InstantiateAssetAsync(location.PrimaryKey, parent, instantiateInWorld);
            }
        };
    }

    private void HandleCallback<T>(string key, AsyncOperationHandle<IList<T>> handle, Action<IList<T>> callback) where T : UnityEngine.Object
    {
        handle.Completed += operationHandle =>
        {
            IList<T> resultList = operationHandle.Result;
            // ����Ʈ�� �� �������� _resources�� �߰��մϴ�.  

            for (int i = 0; i < resultList.Count; i++)
            {
                resources.Add(resultList[i].name, resultList[i]);
            }
            callback?.Invoke(resultList);
        };
    }
}
