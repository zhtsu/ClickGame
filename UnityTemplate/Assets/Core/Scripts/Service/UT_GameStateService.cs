using Cysharp.Threading.Tasks;

public enum UT_EGameState
{
    None,
    Loading,
    InMainMenu,
    Playing,
    Paused,
}

public class UT_GameStateService : UT_Service, UT_IGameStateService
{
    public override string ServiceName => "GameState Service";

    public override void Destroy()
    {

    }

    public override UniTask Initialize()
    {
        return UniTask.CompletedTask;
    }

    public void SetGameState(UT_EGameState InNewState)
    {
        throw new System.NotImplementedException();
    }
}
