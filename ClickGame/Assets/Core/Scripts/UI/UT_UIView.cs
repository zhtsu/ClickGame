using UnityEngine;

public abstract class UT_UIParams
{

}

public abstract class UT_UIView : MonoBehaviour
{
    [SerializeField]
    private string _TypeKey;
    public string TypeKey => _TypeKey;

    virtual public void Initialize(UT_UIParams Params)
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
}
