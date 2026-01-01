using UnityEngine;

public class CG_VideoRoot : MonoBehaviour
{
    private void Awake()
    {
        gameObject.name = "VideoRoot";
        DontDestroyOnLoad(this);
    }
}
