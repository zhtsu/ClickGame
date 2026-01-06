using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RippleRenderPassFeature : ScriptableRendererFeature
{
    class RippleRenderPass : ScriptableRenderPass
    {
        public Material FeatureMaterial;
        public CG_RippleVolumeComponent VolumeComponent;

        private const string _ProfilerRenderTag = "RippleRenderPass";
        private RTHandle _CameraColorTarget;
        private int _ShaderId = Shader.PropertyToID("_TempRT");

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            if (FeatureMaterial == null || VolumeComponent == null)
                return;

            FeatureMaterial.SetFloat("_Size", VolumeComponent.Size.value);
            FeatureMaterial.SetFloat("_Strength", VolumeComponent.Strength.value);
            FeatureMaterial.SetVector("_RingSpawnPosition", VolumeComponent.RingSpawnPosition.value);
            FeatureMaterial.SetFloat("_WaveDistanceFromCenter", VolumeComponent.WaveDistanceFromCenter.value);
        
            FeatureMaterial.SetColor("_TestColor", VolumeComponent.TestColor.value);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (FeatureMaterial == null || VolumeComponent == null)
                return;

            CommandBuffer Cmd = CommandBufferPool.Get(_ProfilerRenderTag);
            Cmd.GetTemporaryRT(_ShaderId, Camera.main.pixelWidth, Camera.main.pixelHeight, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
        
            _CameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;

            Cmd.Blit(_CameraColorTarget.nameID, _ShaderId, FeatureMaterial);
            Cmd.Blit(_ShaderId, _CameraColorTarget.nameID);
        
            context.ExecuteCommandBuffer(Cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ShaderId);
            cmd.Clear();
        }
    }

    public RenderPassEvent TheRenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    public Material FeatureMaterial;

    private RippleRenderPass _ScriptablePass;
    private CG_RippleVolumeComponent _VolumeComponent;

    public override void Create()
    {
        _ScriptablePass = new RippleRenderPass();
        _ScriptablePass.FeatureMaterial = FeatureMaterial;
        _VolumeComponent = VolumeManager.instance.stack.GetComponent<CG_RippleVolumeComponent>();
        _ScriptablePass.VolumeComponent = _VolumeComponent;
        _ScriptablePass.renderPassEvent = TheRenderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (FeatureMaterial != null && _VolumeComponent != null && _VolumeComponent.IsActive())
            renderer.EnqueuePass(_ScriptablePass);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}