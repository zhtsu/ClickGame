using UnityEngine;

public class CG_Boot : UT_Boot
{
    public override void Initialize(UT_IServiceContainer ServiceContainer)
    {
        UT_IUIService IUIService = ServiceContainer.GetService<UT_IUIService>();
        CG_IVideoService IVideoService = ServiceContainer.GetService<CG_IVideoService>();

        CG_FUIParams_VideoRenderer UIParams = new();
        UIParams.IVideoService = IVideoService;
        IUIService.OpenUI("VideoRenderer", UIParams);
    }
}
