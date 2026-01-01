using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public struct CG_FVideoCacheInfo
{
    public VideoPlayer VideoPlayer;
    public RenderTexture VideoTexture;
}

public class CG_VideoService : UT_Service, CG_IVideoService
{
    public override string ServiceName => "Audio Service";

    private GameObject _VideoRootPrefab;
    private CG_VideoRoot _VideoRoot;
    private UT_IPrefabService _IPrefabService;

    private Dictionary<string, CG_FVideoCacheInfo> _CachedVideoDict = new();

    public CG_VideoService(GameObject VideoRootPrefab, UT_IPrefabService prefabService)
    {
        _VideoRootPrefab = VideoRootPrefab;
        _IPrefabService = prefabService;
    }

    public override void Destroy()
    {
        foreach (var kvp in _CachedVideoDict)
        {
            if (kvp.Value.VideoPlayer != null)
            {
                Object.Destroy(kvp.Value.VideoPlayer.gameObject);
            }

            if (kvp.Value.VideoTexture != null)
            {
                kvp.Value.VideoTexture.Release();
                Object.Destroy(kvp.Value.VideoTexture);
            }
        }

        _CachedVideoDict.Clear();
    }

    public override UniTask Initialize()
    {
        if (_VideoRootPrefab == null)
        {
            Debug.LogError("Video Root Prefab is null!");
            return UniTask.CompletedTask;
        }

        _VideoRoot = UnityEngine.Object.Instantiate(_VideoRootPrefab)?.GetComponent<CG_VideoRoot>();
        return UniTask.CompletedTask;
    }

    public RenderTexture PlayVideo(string VideoAddress, bool Loop)
    {
        if (_CachedVideoDict.TryGetValue(VideoAddress, out var CachedInfo))
        {
            CachedInfo.VideoPlayer.isLooping = Loop;
            CachedInfo.VideoPlayer.Play();
            return CachedInfo.VideoTexture;
        }

        GameObject VideoPrefab = _IPrefabService.GetPrefab(VideoAddress);
        if (VideoPrefab == null)
        {
            Debug.LogError($"Failed to load video prefab: {VideoAddress}");
            return null;
        }

        VideoPlayer SourcePlayer = VideoPrefab.GetComponent<VideoPlayer>();
        if (SourcePlayer == null || SourcePlayer.clip == null)
        {
            Debug.LogError($"Video prefab does not contain VideoPlayer or VideoClip: {VideoAddress}");
            return null;
        }

        GameObject VideoObj = new GameObject($"Video_{VideoAddress}");
        VideoObj.transform.SetParent(_VideoRoot.transform);

        VideoPlayer VideoPlayer = VideoObj.AddComponent<VideoPlayer>();
        VideoPlayer.clip = SourcePlayer.clip;
        VideoPlayer.isLooping = Loop;
        VideoPlayer.playOnAwake = false;
        VideoPlayer.renderMode = VideoRenderMode.RenderTexture;

        RenderTexture RenderTexture = new RenderTexture(
            (int)VideoPlayer.clip.width / 2,
            (int)VideoPlayer.clip.height / 2,
            0
        );

        RenderTexture.Create();
        VideoPlayer.targetTexture = RenderTexture;

        _CachedVideoDict[VideoAddress] = new CG_FVideoCacheInfo
        {
            VideoPlayer = VideoPlayer,
            VideoTexture = RenderTexture
        };

        VideoPlayer.Play();

        return RenderTexture;
    }

    public void StopVideo(string VideoAddress)
    {
        if (_CachedVideoDict.TryGetValue(VideoAddress, out var CachedInfo))
        {
            CachedInfo.VideoPlayer.Stop();
        }
    }
}
