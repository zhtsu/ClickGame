using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;

public class UT_PrefabService : UT_Service, UT_IPrefabService
{
    public override string ServiceName => "Prefab Service";

    private UT_SO_PrefabConfig _PrefabConfig;
    
    private AsyncOperationHandle<IList<GameObject>> _PrefabListHandle;
    private AsyncOperationHandle<IList<VideoClip>> _VideoListHandle;

    private Dictionary<string, GameObject> _PrefabDict = new();
    private Dictionary<string, VideoClip> _VideoDict = new();

    public UT_PrefabService(UT_SO_PrefabConfig PrefabConfig)
    {
        _PrefabConfig = PrefabConfig;
    }

    public override async UniTask Initialize()
    {
        List<UniTask> LoadTasks = new List<UniTask>();

        if (string.IsNullOrEmpty(_PrefabConfig.UILabel) == false)
        {
            _PrefabListHandle = Addressables.LoadAssetsAsync<GameObject>(
                _PrefabConfig.UILabel,
                (Prefab) =>
                {
                    if (Prefab != null)
                    {
                        _PrefabDict[Prefab.name] = Prefab;
                    }
                });

            LoadTasks.Add(_PrefabListHandle.ToUniTask());
        }

        if (string.IsNullOrEmpty(_PrefabConfig.UILabel) == false)
        {
            _VideoListHandle = Addressables.LoadAssetsAsync<VideoClip>(
                _PrefabConfig.VideoLabel,
                (Video) =>
                {
                    if (Video != null)
                    {
                        _VideoDict[Video.name] = Video;
                    }
                });

            LoadTasks.Add(_VideoListHandle.ToUniTask());
        }

        await UniTask.WhenAll(LoadTasks);
    }

    public override void Destroy()
    {
        if (_PrefabListHandle.IsValid())
        {
            Addressables.Release(_PrefabListHandle);
        }
        _PrefabDict.Clear();

        if (_VideoListHandle.IsValid())
        {
            Addressables.Release(_VideoListHandle);
        }
        _VideoDict.Clear();
    }

    public GameObject GetPrefab(string Address)
    {
        if (_PrefabDict.TryGetValue(Address, out GameObject OutPrefab))
        {
            return OutPrefab;
        }

        return null;
    }

    public VideoClip GetVideo(string Address)
    {
        if (_VideoDict.TryGetValue(Address, out VideoClip OutVideo))
        {
            return OutVideo;
        }

        return null;
    }
}
