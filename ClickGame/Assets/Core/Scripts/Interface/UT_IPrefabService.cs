using UnityEngine;
using UnityEngine.Video;

public interface UT_IPrefabService
{
    GameObject GetPrefab(string Address);
    VideoClip GetVideo(string Address);
}
