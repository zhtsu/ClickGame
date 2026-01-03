using UnityEngine;

[CreateAssetMenu(fileName = "PrefabConfig", menuName = "UT Config/Prefab Config")]
public class UT_SO_PrefabConfig : ScriptableObject
{
    [SerializeField] private string _UILabel;
    public string UILabel => _UILabel;

    [SerializeField] private string _VideoLabel;
    public string VideoLabel => _VideoLabel;
};