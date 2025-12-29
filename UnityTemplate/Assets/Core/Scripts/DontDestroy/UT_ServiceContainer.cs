using Cysharp.Threading.Tasks;
using UnityEngine;

public struct UT_FServiceContainerInitParams
{
    public UT_SO_GameConfig GameConfig;
    public UT_SO_PrefabConfig PrefabConfig;
    public UT_SO_UIConfig UIConfig;
    public UT_SO_AudioConfig AudioConfig;
}

public class UT_ServiceContainer : MonoBehaviour
{
    private UT_EventService _EventService = null;
    private UT_CommandService _CommandService = null;
    private UT_PrefabService _PrefabService = null;
    private UT_UIService _UIService = null;
    private UT_GameStateService _GameStateService = null;
    private UT_AudioService _AudioService = null;

    private void LateUpdate()
    {
        if (_EventService != null)
            _EventService.Update();
    }

    public async UniTask Initialize(UT_FServiceContainerInitParams Params)
    {
        // Create and initialize services
        _PrefabService = new UT_PrefabService(Params.PrefabConfig);
        await _PrefabService.Initialize();

        _UIService = new UT_UIService(Params.UIConfig, _PrefabService);
        _ = _UIService.Initialize();

        _EventService = new UT_EventService();
        _ = _EventService.Initialize();

        _CommandService = new UT_CommandService();
        _ = _CommandService.Initialize();

        _GameStateService = new UT_GameStateService();
        _ = _GameStateService.Initialize();

        _AudioService = new UT_AudioService(Params.AudioConfig);
        _ = _AudioService.Initialize();
    }

    public void Destroy()
    {
        _EventService?.Destroy();
        _CommandService?.Destroy();
        _GameStateService?.Destroy();
        _UIService?.Destroy();
        _PrefabService?.Destroy();
        _AudioService?.Destroy();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
