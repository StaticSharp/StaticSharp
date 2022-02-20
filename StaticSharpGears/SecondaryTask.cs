using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StaticSharpGears;




public class SecondaryTask<T> : ISynchronouslyCompletable<T>, IAwaitable<T>, IAwaiter<T> {

    T Result = default!;

    Exception? Exception;
    public bool IsCompleted { get; private set; }
    event Action? Continuation;


    public T GetResult() {
        if (!IsCompleted)
            throw new InvalidOperationException("Result not available.");

        if (Exception!=null)
            throw Exception;

        return Result;
    }

    public void OnCompleted(Action continuation) {
        Continuation += continuation;
    }

    public IAwaiter<T> GetAwaiter() {
        return this;
    }

    public void SetResult(T value) {
        if (!TrySetResult(value)) {
            throw ISynchronouslyFailable.TaskAlreadyCompleted;
        }
    }
    public bool TrySetResult(T value) {
        if (IsCompleted) return false;
        Result = value;
        IsCompleted = true;
        Continuation?.Invoke();
        return true;
    }

    public void SetException(Exception exception) {
        if (!TrySetException(exception)) {
            throw ISynchronouslyFailable.TaskAlreadyCompleted;
        }
    }

    public bool TrySetException(Exception exception) {
        if (IsCompleted) return false;
        Exception = exception;
        IsCompleted = true;
        Continuation?.Invoke();
        return false;
    }
}




