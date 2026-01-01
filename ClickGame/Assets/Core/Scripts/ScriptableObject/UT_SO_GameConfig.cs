using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "UT Config/Game Config")]
public class UT_SO_GameConfig : ScriptableObject
{
    [SerializeField] private GameObject _BootPrefab;
    public GameObject BootPrefab => _BootPrefab;

    [SerializeField] private GameObject _MainCameraPrefab;
    public GameObject MainCameraPrefab => _MainCameraPrefab;

    [SerializeField] private GameObject _VideoRootPrefab;
    public GameObject VideoRootPrefab => _VideoRootPrefab;

    [SerializeField] private GameObject _ServiceContainerPrefab;
    public GameObject ServiceContainerPrefab => _ServiceContainerPrefab;

    [SerializeField] private float _MouseDragThreshold = 5f;
    public float MouseDragThreshold => _MouseDragThreshold;
};