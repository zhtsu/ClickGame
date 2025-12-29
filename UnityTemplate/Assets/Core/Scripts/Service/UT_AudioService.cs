using Cysharp.Threading.Tasks;
using UnityEngine;

public class UT_AudioService : UT_Service, UT_IAudioService
{
    public override string ServiceName => "Audio Service";

    private UT_SO_AudioConfig _AudioConfig;
    private UT_AudioRoot _AudioRoot;

    public UT_AudioService(UT_SO_AudioConfig AudioConfig)
    {
        _AudioConfig = AudioConfig;
    }

    public override void Destroy()
    {

    }

    public override UniTask Initialize()
    {
        if (_AudioConfig.AudioRootPrefab == null)
        {
            Debug.LogError("Audio Root Prefab is null!");
            return UniTask.CompletedTask;
        }
        
        _AudioRoot = UnityEngine.Object.Instantiate(_AudioConfig.AudioRootPrefab)?.GetComponent<UT_AudioRoot>();
        return UniTask.CompletedTask;
    }
}
