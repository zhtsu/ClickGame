using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "UT Config/UI Config")]
public class UT_SO_UIConfig : ScriptableObject
{
    [SerializeField] private GameObject _UIRootPrefab;
    public GameObject UIRootPrefab => _UIRootPrefab;

    [SerializeField] private GameObject _LoadingScreenPrefab;
    public GameObject LoadingScreenPrefab => _LoadingScreenPrefab;

    [SerializeField] private UT_SO_UIDescriptor[] _UITypeDescriptorList;
    public UT_SO_UIDescriptor[] UITypeDescriptorList => _UITypeDescriptorList;
};