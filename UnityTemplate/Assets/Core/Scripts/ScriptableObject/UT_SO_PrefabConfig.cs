using UnityEngine;

[CreateAssetMenu(fileName = "PrefabConfig", menuName = "UT Config/Prefab Config")]
public class UT_SO_PrefabConfig : ScriptableObject
{
    [SerializeField] private string[] _PrefabAddressList;
    public string[] PrefabAddressList => _PrefabAddressList;
};