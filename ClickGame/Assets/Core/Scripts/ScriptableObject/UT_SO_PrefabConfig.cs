using UnityEngine;

[CreateAssetMenu(fileName = "PrefabConfig", menuName = "UT Config/Prefab Config")]
public class UT_SO_PrefabConfig : ScriptableObject
{
    [SerializeField] private string[] _PrefabAddressList;
    public string[] PrefabAddressList => _PrefabAddressList;

    [SerializeField] private string[] _VideoAddressList;
    public string[] VideoAddressList => _VideoAddressList;
};