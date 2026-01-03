using UnityEngine;

public class CG_Boot : UT_Boot
{
    public override void Initialize(UT_IServiceContainer ServiceContainer)
    {
        UT_IUIService IUIService = ServiceContainer.GetService<UT_IUIService>();
        CG_IVideoService IVideoService = ServiceContainer.GetService<CG_IVideoService>();
        UT_IEventService IEventService = ServiceContainer.GetService<UT_IEventService>();

        CG_FUIParams_VideoRenderer VRParams = new();
        VRParams.IVideoService = IVideoService;
        VRParams.IEventService = IEventService;
        IUIService.OpenUI("VideoRenderer", VRParams);

        CG_FUIParams_MainUI MUIParams = new();
        MUIParams.IEventService = IEventService;
        IUIService.OpenUI("MainUI", MUIParams);
    }
}
