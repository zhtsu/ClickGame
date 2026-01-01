using UnityEngine;

public class CG_Boot : UT_Boot
{
    public override void Initialize(UT_IServiceContainer ServiceContainer)
    {
        UT_IUIService IUIService = ServiceContainer.GetService<UT_IUIService>();

        IUIService.OpenUI("VideoRenderer");
    }
}
