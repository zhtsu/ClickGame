using System;
using UnityEngine;

public interface CG_IVideoService
{
    RenderTexture PlayVideo(string VideoAddress, bool Loop);
    void StopVideo(string VideoAddress);
}
