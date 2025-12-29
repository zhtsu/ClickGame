using Cysharp.Threading.Tasks;
using System.Collections.Concurrent;

public class UT_CommandService : UT_Service, UT_ICommandService
{
    public override string ServiceName => "Command Service";

    private readonly ConcurrentQueue<UT_Command> _CommandQueue = new();

    public override UniTask Initialize()
    {
        _CommandQueue.Clear();

        return UniTask.CompletedTask;
    }

    public override void Destroy()
    {
        _CommandQueue.Clear();
    }

    public void PushCommand(UT_Command Command)
    {
        if (Command == null)
            return;

        _CommandQueue.Enqueue(Command);
    }

    public void Update()
    {
        while (_CommandQueue.TryDequeue(out UT_Command Command))
        {
            Command.Execute().Forget();
        }
    }
}
