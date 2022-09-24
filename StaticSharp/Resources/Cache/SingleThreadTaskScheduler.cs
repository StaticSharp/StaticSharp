using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

namespace StaticSharp.Gears;

public sealed class SingleThreadTaskScheduler : TaskScheduler {
    [ThreadStatic]
    private static bool isExecuting;
    private readonly CancellationToken cancellationToken;

    private readonly BlockingCollection<Task> taskQueue;

    public SingleThreadTaskScheduler(CancellationToken cancellationToken = default) {
        this.cancellationToken = cancellationToken;
        this.taskQueue = new BlockingCollection<Task>();
    }

    public SingleThreadTaskScheduler Start(string threadName = "SingleThreadTaskScheduler Thread") {
        new Thread(RunOnCurrentThread) { Name = threadName }.Start();
        return this;
    }

    // Just a helper for the sample code
    public Task Schedule(Action action) {
        return
            Task.Factory.StartNew
                (
                    action,
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    this
                );
    }

    public Task<T> Schedule<T>(Func<Task<T>> func) {

        
        return
            Task.Factory.StartNew
                (
                    ()=> {

                        var task = func();
                        //task.Wait();
                        return task.Result;
                     },
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    this
                ); ;
    }


    // You can have this public if you want - just make sure to hide it
    private void RunOnCurrentThread() {
        isExecuting = true;

        try {
            foreach (var task in taskQueue.GetConsumingEnumerable(cancellationToken)) {
                TryExecuteTask(task);
            }
        }
        catch (OperationCanceledException) { }
        finally {
            isExecuting = false;
        }
    }

    // Signaling this allows the task scheduler to finish after all tasks complete
    public void Complete() { taskQueue.CompleteAdding(); }
    protected override IEnumerable<Task> GetScheduledTasks() { return null; }

    protected override void QueueTask(Task task) {
        try {
            taskQueue.Add(task, cancellationToken);
        }
        catch (OperationCanceledException) { }
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) {
        // We'd need to remove the task from queue if it was already queued. 
        // That would be too hard.
        if (taskWasPreviouslyQueued) return false;

        return isExecuting && TryExecuteTask(task);
    }
}




