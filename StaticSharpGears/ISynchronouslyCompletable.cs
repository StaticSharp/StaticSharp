namespace StaticSharpGears;


public interface ISynchronouslyFailable {
    public static InvalidOperationException TaskAlreadyCompleted => new InvalidOperationException("Task already completed.");

    bool IsCompleted { get; }
    bool TrySetException(Exception exception);
    void SetException(Exception exception) {
        if (!TrySetException(exception)) {
            throw TaskAlreadyCompleted;
        }
    }
}


public interface ISynchronouslyCompletable : ISynchronouslyFailable {    
    bool TrySetResult();
    void SetResult() {
        if (!TrySetResult()) {
            throw TaskAlreadyCompleted;
        }
    }
}

public interface ISynchronouslyCompletable<in TResult> : ISynchronouslyFailable {
    bool TrySetResult(TResult value);
    void SetResult(TResult value) {
        if (!TrySetResult(value)) {
            throw TaskAlreadyCompleted;
        }
    }
}

