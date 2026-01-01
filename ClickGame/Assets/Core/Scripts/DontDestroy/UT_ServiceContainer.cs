using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public struct UT_FServiceContainerInitParams
{
    public UT_SO_GameConfig GameConfig;
    public UT_SO_PrefabConfig PrefabConfig;
    public UT_SO_UIConfig UIConfig;
    public UT_SO_AudioConfig AudioConfig;
}

public class UT_ServiceContainer : MonoBehaviour, UT_IServiceContainer
{
    private readonly Dictionary<System.Type, UT_Service> _ServiceDict = new();

    private UT_EventService _EventService = null;
    private UT_CommandService _CommandService = null;
    private UT_PrefabService _PrefabService = null;
    private UT_UIService _UIService = null;
    private UT_GameStateService _GameStateService = null;
    private UT_AudioService _AudioService = null;
    private CG_VideoService _VideoService = null;

    public TServiceInterface GetService<TServiceInterface>()
    {
        if (_ServiceDict.TryGetValue(typeof(TServiceInterface), out UT_Service Service))
            return (TServiceInterface)(object)Service;

        return default;
    }

    private void RegistryService<TServiceInterface>(UT_Service Service)
    {
        if (!_ServiceDict.ContainsKey(typeof(TServiceInterface)))
        {
            _ServiceDict.Add(typeof(TServiceInterface), Service);
        }
    }

    private void LateUpdate()
    {
        if (_EventService != null)
            _EventService.Update();
    }

    public async UniTask Initialize(UT_FServiceContainerInitParams Params)
    {
        // Create and initialize services
        _PrefabService = new UT_PrefabService(Params.PrefabConfig);
        RegistryService<UT_IPrefabService>(_PrefabService);
        await _PrefabService.Initialize();

        _UIService = new UT_UIService(Params.UIConfig, _PrefabService);
        RegistryService<UT_IUIService>(_UIService);
        _ = _UIService.Initialize();

        _EventService = new UT_EventService();
        RegistryService<UT_IEventService>(_EventService);
        _ = _EventService.Initialize();

        _CommandService = new UT_CommandService();
        RegistryService<UT_ICommandService>(_CommandService);
        _ = _CommandService.Initialize();

        _GameStateService = new UT_GameStateService();
        RegistryService<UT_IGameStateService>(_GameStateService);
        _ = _GameStateService.Initialize();

        _AudioService = new UT_AudioService(Params.AudioConfig);
        RegistryService<UT_IAudioService>(_AudioService);
        _ = _AudioService.Initialize();

        _VideoService = new CG_VideoService(Params.GameConfig.VideoRootPrefab, _PrefabService);
        RegistryService<CG_IVideoService>(_VideoService);
        _ = _VideoService.Initialize();
    }

    public void Destroy()
    {
        _EventService?.Destroy();
        _CommandService?.Destroy();
        _GameStateService?.Destroy();
        _UIService?.Destroy();
        _PrefabService?.Destroy();
        _AudioService?.Destroy();
        _VideoService?.Destroy();
    }

    private void Awake()
    {
        gameObject.name = "ServiceContainer";
        DontDestroyOnLoad(this);
    }
}
