using Cysharp.Threading.Tasks;
using UnityEngine;

public class UT_GameController : MonoBehaviour
{
    [SerializeField] private UT_SO_GameConfig _GameConfig;
    [SerializeField] private UT_SO_AudioConfig _AudioConfig;
    [SerializeField] private UT_SO_PrefabConfig _PrefabConfig;
    [SerializeField] private UT_SO_UIConfig _UIConfig;

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

        var Results_SCP = await InstantiateAsync(_GameConfig.ServiceContainerPrefab).ToUniTask();
        if (Results_SCP.Length > 0)
        {
            _ServiceContainer = Results_SCP[0].GetComponent<UT_ServiceContainer>();
            if (_ServiceContainer == null)
            {
                Debug.LogError("ServiceContainer Component is missing in the prefab!");
            }
        }

        if (_ServiceContainer != null)
            await _ServiceContainer.Initialize(Params);

        if (LoadingScreen != null)
            Destroy(LoadingScreen);
    }

    private void OnDestroy()
    {
        if (_ServiceContainer != null)
            _ServiceContainer.Destroy();
    }
}
