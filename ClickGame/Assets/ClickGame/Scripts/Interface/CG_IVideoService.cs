using System;
using UnityEngine;
using UnityEngine.Video;

public struct CG_FVideoCacheInfo
{
    public VideoPlayer VideoPlayer;
    public RenderTexture VideoTexture;
}

public interface CG_IVideoService
{
    bool LoadVideo(string VideoAddress);
    RenderTexture PlayVideo(string VideoAddress, bool Loop = false, VideoPlayer.EventHandler OnLoopPointReached = null);
    void StopVideo(string VideoAddress, VideoPlayer.EventHandler OnLoopPointReached = null);
}
