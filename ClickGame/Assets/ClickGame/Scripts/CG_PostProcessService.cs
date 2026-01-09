using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class CG_PostProcessService : UT_Service, CG_IPostProcessService
{
    public override string ServiceName => "PostProcess Service";

    private GameObject _PostProcessVolumePrefab;
    private CG_RippleVolumeComponent _RippleVolumeComp;
    private bool IsPlayingRippleEffect = false;

    public CG_PostProcessService(GameObject PostProcessVolumePrefab)
    {
        _PostProcessVolumePrefab = PostProcessVolumePrefab;
    }

    public override UniTask Initialize()
    {
        if (_PostProcessVolumePrefab == null)
        {
            Debug.LogError("PostProcessVolumePrefab is null!");
            return UniTask.CompletedTask;
        }

        GameObject Obj = UnityEngine.Object.Instantiate(_PostProcessVolumePrefab);
        Volume PPVolume = Obj?.GetComponent<Volume>();
        VolumeComponent VolumeComp = PPVolume?.profile?.components.Find(x => x is CG_RippleVolumeComponent);
        _RippleVolumeComp = VolumeComp as CG_RippleVolumeComponent;

        return UniTask.CompletedTask;
    }

    public void Update()
    {
        if (_RippleVolumeComp == null)
            return;

        if (IsPlayingRippleEffect)
        {
            if (_RippleVolumeComp.WaveDistanceFromCenter.value < 0.1f)
                _RippleVolumeComp.WaveDistanceFromCenter.value += Time.deltaTime * _RippleVolumeComp.Speed.value;
            else
            {
                _RippleVolumeComp.WaveDistanceFromCenter.value = -0.1f;
                IsPlayingRippleEffect = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                PlayRippleEffect(Input.mousePosition);
        }
    }

    public void PlayRippleEffect(Vector2 MousePosition)
    {
        if (_RippleVolumeComp == null)
            return;

        IsPlayingRippleEffect = true;

        Vector2 ViewportPos = Camera.main.ScreenToViewportPoint(MousePosition);

        _RippleVolumeComp.WaveDistanceFromCenter.value = -0.1f;
        _RippleVolumeComp.WaveSpawnPosition.value = ViewportPos;
    }
}
