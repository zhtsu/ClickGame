using Cysharp.Threading.Tasks;

public abstract class UT_Command
{
    public string GetString() { return "Command"; }
    public abstract UniTask Execute();
}
