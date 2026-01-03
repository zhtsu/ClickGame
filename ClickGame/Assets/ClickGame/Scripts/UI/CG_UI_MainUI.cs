using UnityEngine;
using UnityEngine.UI;

public class CG_Event_OnHeadButtonClicked : UT_Event
{
}

public class CG_FUIParams_MainUI : UT_FUIParams
{
    public UT_IEventService IEventService;
}

public class CG_UI_MainUI : UT_UIBase
{
    [SerializeField] private Button _HeadButton;
    [SerializeField] private Button _MissionButton;
    [SerializeField] private Button _TalentButton;
    [SerializeField] private Button _MenuButton;

    private UT_IEventService _IEventService;

    public override void Initialize(UT_FUIParams Params)
    {
        if (Params is not CG_FUIParams_MainUI)
            return;

        _HeadButton.onClick.AddListener(OnHeadButtonClicked);
        _MissionButton.onClick.AddListener(OnMissionButtonClicked);
        _TalentButton.onClick.AddListener(OnTalentButtonClicked);
        _MenuButton.onClick.AddListener(OnMenuButtonClicked);

        var CastedParams = Params as CG_FUIParams_MainUI;
        _IEventService = CastedParams.IEventService;
    }

    public void OnHeadButtonClicked()
    {
        _IEventService.Dispatch(new CG_Event_OnHeadButtonClicked());
    }

    public void OnMissionButtonClicked()
    {
        Debug.Log("Mission Button Clicked");
    }

    public void OnTalentButtonClicked()
    {
        Debug.Log("Talent Button Clicked");
    }

    public void OnMenuButtonClicked()
    {
        Debug.Log("Menu Button Clicked");
    }
}
