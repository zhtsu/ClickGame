using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UT_PrefabService : UT_Service, UT_IPrefabService
{
    public override string ServiceName => "Prefab Service";

    private UT_SO_PrefabConfig _PrefabConfig;
    private Dictionary<Hash128, AsyncOperationHandle<GameObject>> _PrefabHandleDict = new Dictionary<Hash128, AsyncOperationHandle<GameObject>>();

    public UT_PrefabService(UT_SO_PrefabConfig PrefabConfig)
    {
        _PrefabConfig = PrefabConfig;
    }

    public override async UniTask Initialize()
    {
        List<UniTask> LoadTasks = new List<UniTask>();

        foreach (string Address in _PrefabConfig.PrefabAddressList)
        {
            if (string.IsNullOrEmpty(Address) == false)
            {
                AsyncOperationHandle<GameObject> Handle = Addressables.LoadAssetAsync<GameObject>(Address);
                LoadTasks.Add(Handle.ToUniTask());

                _PrefabHandleDict.Add(Hash128.Compute(Address), Handle);
            }
        }

        await UniTask.WhenAll(LoadTasks);
    }

    public override void Destroy()
    {
        foreach (AsyncOperationHandle<GameObject> Handle in _PrefabHandleDict.Values)
        {
            Addressables.Release(Handle);
        }

        _PrefabHandleDict.Clear();
    }

    public GameObject GetPrefab(string Address)
    {
        if (_PrefabHandleDict.TryGetValue(Hash128.Compute(Address), out AsyncOperationHandle<GameObject> OutHandle))
        {
            return OutHandle.Result;
        }

        return null;
    }
}
