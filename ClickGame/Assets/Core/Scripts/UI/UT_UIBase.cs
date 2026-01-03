using UnityEngine;

public abstract class UT_FUIParams
{

}

public abstract class UT_UIBase : MonoBehaviour
{
    [SerializeField]
    private string _TypeKey;
    public string TypeKey => _TypeKey;

    [SerializeField]
    private GameObject _SafeArea;

    virtual public void Initialize(UT_FUIParams Params)
    {
    }

    virtual public void OnOpen()
    {
    }

    virtual public void OnClose()
    {
    }

    virtual public void OnResume()
    {
    }

    virtual public void OnPause()
    {
    }

    public void ApplySafeArea()
    {
        if (_SafeArea != null)
        {
            M3_SafeAreaEnforcer SafeAreaEnforcer = _SafeArea.GetComponent<M3_SafeAreaEnforcer>();
            if (SafeAreaEnforcer != null)
                SafeAreaEnforcer.ApplySafeArea();
        }
    }
}
