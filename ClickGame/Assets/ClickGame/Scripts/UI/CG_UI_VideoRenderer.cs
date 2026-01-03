using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CG_FUIParams_VideoRenderer : UT_FUIParams
{
    public CG_IVideoService IVideoService;
    public UT_IEventService IEventService;
}

public class CG_UI_VideoRenderer : UT_UIBase
{
    [SerializeField] private RawImage _UpVideoImage;
    [SerializeField] private RawImage _DownVideoImage;
    [SerializeField] private string[] _UpVideoList;
    [SerializeField] private string[] _DownVideoList;

    private CG_IVideoService _IVideoService;
    private UT_IEventService _IEventService;

    private bool _IsPlayHeadClickAnim = false;

    public override void Initialize(UT_FUIParams Params)
    {
        if (Params is not CG_FUIParams_VideoRenderer)
            return;

        var CastedParams = Params as CG_FUIParams_VideoRenderer;
        _IVideoService = CastedParams.IVideoService;
        _IEventService = CastedParams.IEventService;

        LoadAllVideo();
        UpdateUpVideo(null);
        ResetDownVideo(null);

        _IEventService.Subscribe<CG_Event_OnHeadButtonClicked>(OnHeadButtonClicked);
    }

    private void LoadAllVideo()
    {
        foreach (string VideoAddress in _UpVideoList)
        {
            _IVideoService.LoadVideo(VideoAddress);
        }

        foreach (string VideoAddress in _DownVideoList)
        {
            _IVideoService.LoadVideo(VideoAddress);
        }
    }

    private void SetUpVideo(string VideoAddress)
    {
        _UpVideoImage.texture = _IVideoService.PlayVideo(VideoAddress, false, UpdateUpVideo);
    }

    private void SetDownVideo(string VideoAddress)
    {
        _DownVideoImage.texture = _IVideoService.PlayVideo(VideoAddress, true);
    }

    private void PlayClickHeadAnim(string VideoAddress)
    {
        _DownVideoImage.texture = _IVideoService.PlayVideo(VideoAddress, false, ResetDownVideo);
    }

    private void UpdateUpVideo(VideoPlayer VP)
    {
        int RandomIndex = Random.Range(0, _UpVideoList.Length);
        SetUpVideo(_UpVideoList[RandomIndex]);
    }

    private void ResetDownVideo(VideoPlayer VP)
    {
        _IsPlayHeadClickAnim = false;

        if (_DownVideoList.Length > 0)
            SetDownVideo(_DownVideoList[0]);
    }

    private void OnDestroy()
    {
        foreach (string VideoAddress in _UpVideoList)
        {
            _IVideoService.StopVideo(VideoAddress, UpdateUpVideo);
        }

        foreach (string VideoAddress in _DownVideoList)
        {
            _IVideoService.StopVideo(VideoAddress, ResetDownVideo);
        }

        _IEventService.Unsubscribe<CG_Event_OnHeadButtonClicked>(OnHeadButtonClicked);
    }

    private void OnHeadButtonClicked(CG_Event_OnHeadButtonClicked Event)
    {
        if (_IsPlayHeadClickAnim)
            return;

        _IsPlayHeadClickAnim = true;

        int RandomIndex = Random.Range(0, _DownVideoList.Length);
        PlayClickHeadAnim(_DownVideoList[RandomIndex]);
    }
}
