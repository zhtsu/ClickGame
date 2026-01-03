using UnityEngine;
using UnityEngine.UI;

public class CG_FUIParams_MainUI : UT_FUIParams
{
    public CG_IVideoService IVideoService;
}

public class CG_UI_MainUI : UT_UIBase
{
    [SerializeField] private RawImage UpVideoImage;
    [SerializeField] private RawImage DownVideoImage;
    [SerializeField] private string[] UpVideoList;
    [SerializeField] private string DownVideo;

    public override void Initialize(UT_FUIParams Params)
    {
        if (Params is not CG_FUIParams_MainUI)
            return;

        var CastedParams = Params as CG_FUIParams_MainUI;
    }
}
