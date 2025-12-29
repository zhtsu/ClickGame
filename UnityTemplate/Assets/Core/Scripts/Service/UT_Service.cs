using Cysharp.Threading.Tasks;

public abstract class UT_Service
{
    virtual public string ServiceName => "Default Service";
    virtual public UniTask Initialize() { return UniTask.CompletedTask; }
    virtual public void Destroy() { }
}
