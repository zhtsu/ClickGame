using UnityEngine;

[CreateAssetMenu(fileName = "UITypeDescriptor", menuName = "UI/Type Descriptor")]
public class UT_SO_UIDescriptor : ScriptableObject
{
    [SerializeField] private string _TypeKey;
    public string TypeKey => _TypeKey;

    [SerializeField] private string _PrefabAddress;
    public string PrefabAddress => _PrefabAddress;

    [SerializeField] private UT_EUILayer _Layer;
    public UT_EUILayer Layer => _Layer;
};