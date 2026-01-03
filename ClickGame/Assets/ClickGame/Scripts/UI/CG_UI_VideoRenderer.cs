using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CG_FUIParams_VideoRenderer : UT_FUIParams
{
    public CG_IVideoService IVideoService;
}

public class CG_UI_VideoRenderer : UT_UIBase
{
    CG_IVideoService _IVideoService;

    [SerializeField] private RawImage _UpVideoImage;
    [SerializeField] private RawImage _DownVideoImage;
    [SerializeField] private string[] _UpVideoList;
    [SerializeField] private string _DownVideo;

    public override void Initialize(UT_FUIParams Params)
    {
        if (Params is not CG_FUIParams_VideoRenderer)
            return;

        var CastedParams = Params as CG_FUIParams_VideoRenderer;
        _IVideoService = CastedParams.IVideoService;

        LoadAllVideo();
        UpdateUpVideo(null);
        SetDownVideo(_DownVideo);
    }

    private void LoadAllVideo()
    {
        foreach (string VideoAddress in _UpVideoList)
        {
            _IVideoService.LoadVideo(VideoAddress);
        }

        _IVideoService.LoadVideo(_DownVideo);
    }

    private void SetUpVideo(string VideoAddress)
    {
        _UpVideoImage.texture = _IVideoService.PlayVideo(VideoAddress, false, UpdateUpVideo);
    }

    private void SetDownVideo(string VideoAddress)
    {
        _DownVideoImage.texture = _IVideoService.PlayVideo(VideoAddress, true);
    }

    private void UpdateUpVideo(VideoPlayer VP)
    {
        int RandomIndex = Random.Range(0, _UpVideoList.Length);
        SetUpVideo(_UpVideoList[RandomIndex]);
    }

    private void OnDestroy()
    {
        foreach (string VideoAddress in _UpVideoList)
        {
            _IVideoService.StopVideo(VideoAddress, UpdateUpVideo);
        }

        _IVideoService.StopVideo(_DownVideo);
    }
}
