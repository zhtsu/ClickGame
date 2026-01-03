using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

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

    public bool LoadVideo(string VideoAddress)
    {
        if (_CachedVideoDict.TryGetValue(VideoAddress, out var CachedInfo))
        {
            return true;
        }

        VideoClip VideoClip = _IPrefabService.GetVideo(VideoAddress);
        if (VideoClip == null)
        {
            Debug.LogError($"Failed to load video: {VideoAddress}");
            return false;
        }

        GameObject VideoObj = new GameObject($"Video_{VideoAddress}");
        VideoObj.transform.SetParent(_VideoRoot.transform);

        VideoPlayer VideoPlayer = VideoObj.AddComponent<VideoPlayer>();
        VideoPlayer.clip = VideoClip;
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

        return true;
    }

    public RenderTexture PlayVideo(string VideoAddress, bool Loop = false, VideoPlayer.EventHandler OnLoopPointReached = null)
    {
        if (_CachedVideoDict.TryGetValue(VideoAddress, out var CachedInfo))
        {
            CachedInfo.VideoPlayer.isLooping = Loop;
            if (OnLoopPointReached != null)
            {
                CachedInfo.VideoPlayer.loopPointReached -= OnLoopPointReached;
                CachedInfo.VideoPlayer.loopPointReached += OnLoopPointReached;
            }
            CachedInfo.VideoPlayer.Play();
            return CachedInfo.VideoTexture;
        }

        Debug.LogWarning($"Video not found in cache: {VideoAddress}");
        return null;
    }

    public void StopVideo(string VideoAddress, VideoPlayer.EventHandler OnLoopPointReached = null)
    {
        if (_CachedVideoDict.TryGetValue(VideoAddress, out var CachedInfo))
        {
            if (OnLoopPointReached != null)
                CachedInfo.VideoPlayer.loopPointReached -= OnLoopPointReached;
            
            CachedInfo.VideoPlayer.Stop();
        }
    }
}
