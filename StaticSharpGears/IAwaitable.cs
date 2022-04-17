using System.Runtime.CompilerServices;

namespace StaticSharp.Gears;

public interface IAwaitable {
    IAwaiter GetAwaiter();
}


public interface IAwaiter : INotifyCompletion {
    bool IsCompleted { get; }
    void GetResult();
}


public interface IAwaitable<out TResult> {
    IAwaiter<TResult> GetAwaiter();
}

public interface IAwaiter<out TResult> : INotifyCompletion // or ICriticalNotifyCompletion
{
    bool IsCompleted { get; }

    TResult GetResult();
}

