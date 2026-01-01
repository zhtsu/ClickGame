using Cysharp.Threading.Tasks;
using UnityEngine;

public class UT_GameController : MonoBehaviour
{
    [SerializeField] private UT_SO_GameConfig _GameConfig;
    [SerializeField] private UT_SO_AudioConfig _AudioConfig;
    [SerializeField] private UT_SO_PrefabConfig _PrefabConfig;
    [SerializeField] private UT_SO_UIConfig _UIConfig;

    private UT_Boot _Boot = new CG_Boot();
    private UT_ServiceContainer _ServiceContainer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private async void Start()
    {
        Instantiate(_GameConfig.MainCameraPrefab);
        GameObject LoadingScreen = Instantiate(_UIConfig.LoadingScreenPrefab);

        UT_FServiceContainerInitParams Params = new();
        Params.GameConfig = _GameConfig;
        Params.PrefabConfig = _PrefabConfig;
        Params.UIConfig = _UIConfig;
        Params.AudioConfig = _AudioConfig;

        GameObject ServiceContainerInst = Instantiate(_GameConfig.ServiceContainerPrefab);
        if (ServiceContainerInst != null)
            _ServiceContainer = ServiceContainerInst.GetComponent<UT_ServiceContainer>();

        if (_ServiceContainer != null)
            await _ServiceContainer.Initialize(Params);

        if (LoadingScreen != null)
            Destroy(LoadingScreen);

        if (_Boot != null)
            _Boot.Initialize(_ServiceContainer);
    }

    private void OnDestroy()
    {
        if (_ServiceContainer != null)
            _ServiceContainer.Destroy();
    }
}
