using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("ClickGame/Ripple")]
public class CG_RippleVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public BoolParameter Enable = new BoolParameter(false);

    public FloatParameter AspectRatio = new FloatParameter(0.5625f);
    public FloatParameter Size = new FloatParameter(0.05f);
    public FloatParameter Strength = new FloatParameter(-0.1f);
    public FloatParameter Speed = new FloatParameter(1f);
    public Vector2Parameter WaveSpawnPosition = new Vector2Parameter(new Vector2(0.5f, 0.5f));
    public FloatParameter WaveDistanceFromCenter = new FloatParameter(-0.1f);

    public bool IsActive()
    {
        return Enable.value;
    }

    public bool IsTileCompatible()
    {
        return true;
    }
}
